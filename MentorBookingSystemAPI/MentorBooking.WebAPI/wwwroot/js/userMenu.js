document.getElementById('menu-btn').addEventListener('click', function() {
    const userMenu = document.getElementById('user-menu');
    userMenu.classList.toggle('active'); // Thay đổi lớp active
});

// Đóng menu khi nhấp ra ngoài
window.addEventListener('click', function(event) {
    const userMenu = document.getElementById('user-menu');
    const menuButton = document.getElementById('menu-btn');

    // Kiểm tra nếu nhấp ra ngoài nút menu và menu
    if (!menuButton.contains(event.target) && !userMenu.contains(event.target)) {
        userMenu.classList.remove('active'); // Ẩn menu
    }
});
