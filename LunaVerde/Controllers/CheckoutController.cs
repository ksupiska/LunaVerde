using LunaVerde.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LunaVerde.Data;

namespace LunaVerde.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly LunaVerdeDBContext _context;

        public CheckoutController(LunaVerdeDBContext context)
        {
            _context = context;
        }

        //просмотр корзины
        public IActionResult Index()
        {
            // Получаем текущую корзину из сессии
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            // Передаём корзину в представление
            return View(cart);
        }

        // Метод для добавления блюда в корзину
        [HttpPost]
        public IActionResult AddToCart(int menuId)
        {
            // Получаем текущую корзину из сессии
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Ищем блюдо в корзине
            var existingItem = cart.FirstOrDefault(item => item.MenuId == menuId);
            if (existingItem != null)
            {
                // Если блюдо уже есть, увеличиваем количество
                existingItem.Quantity++;
            }
            else
            {
                // Если блюда нет, добавляем его
                var menuItem = _context.Menu.FirstOrDefault(m => m.MenuId == menuId);
                if (menuItem != null)
                {
                    cart.Add(new CartItem
                    {
                        MenuId = menuItem.MenuId,
                        Name = menuItem.Name,
                        Description = menuItem.Description,
                        ImagePath = menuItem.ImagePath,
                        Price = menuItem.Price,
                        Quantity = 1
                    });
                }
                else
                {
                    // Логирование ошибки
                    Console.WriteLine($"Menu item with ID {menuId} not found.");
                }
            }

            // Сохраняем обновлённую корзину в сессию
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Перенаправляем пользователя обратно
            return RedirectToAction("Breakfast", "Menu");
        }


        // Метод для изменения количества блюда
        [HttpPost]
        public IActionResult UpdateCart(int menuId, int quantity)
        {
            // Получаем текущую корзину из сессии
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (cart != null)
            {
                // Ищем элемент в корзине по MenuId
                var item = cart.FirstOrDefault(c => c.MenuId == menuId);
                if (item != null)
                {
                    // Обновляем количество
                    if (quantity > 0)
                    {
                        item.Quantity = quantity;
                    }
                    else
                    {
                        // Если количество 0, удаляем элемент из корзины
                        cart.Remove(item);
                    }
                }

                // Сохраняем обновленную корзину в сессию
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            // Перезагружаем страницу корзины
            return RedirectToAction("Index");
        }


        // Метод для оформления заказа
        [HttpPost]
        public IActionResult Checkout(string customerName, string phoneNumber)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (cart.Any())
            {
                // Создаем новый заказ
                var order = new Order
                {
                    CustomerName = customerName,
                    PhoneNumber = phoneNumber,
                    OrderDate = DateTime.Now,
                    TotalPrice = cart.Sum(c => c.Price * c.Quantity)
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Добавляем элементы заказа
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        MenuId = item.MenuId,
                        Quantity = item.Quantity
                    };

                    _context.OrderItems.Add(orderItem);
                }

                _context.SaveChanges();

                // Очищаем корзину
                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("OrderSuccess");
        }

        // Страница оформления заказа (запись в бд должна происходить)
        public IActionResult OrderSuccess()
        {
            return View();
        }
        //страница успешного заказа!!!
        public ActionResult Order() 
        { 
            return View();
        }

        //добавление в бд
        [HttpPost]
        public IActionResult PlaceOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Получаем текущую корзину
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                if (cart == null || !cart.Any())
                {
                    return RedirectToAction("Cart");
                }

                // Создаем новый заказ
                var order = new Order
                {
                    CustomerName = model.CustomerName,
                    PhoneNumber = model.PhoneNumber,
                    OrderDate = DateTime.Now,
                    TotalPrice = cart.Sum(c => c.Price * c.Quantity) // Общая сумма заказа
                };

                // Сохраняем заказ в базу данных
                _context.Orders.Add(order);
                _context.SaveChanges(); // После сохранения OrderId будет сгенерирован автоматически

                // Сохраняем позиции заказа в таблице OrderItems
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId, // Связываем с созданным заказом
                        MenuId = item.MenuId,
                        Quantity = item.Quantity
                    };

                    _context.OrderItems.Add(orderItem);
                }

                _context.SaveChanges(); // Сохраняем все OrderItems в базу данных

                // Логируем сохраненный заказ
                Console.WriteLine($"OrderId: {order.OrderId}, CustomerName: {order.CustomerName}");

                // Очищаем корзину
                HttpContext.Session.Remove("Cart");

                // Перенаправляем на страницу успешного заказа
                return RedirectToAction("OrderSuccess");
            }

            // Если данные неверные, возвращаем форму
            return View("Checkout", model);
        }


    }
}
