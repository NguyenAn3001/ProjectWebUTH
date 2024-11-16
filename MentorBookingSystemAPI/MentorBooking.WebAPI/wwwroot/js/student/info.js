function saveUserInfo(event) {
    event.preventDefault();
    // Lấy dữ liệu từ các trường nhập
    const email = document.getElementById("email").value;
    const firstName = document.getElementById("firstName").value;
    const lastName = document.getElementById("lastName").value;
    const dob = document.getElementById("dob").value;
  
    // Lưu dữ liệu vào Local Storage
    localStorage.setItem("email", email);
    localStorage.setItem("firstName", firstName);
    localStorage.setItem("lastName", lastName);
    localStorage.setItem("dob", dob);
  
    // Chuyển hướng tới trang hiển thị
    window.location.href = "info-user.html";
  }
  
   // Kiểm tra dữ liệu từ Local Storage
   const email = localStorage.getItem("email");
   const firstName = localStorage.getItem("firstName");
   const lastName = localStorage.getItem("lastName");
   const dob = localStorage.getItem("dob");
  
   if (email && firstName && lastName && dob) {
     // Nếu có dữ liệu, hiển thị thông tin người dùng
     document.getElementById("email-display").textContent = email;
     document.getElementById("firstName-display").textContent = firstName;
     document.getElementById("lastName-display").textContent = lastName;
     document.getElementById("dob-display").textContent = dob;
     document.getElementById("profile-details").style.display = "grid";
     document.getElementById("add-profile").style.display = "none";
   } else {
     // Nếu không có dữ liệu, hiển thị nút Add Profile
     document.getElementById("profile-details").style.display = "none";
     document.getElementById("add-profile").style.display = "grid";
   }
  