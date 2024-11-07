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

// Get references to the input and h3 elements
const fullnameInput = document.getElementById('fullname');
const nameDisplay = document.querySelector('.menu h3');

// Store the original span content
const originalSpan = nameDisplay.querySelector('span').outerHTML;

// Add event listener for input changes
fullnameInput.addEventListener('input', function() {
    // Get the current input value
    const newName = this.value.trim();
    
    // Update the h3 content, preserving the span
    if (newName) {
        nameDisplay.innerHTML = `${newName}<br>${originalSpan}`;
    } else {
        // If input is empty, you could either keep the old name or show a placeholder
        nameDisplay.innerHTML = `User Name<br>${originalSpan}`;
    }
});

// Optional: Initialize with existing value if form is pre-filled
document.addEventListener('DOMContentLoaded', function() {
    const initialName = fullnameInput.value.trim();
    if (initialName) {
        nameDisplay.innerHTML = `${initialName}<br>${originalSpan}`;
    }
});