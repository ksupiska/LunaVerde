using LunaVerde.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using LunaVerde.Data;

namespace LunaVerde.Controllers
{
    public class AddingController : Controller
    {
        private readonly LunaVerdeDBContext _context;
        public AddingController(LunaVerdeDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var menu = _context.Menu.ToList();
            ViewBag.Menu = menu;
            return View();
        }
        [HttpPost]
        public IActionResult Index(string name, string description, decimal price, string item, IFormFile image)
        {
            var menu = _context.Menu.ToList();
            ViewBag.Menu = menu;

            // Проверка: есть ли загруженное изображение
            string imagePath = null;
            if (image != null)
            {
                // Сохранение изображения в wwwroot/images
                var filePath = Path.Combine("wwwroot/images", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                imagePath = $"/images/{image.FileName}";
            }

            // Логика для сохранения данных блюда в базе
            var dish = new Menu
            {
                Name = name,
                Description = description,
                Price = price,
                MenuItem = item,
                ImagePath = imagePath
            };

            // Добавление в базу данных (пример с Entity Framework)
            _context.Menu.Add(dish);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Поиск элемента по ID
            var menu = _context.Menu.FirstOrDefault(m => m.MenuId == id);

            if (menu != null)
            {
                // Удаление изображения, если оно существует
                if (!string.IsNullOrEmpty(menu.ImagePath))
                {
                    var imagePath = Path.Combine("wwwroot", menu.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Удаление записи из базы данных
                _context.Menu.Remove(menu);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
