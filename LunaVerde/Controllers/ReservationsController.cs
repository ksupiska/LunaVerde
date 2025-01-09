using Microsoft.AspNetCore.Mvc;
using LunaVerde.Models;
using Microsoft.EntityFrameworkCore;
using LunaVerde.Data;
using System;

namespace LunaVerde.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly LunaVerdeDBContext _context;
        public ReservationsController(LunaVerdeDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ReserveTable(string fullName, string email, string phone, DateTime reservationDateTime, int tableNumber, int guestsCount, string? specialRequests)
        {
            // Проверка: дата резервирования должна быть в будущем
            if (reservationDateTime <= DateTime.Now ||
                (reservationDateTime.Date == DateTime.Now.Date && reservationDateTime.TimeOfDay <= DateTime.Now.TimeOfDay))
            {
                ModelState.AddModelError("", "Reservation date and time must be in the future.");
                return View("Index"); // Возврат на форму, если дата некорректна
            }

            // Извлекаем время из даты резервирования
            TimeSpan reservationTime = reservationDateTime.TimeOfDay;

            // Проверка времени бронирования: между 9:00 и 23:00
            TimeSpan openingTime = new TimeSpan(9, 0, 0); // 9:00 AM
            TimeSpan closingTime = new TimeSpan(23, 0, 0); // 11:00 PM

            if (reservationTime < openingTime || reservationTime > closingTime)
            {
                ModelState.AddModelError("", "You can reserve a table only between 9:00 AM and 11:00 PM.");
                return View("Index");
            }

            // Проверяем, есть ли уже клиент с таким Email
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);

            if (customer == null)
            {
                // Если клиента нет, добавляем его
                customer = new Customer
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }

            // Если specialRequests = null, заменяем на значение по умолчанию
            if (string.IsNullOrWhiteSpace(specialRequests))
            {
                specialRequests = "No special requests";
            }

            // Создаём новую запись о резервировании
            var reservation = new Reservation
            {
                CustomerID = customer.CustomerID,
                ReservationDate = reservationDateTime.Date,          // Сохраняем дату
                ReservationTime = reservationDateTime.TimeOfDay,     // Сохраняем время
                TableNumber = tableNumber,
                GuestsCount = guestsCount,
                SpecialRequests = specialRequests
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            // Показываем сообщение пользователю
            TempData["Message"] = "Your table has been reserved!";
            return RedirectToAction("Index");
        }


    }
}
