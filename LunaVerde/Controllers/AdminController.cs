using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LunaVerde.Data;
using LunaVerde.Models;

namespace LunaVerde.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Получаем данные из таблиц
            var customers = _context.Customers.ToList();
            var reservations = _context.Reservations.ToList();
            var menu = _context.Menu.ToList();
            var order = _context.Orders.ToList();
            var orderItem = _context.OrderItems.ToList();

            // Передаём данные в представление через ViewBag или ViewModel
            ViewBag.Customers = customers;
            ViewBag.Reservations = reservations;
            ViewBag.Menu = menu;
            ViewBag.Orders = order;
            ViewBag.OrderItem = orderItem;
            return View();
        }
        private readonly LunaVerdeDBContext _context;
        public AdminController(LunaVerdeDBContext context)
        {
            _context = context;
        }
    }
}
