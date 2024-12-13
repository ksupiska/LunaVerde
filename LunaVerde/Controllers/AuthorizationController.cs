using Microsoft.AspNetCore.Mvc;

namespace LunaVerde.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Очищаем ошибку при загрузке страницы
            ViewBag.Error = null;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            // Проверяем имя пользователя и пароль
            if (username == "admin" && password == "1111")
            {
                // Если данные верны, перенаправляем на страницу администратора
                return RedirectToAction("Dashboard", "Admin"); // предполагается, что контроллер Admin существует
            }
            else
            {
                // Если данные неверны, показываем ошибку
                ViewBag.Error = "Invalid username or password!";
                return View();
            }
        }
    }
}
