using Microsoft.AspNetCore.Mvc;
using LunaVerde.Models;
using Microsoft.EntityFrameworkCore;
using LunaVerde.Data;
using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace LunaVerde.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ILogger<ReservationsController> _logger;
        public IActionResult Index()
        {
            return View();
        }

        private readonly LunaVerdeDBContext _context;
        public ReservationsController(LunaVerdeDBContext context, ILogger<ReservationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult ReserveTable(string fullName, string email, string phone, string reservationDateTime, int tableNumber, int guestsCount, string? specialRequests)
        {
            // Парсим строку даты и времени
            if (!DateTime.TryParse(reservationDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
            {
                TempData["ErrorMessage"] = "Invalid date and time format. Please try again.";
                return RedirectToAction("Index");
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
                ReservationDate = parsedDateTime.Date,          // Сохраняем дату
                ReservationTime = parsedDateTime.TimeOfDay,      // Сохраняем время
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
