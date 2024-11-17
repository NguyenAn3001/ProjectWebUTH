function toggleNotifications() {
    const panel = document.getElementById('notificationsPanel');
    if (panel.style.display === 'block') {
        panel.style.display = 'none';
    } else {
        panel.style.display = 'block';
    }
}

const API_BASE_URL = "http://localhost:5076/api";
const token = localStorage.getItem("accessToken");

// Headers configuration
const headers = {
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
};

// Fetch tổng số student
async function fetchTotalStudents() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/numbers-student`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch student count");
    const data = await response.json();

    // Cập nhật UI với số lượng student
    document.querySelector(
      ".stat-card:first-child h3"
    ).textContent = `Total number of students: ${data}`;
  } catch (error) {
    console.error("Error fetching student count:", error);
  }
}

// Fetch tổng số mentor
async function fetchTotalMentors() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/numbers-mentor`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch mentor count");
    const data = await response.json();

    // Cập nhật UI với số lượng mentor
    document.querySelector(
      ".stat-card:nth-child(3) h3"
    ).textContent = `Total number of mentors: ${data}`;
  } catch (error) {
    console.error("Error fetching mentor count:", error);
  }
}

// Fetch danh sách student
// Fetch danh sách student
async function fetchStudentList() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/get-all-student`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch student list");
    const students = await response.json();

    const studentTableBody = document.querySelector(
      ".table-container table tbody"
    );
    studentTableBody.innerHTML = ""; // Xóa dữ liệu cũ

    const labels = [];
    const data = [];

    students.forEach((studentItem) => {
      if (studentItem.status === "Succes" && studentItem.data) {
        const student = studentItem.data;
        const createDate = new Date(student.createAt).toLocaleDateString();

        labels.push(student.userName || "Unknown");
        data.push(student.pointBalance);

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

    renderChart(labels, data);
  } catch (error) {
    console.error("Error fetching student list:", error);
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

function renderChart(labels, data) {
  const ctx = document.getElementById("studentChart").getContext("2d");
  new Chart(ctx, {
    type: "bar", // Loại biểu đồ
    data: {
      labels: labels, // Tên sinh viên
      datasets: [
        {
          label: "Wallet Balance",
          data: data, // Số dư điểm
          backgroundColor: "#6c7b6c",
          borderColor: "#6c7b6c",
          borderWidth: 1,
        },
      ],
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
        },
      },
    },
  });
}

// Hàm cập nhật biểu đồ tròn
function updatePieChart(totalStudents) {
    // Giả định tỷ lệ active là 75% (có thể điều chỉnh theo logic thực tế)
    const activePercentage = 75;
    const pieChartValue = document.querySelector('.pie-chart-value span');
    if (pieChartValue) {
        pieChartValue.textContent = `${activePercentage}%`;
    }

    // Cập nhật legend
    const activeLegend = document.querySelector('.legend-item:first-child span:last-child');
    const inactiveLegend = document.querySelector('.legend-item:last-child span:last-child');
    if (activeLegend && inactiveLegend) {
        const activeCount = Math.round(totalStudents * activePercentage / 100);
        const inactiveCount = totalStudents - activeCount;
        activeLegend.textContent = `Studying (${activeCount} - ${activePercentage}%)`;
        inactiveLegend.textContent = `Suspended (${inactiveCount} - ${100 - activePercentage}%)`;
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

// Fetch danh sách mentor
async function fetchMentorList() {
  try {
    const response = await fetch(`${API_BASE_URL}/Admin/get-all-mentor`, {
      method: "GET",
      headers: headers,
    });

    if (!response.ok) throw new Error("Failed to fetch mentor list");
    const mentors = await response.json();

    const mentorTableBody = document.querySelector(
      ".table-container:last-child tbody"
    );
    mentorTableBody.innerHTML = ""; // Clear old data

    // Prepare data for the chart
    const mentorNames = [];
    const mentorRatings = [];

    mentors.forEach((mentorItem) => {
      if (mentorItem.status === "Succes" && mentorItem.data) {
        const mentor = mentorItem.data;

        // Add to chart data
        mentorNames.push(mentor.userName || "Unknown");
        mentorRatings.push(mentor.ratings);

        // Add to table
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

    // Render the chart
    renderRatingChart(mentorNames, mentorRatings);
  } catch (error) {
    console.error("Error fetching mentor list:", error);
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

function renderRatingChart(names, ratings) {
  const ctx = document.getElementById("mentorRatingChart").getContext("2d");
  new Chart(ctx, {
    type: "bar", // Use bar chart
    data: {
      labels: names,
      datasets: [
        {
          label: "Mentor Ratings",
          data: ratings,
          backgroundColor: "#6c7b6c",
          borderColor: "#6c7b6c",
          borderWidth: 1,
        },
      ],
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          max: 5, // Ratings are out of 5
        },
      },
    },
  });
}

// Hàm cập nhật biểu đồ đường cho mentor
function updateMentorLineChart(totalMentors) {
    // Lấy các điểm trên biểu đồ
    const linePoints = document.querySelectorAll('.line-point');
    
    // Tính toán giá trị cho từng điểm (ví dụ: phân bố đều)
    const step = totalMentors / (linePoints.length - 1);
    
    linePoints.forEach((point, index) => {
        const value = Math.round(step * index);
        const tooltip = point.querySelector('.tooltip');
        if (tooltip) {
            tooltip.textContent = value;
        }
    });
}

// Thêm function khởi tạo dashboard khi trang load
function initializeDashboard() {
    Promise.all([
        fetchTotalStudents(),
        fetchTotalMentors(),
        fetchStudentList(),
        fetchMentorList()
    ]).catch(error => {
        console.error('Error initializing dashboard:', error);
        alert('Error loading dashboard data. Please refresh the page.');
    });
}

// Thêm chức năng sort cho các cột trong bảng
function addSortingFunctionality() {
    const tables = document.querySelectorAll('.table-container table');
    
    tables.forEach(table => {
        const headers = table.querySelectorAll('th');
        headers.forEach((header, index) => {
            header.style.cursor = 'pointer';
            header.addEventListener('click', () => sortTable(table, index));
        });
    });
}

// Hàm sort table
function sortTable(table, column) {
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    const isNumeric = column === 3 || column === 4; // Cột số lượng học sinh và rating

    rows.sort((a, b) => {
        const aValue = a.cells[column].textContent.trim();
        const bValue = b.cells[column].textContent.trim();

        if (isNumeric) {
            return parseFloat(aValue) - parseFloat(bValue);
        }
        return aValue.localeCompare(bValue);
    });

    // Toggle sort direction
    if (table.dataset.sortColumn === column.toString()) {
        rows.reverse();
        table.dataset.sortColumn = '';
    } else {
        table.dataset.sortColumn = column;
    }

    // Update table
    tbody.innerHTML = '';
    rows.forEach(row => tbody.appendChild(row));
}

// Event listeners
document.addEventListener("DOMContentLoaded", () => {
  initializeDashboard();

  // Thêm xử lý đăng xuất
  const logoutBtn = document.querySelector(".dropdown-item");
  logoutBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${API_BASE_URL}/auth/logout`, {
        method: "POST",
        headers: headers,
      });

      if (response.ok) {
        localStorage.removeItem("accessToken");
        window.location.href = "../../../index.html";
      }
    } catch (error) {
      console.error("Logout failed:", error);
    }
  });
});
