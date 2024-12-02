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
        public IActionResult ReserveTable(string fullName, string email, string phone, DateTime reservationDate, int tableNumber, int guestsCount, string? specialRequests)
        {
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

            // Создаём новую запись о резервировании
            var reservation = new Reservation
            {
                CustomerID = customer.CustomerID,
                ReservationDate = reservationDate,
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
