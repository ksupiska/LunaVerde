// Обработчик клика на иконку бургер
// Получаем элементы
const burgerIcon = document.getElementById('burger-icon');
const mobileMenu = document.getElementById('mobile-menu');

// При клике на бургер-иконку открываем/закрываем мобильное меню
burgerIcon.addEventListener('click', () => {
    mobileMenu.classList.toggle('open');
});

