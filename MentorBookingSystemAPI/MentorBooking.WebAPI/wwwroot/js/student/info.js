function saveUserInfo(event) {
  event.preventDefault();
  // Lấy dữ liệu từ các trường nhập
  const numberPhone = document.getElementById("numberPhone").value;
  const firstName = document.getElementById("firstName").value;
  const lastName = document.getElementById("lastName").value;

  // Lưu dữ liệu vào Local Storage
  localStorage.setItem("numberPhone", numberPhone);
  localStorage.setItem("firstName", firstName);
  localStorage.setItem("lastName", lastName);


  // Chuyển hướng tới trang hiển thị
  window.location.href = "info-user.html";
}

 // Lấy dữ liệu từ Local Storage
const firstName = localStorage.getItem("firstName");
const lastName = localStorage.getItem("lastName");
const numberPhone = localStorage.getItem("numberPhone");

// Kiểm tra dữ liệu
if (firstName && lastName && numberPhone) {
  // Gắn dữ liệu vào các phần tử HTML
  document.getElementById("firstName-display").textContent = firstName;
  document.getElementById("lastName-display").textContent = lastName;
  document.getElementById("numberPhone-display").textContent = numberPhone;

  // Hiển thị phần thông tin người dùng
  document.getElementById("profile-details").style.display = "block";
  document.getElementById("add-profile").style.display = "none";
} else {
  // Nếu không có dữ liệu, hiển thị nút Add Profile
  document.getElementById("profile-details").style.display = "none";
  document.getElementById("add-profile").style.display = "block";
}
