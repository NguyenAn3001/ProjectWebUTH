// function saveUserInfo(event) {
//   event.preventDefault();
//   // Lấy dữ liệu từ các trường nhập
//   const phone = document.getElementById("phone").value;
//   const firstName = document.getElementById("firstName").value;
//   const lastName = document.getElementById("lastName").value;

//   // Lưu dữ liệu vào Local Storage
//   localStorage.setItem("phone", phone);
//   localStorage.setItem("firstName", firstName);
//   localStorage.setItem("lastName", lastName);


//   // Chuyển hướng tới trang hiển thị
//   window.location.href = "info-user.html";
// }

//  // Lấy dữ liệu từ Local Storage
// const firstName = localStorage.getItem("firstName");
// const lastName = localStorage.getItem("lastName");
// const phone = localStorage.getItem("phone");

// // Kiểm tra dữ liệu
// if (firstName && lastName && phone) {
//   // Gắn dữ liệu vào các phần tử HTML
//   document.getElementById("firstName-display").textContent = firstName;
//   document.getElementById("lastName-display").textContent = lastName;
//   document.getElementById("phone-display").textContent = phone;

//   // Hiển thị phần thông tin người dùng
//   document.getElementById("profile-details").style.display = "grid";
//   document.getElementById("add-profile").style.display = "none";
// } else {
//   // Nếu không có dữ liệu, hiển thị nút Add Profile
//   document.getElementById("profile-details").style.display = "none";
//   document.getElementById("add-profile").style.display = "grid";
// }

// Lưu thông tin người dùng qua API
const studentId = localStorage.getItem('studentId');
const accessToken = localStorage.getItem('accessToken');
function saveUserInfo(event) {
  event.preventDefault();

  // Lấy dữ liệu từ các trường nhập
  const name = document.getElementById("name").value;
  const phone = document.getElementById("phone").value.trim();
  const firstName = document.getElementById("firstName").value.trim();
  const lastName = document.getElementById("lastName").value.trim();

  // Kiểm tra nếu người dùng chưa nhập đủ thông tin
  if (!phone || !firstName || !lastName) {
    alert("Vui lòng điền đầy đủ thông tin.");
    return;
  }

  const data = {
    phone,
    firstName,
    lastName,
  };
}
//   // Gửi dữ liệu đến API
//   fetch(`http://localhost:5076/api/info/student?studentId=${studentId}`, {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     body: JSON.stringify(data),
//   })
//     .then((response) => {
//       if (!response.ok) {
//         throw new Error("Failed to save user information");
//       }
//       return response.json();
//     })
//     .then((responseData) => {
//       console.log("User information saved:", responseData);
//       alert("Thông tin đã được lưu thành công!");

//       // Chuyển hướng tới trang hiển thị thông tin
//       window.location.href = "info-user.html";
//     })
//     .catch((error) => {
//       console.error("Error saving user information:", error);
//       alert("Failed to save user information. Please try again.");
//     });
// }

// Lấy thông tin người dùng từ API
function fetchUserInfo() {
  fetch("http://localhost:5076/api/UserProfiles/my-student-profiles", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${accessToken}`
    },
  })
    .then((response) => {
      if (!response.ok) {
        if (response.status === 401) {
          throw new Error("Unauthorized access. Please log in.");
        } else if (response.status === 404) {
          throw new Error("User profile not found.");
        } else {
          throw new Error("Failed to fetch user profile");
        }
      }
      return response.json();
    })
    .then((data) => {
      console.log("Fetched user profile:", data);

      // Kiểm tra dữ liệu có đầy đủ hay không
      if (data && data.name && data.phone) {
        document.getElementById("name-display").textContent = data.name;
        document.getElementById("phone-display").textContent = data.phone;

        // Hiển thị thông tin người dùng
        document.getElementById("profile-details").style.display = "grid";
        document.getElementById("add-profile").style.display = "none";
      } else {
        // Không có thông tin người dùng, hiển thị form thêm mới
        document.getElementById("profile-details").style.display = "none";
        document.getElementById("add-profile").style.display = "grid";
      }
    })
    .catch((error) => {
      console.error("Error fetching user profile:", error);
      alert(error.message);
    });
}

// Gọi hàm fetchUserInfo() khi trang load
document.addEventListener("DOMContentLoaded", fetchUserInfo);
