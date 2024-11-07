const profileButton = document.querySelector('.action .profile');
const menu = document.querySelector('.action .menu');

// Sự kiện mở/đóng menu khi nhấn nút profile
profileButton.addEventListener('click', (event) => {
  event.stopPropagation(); // Ngăn sự kiện lan ra ngoài
  menu.classList.toggle('active');
});

// Sự kiện để đóng menu khi nhấn bên ngoài
document.addEventListener('click', (event) => {
  if (!menu.contains(event.target) && !profileButton.contains(event.target)) {
    menu.classList.remove('active');
  }
});
