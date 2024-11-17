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
async function fetchStudentList() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/get-all-student`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch student list");
    const students = await response.json();

    // Lấy tbody của bảng student
    const studentTableBody = document.querySelector(
      ".table-container table tbody"
    );
    studentTableBody.innerHTML = ""; // Xóa dữ liệu cũ

    // Thêm dữ liệu mới
    students.forEach((studentItem) => {
      // Kiểm tra status thành công và có data
      if (studentItem.status === "Succes" && studentItem.data) {
        const student = studentItem.data;
        const createDate = new Date(student.createAt).toLocaleDateString();

        const row = document.createElement("tr");
        row.innerHTML = `
                    <td>${student.studentId}</td>
                    <td>${student.userName || "-"}</td>
                    <td>${student.email}</td>
                    <td>${student.countGroup}</td>
                    <td>${student.pointBalance}</td>
                `;
        studentTableBody.appendChild(row);
      }
    });

  } catch (error) {
    console.error("Error fetching student list:", error);
    // Hiển thị thông báo lỗi cho người dùng
    const studentTableBody = document.querySelector(
      ".table-container table tbody"
    );
    studentTableBody.innerHTML = `
            <tr>
                <td colspan="5" class="error-message">
                    Error loading student data. Please try again later.
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
        fetchStudentList()
    ]).catch(error => {
        console.error('Error initializing dashboard:', error);
        alert('Error loading dashboard data. Please refresh the page.');
    });
}

document.addEventListener("DOMContentLoaded", initializeDashboard);
