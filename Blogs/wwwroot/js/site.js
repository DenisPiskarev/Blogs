$(document).ready(function () {
    // Получаем текущий URL страницы
    var currentUrl = window.location.pathname;

    // Находим кнопку с href, соответствующим текущему URL
    $('.button[href="' + currentUrl + '"]').addClass('active');
});