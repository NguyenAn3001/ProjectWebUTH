const modal = document.getElementById("mentorModal");
const buttons = document.querySelectorAll(".btn");

// Modal display functions
function showModal() {
  modal.style.display = "block";
}

function closeModal() {
  modal.style.display = "none";
}

function showMentorForm() {
  closeModal();
  window.location.href = "register.html";
}

modal.style.display = "none";

function validateSignup() {
  if (validateUsername() && validatePassword() && validateConfirmPassword()) {
      alert('Đăng kí thành công!');
      showModal(); 
  } else {
      alert('Vui lòng nhập đầy đủ thông tin.');
  }
}

function validateLogin() {
  const logUsername = document.getElementById("logusernameLogin").value.trim();
  const password = document.getElementById("logpassLogin").value.trim();
  if (logUsername && password) {
      alert('Đăng nhập thành công!');
      showModal();
  } else {
      alert('Vui lòng nhập đầy đủ thông tin.');
  }
}

function validateUsername() {
  const username = document.getElementById('username').value.trim();
  const regex = /^[a-zA-Z0-9_.-]{6,20}$/;
  return regex.test(username);
}

function validateEmail() {
  const logUsername = document.getElementById("logusernameSignup").value.trim();
  const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  return regex.test(email);
}

function validatePassword() {
  const password = document.getElementById("logpassSignup").value.trim();
  const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/;
  return regex.test(password);
}

function validateConfirmPassword() {
  const password = document.getElementById("logpassSignup").value.trim();
  const confirmPassword = document.getElementById("confirmPass").value.trim();
  return password === confirmPassword;
}
