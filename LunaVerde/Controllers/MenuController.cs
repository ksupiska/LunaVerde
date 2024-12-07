using Microsoft.AspNetCore.Mvc;

namespace LunaVerde.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Breakfast()
        {
            return View();
        }
    }
}
