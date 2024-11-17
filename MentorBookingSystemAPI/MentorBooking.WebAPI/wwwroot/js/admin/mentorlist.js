function toggleNotifications() {
  const panel = document.getElementById("notificationsPanel");
  if (panel.style.display === "block") {
    panel.style.display = "none";
  } else {
    panel.style.display = "block";
  }
}

const API_BASE_URL = "http://localhost:5076/api";
const token = localStorage.getItem("accessToken");

// Headers configuration
const headers = {
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
};
async function fetchMentorList() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/get-all-mentor`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch mentor list");
    const mentors = await response.json();

    // Lấy tbody của bảng mentor
    const mentorTableBody = document.querySelector(
      ".table-container:last-child tbody"
    );
    mentorTableBody.innerHTML = ""; // Xóa dữ liệu cũ

    // Thêm dữ liệu mới
    mentors.forEach((mentorItem) => {
      // Kiểm tra status thành công và có data
      if (mentorItem.status === "Succes" && mentorItem.data) {
        const mentor = mentorItem.data;
        const createDate = new Date(mentor.createAt).toLocaleDateString();

        const row = document.createElement("tr");
        row.innerHTML = `
                    <td>${mentor.mentorId}</td>
                    <td>${mentor.userName || "-"}</td>
                    <td>${mentor.skills}</td>
                    <td>${mentor.countSession}</td>
                    <td>${mentor.ratings}/5</td>
                `;
        mentorTableBody.appendChild(row);
      }
    });


  } catch (error) {
    console.error("Error fetching mentor list:", error);
    // Hiển thị thông báo lỗi cho người dùng
    const mentorTableBody = document.querySelector(
      ".table-container:last-child tbody"
    );
    mentorTableBody.innerHTML = `
            <tr>
                <td colspan="5" class="error-message">
                    Error loading mentor data. Please try again later.
                </td>
            </tr>
        `;
  }
}


// Thêm CSS để hiển thị thông báo lỗi
const style = document.createElement('style');
style.textContent = `
    .error-message {
        color: #e74c3c;
        text-align: center;
        padding: 1rem;
        font-style: italic;
    }
`;
document.head.appendChild(style);

// Thêm function khởi tạo dashboard khi trang load
function initializeDashboard() {
    Promise.all([
        fetchMentorList()
    ]).catch(error => {
        console.error('Error initializing dashboard:', error);
        alert('Error loading dashboard data. Please refresh the page.');
    });
}

document.addEventListener("DOMContentLoaded", initializeDashboard);
