// Обработчик клика на иконку бургер
// Получаем элементы
const burgerIcon = document.getElementById('burger-icon');
const mobileMenu = document.getElementById('mobile-menu');

// При клике на бургер-иконку открываем/закрываем мобильное меню
burgerIcon.addEventListener('click', () => {
    mobileMenu.classList.toggle('open');
});


//ДЛЯ КОРЗИНЫ
document.addEventListener("DOMContentLoaded", () => {
    const cart = {};

    // Увеличение количества
    document.querySelectorAll(".increment").forEach((button) => {
        button.addEventListener("click", (e) => {
            const menuId = e.target.getAttribute("data-id");
            const quantityElem = document.querySelector(`.quantity[data-id='${menuId}']`);
            const quantity = parseInt(quantityElem.textContent) || 0;

            quantityElem.textContent = quantity + 1;
            cart[menuId] = (cart[menuId] || 0) + 1;
        });
    });

    // Уменьшение количества
    document.querySelectorAll(".decrement").forEach((button) => {
        button.addEventListener("click", (e) => {
            const menuId = e.target.getAttribute("data-id");
            const quantityElem = document.querySelector(`.quantity[data-id='${menuId}']`);
            const quantity = parseInt(quantityElem.textContent) || 0;

            if (quantity > 0) {
                quantityElem.textContent = quantity - 1;
                cart[menuId] = (cart[menuId] || 0) - 1;

                if (cart[menuId] <= 0) {
                    delete cart[menuId];
                }
            }
        });
    });

    // Добавление в корзину
    document.querySelectorAll(".add-to-cart").forEach((button) => {
        button.addEventListener("click", () => {
            console.log("Текущая корзина:", cart);
            alert("Блюдо добавлено в корзину!");
        });
    });
});

//отображение корзины
document.getElementById("view-cart").addEventListener("click", () => {
    const cartItems = Object.entries(cart)
        .map(([id, quantity]) => `Menu ID: ${id}, Quantity: ${quantity}`)
        .join("\n");

    alert(`Your Cart:\n${cartItems || "Cart is empty!"}`);
});

//для отправки заказа
document.getElementById("checkout-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const customerName = document.getElementById("customerName").value;
    const phoneNumber = document.getElementById("phoneNumber").value;

    // Формируем данные заказа
    const order = {
        customerName: customerName,
        phoneNumber: phoneNumber,
        orderItems: Object.entries(cart).map(([menuId, quantity]) => ({
            menuId: parseInt(menuId),
            quantity: quantity,
            price: getPriceById(menuId) // Предполагаем, что есть функция для получения цены
        }))
    };

    // Отправляем заказ на сервер
    const response = await fetch("/Order/PlaceOrder", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(order)
    });

    if (response.ok) {
        alert("Order placed successfully!");
        location.reload();
    } else {
        alert("Failed to place order.");
    }
});

//для отбражения заказов
async function fetchOrders() {
    const response = await fetch("/Order/GetOrders");
    if (response.ok) {
        const orders = await response.json();
        console.log(orders);
        // Отобразить заказы на странице
    }
}
fetchOrders();
