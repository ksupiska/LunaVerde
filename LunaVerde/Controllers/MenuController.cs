using Microsoft.AspNetCore.Mvc;
using LunaVerde.Data;

namespace LunaVerde.Controllers
{
    public class MenuController : Controller
    {
        private readonly LunaVerdeDBContext _context;
        public MenuController(LunaVerdeDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Breakfast()
        {
            // Получение данных из базы данных
            // Получение списка блюд из базы данных
            var breakfastItems = _context.Menu.Where(m => m.MenuItem == "Breakfast").ToList();

            return View(breakfastItems);
        }

        public async Task<IActionResult> Lunch()
        {
            // Получение данных из базы данных
            // Получение списка блюд из базы данных
            var lunchItems = _context.Menu.Where(m => m.MenuItem == "Lunch").ToList();
            return View(lunchItems);
        }
        public async Task<IActionResult> Dinner()
        {
            // Получение данных из базы данных
            // Получение списка блюд из базы данных
            var dinnerItems = _context.Menu.Where(m => m.MenuItem == "Dinner").ToList();
            return View(dinnerItems);
        }
        public async Task<IActionResult> Drink()
        {
            // Получение данных из базы данных
            // Получение списка блюд из базы данных
            var drinkItems = _context.Menu.Where(m => m.MenuItem == "Drink").ToList();
            return View(drinkItems);
        }
    }
}
