function saveUserInfo(event) {
  event.preventDefault();
  // Lấy dữ liệu từ các trường nhập
  const email = document.getElementById("umberPhone").value;
  const firstName = document.getElementById("firstName").value;
  const lastName = document.getElementById("lastName").value;

  // Lưu dữ liệu vào Local Storage
  localStorage.setItem("numberPhone", numberPhone);
  localStorage.setItem("firstName", firstName);
  localStorage.setItem("lastName", lastName);


  // Chuyển hướng tới trang hiển thị
  window.location.href = "info-user.html";
}

 // Kiểm tra dữ liệu từ Local Storage
 const email = localStorage.getItem("umberPhone");
 const firstName = localStorage.getItem("firstName");
 const lastName = localStorage.getItem("lastName");

 if (email && firstName && lastName && dob) {
   // Nếu có dữ liệu, hiển thị thông tin người dùng
   document.getElementById("numberPhone-display").textContent = numberPhone;
   document.getElementById("firstName-display").textContent = firstName;
   document.getElementById("lastName-display").textContent = lastName;
   document.getElementById("profile-details").style.display = "grid";
   document.getElementById("add-profile").style.display = "none";
 } else {
   // Nếu không có dữ liệu, hiển thị nút Add Profile
   document.getElementById("profile-details").style.display = "none";
   document.getElementById("add-profile").style.display = "grid";
 }
