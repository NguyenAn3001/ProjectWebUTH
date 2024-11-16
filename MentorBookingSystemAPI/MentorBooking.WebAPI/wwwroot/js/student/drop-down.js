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

document.addEventListener("DOMContentLoaded", function() {
  const firstName = localStorage.getItem("firstName");
  const lastName = localStorage.getItem("lastName");
  const role = localStorage.getItem("role");

  if (firstName && lastName && role) {
      const usernameDisplay = document.getElementById("username-display");
      usernameDisplay.innerHTML = `
          ${lastName} ${firstName}<br><span>${role}</span>
      `;
  }
});

function logout() {
  localStorage.clear();
  window.location.href = "../../views/login/login.html";
}
