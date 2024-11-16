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
})

function fetchUserInfo() {
  const accessToken = localStorage.getItem('accessToken'); // Lấy token từ localStorage
  console.log("Access Token:", accessToken);

  fetch("http://localhost:5076/api/UserProfiles/my-student-profiles", {
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
        document.getElementById("name-user").textContent = user.name;
      }

    })
    .catch((error) => {
      console.error("Error fetching user profile:", error);
      console.log(error.message);
    });
}

// Gọi hàm fetchUserInfo() khi trang load
document.addEventListener("DOMContentLoaded", fetchUserInfo);
