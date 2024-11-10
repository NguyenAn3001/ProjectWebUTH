const profileButton = document.querySelector('.action .profile');
const menu = document.querySelector('.action .menu');

profileButton.addEventListener('click', (event) => {
  event.stopPropagation(); 
  menu.classList.toggle('active');
});
document.addEventListener('click', (event) => {
  if (!menu.contains(event.target) && !profileButton.contains(event.target)) {
    menu.classList.remove('active');
  }
});
