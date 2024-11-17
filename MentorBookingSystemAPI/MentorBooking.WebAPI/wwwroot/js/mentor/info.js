function fetchUserInfo() {
    const accessToken = localStorage.getItem('accessToken'); // Lấy token từ localStorage
    console.log("Access Token:", accessToken);
  
    if (!accessToken) {
      console.error("Access token is missing. Please log in.");
      alert("Bạn cần đăng nhập để lấy thông tin người dùng.");
      return;
    }
  
    fetch("http://localhost:5076/api/UserProfiles/my-mentor-profiles", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${accessToken}`
      },
    })
      .then((response) => {
        console.log("Response status:", response.status); // Log trạng thái phản hồi từ API
  
        if (!response.ok) {
          if (response.status === 401) {
            throw new Error("Unauthorized access. Please log in.");
          } else if (response.status === 404) {
            throw new Error("User profile not found.");
          } else {
            throw new Error(`Failed to fetch user profile (status: ${response.status})`);
          }
        }
        return response.json();
      })
      .then((data) => {
        console.log("Fetched user profile:", data);
  
        if (data && data.status === "Success") {
          const user = data.data; // Truy cập dữ liệu người dùng
          console.log("User data:", user);
  
          // Hiển thị thông tin người dùng
          document.getElementById("name-display").textContent = user.name;
          document.getElementById("phone-display").textContent = user.phone;
          document.getElementById("profile-details").style.display = "grid";
          document.getElementById("add-profile").style.display = "none";
  
          // Hiển thị studentID vào userID-Display
          document.getElementById("userId-display").textContent = user.studentId; // Xuất studentID
        } else {
          // Không có dữ liệu hợp lệ
          console.error("Invalid API response:", data.message);
          document.getElementById("profile-details").style.display = "none";
          document.getElementById("add-profile").style.display = "grid";
        }
      })
      .catch((error) => {
        console.error("Error fetching user profile:", error);
        console.log(error.message);
      });
  }
  
  // Gọi hàm fetchUserInfo() khi trang load
  document.addEventListener("DOMContentLoaded", fetchUserInfo);
  