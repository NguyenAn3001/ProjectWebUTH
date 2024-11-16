function toggleNotifications() {
    const panel = document.getElementById('notificationsPanel');
    if (panel.style.display === 'block') {
        panel.style.display = 'none';
    } else {
        panel.style.display = 'block';
    }
}

// Dữ liệu từ Local Storage hoặc API
const data = {
    totalStudents: 1000,
    activePercentage: 75,
    meetingsPerMonth: [120, 150, 90, 96, 130, 145, 160, 110, 140, 175, 200, 190],
};

data.suspendedPercentage = 100 - data.activePercentage;

// Hàm cập nhật HTML từ dữ liệu
function updateStatCard(data) {
    // Cập nhật số tổng học sinh
    const totalStudentsElement = document.getElementById("total-students");
    totalStudentsElement.textContent = `Total number of students: ${data.totalStudents}`;

    // Cập nhật phần trăm Active
    const activePercentageElement = document.getElementById("active-percentage");
    activePercentageElement.textContent = `${data.activePercentage}%`;

    // Cập nhật phần trăm Studying
    const studyingPercentageElement = document.getElementById("studying-percentage");
    studyingPercentageElement.textContent = `Studying (${data.activePercentage}%)`;

    // Cập nhật phần trăm Suspended
    const suspendedPercentageElement = document.getElementById("suspended-percentage");
    suspendedPercentageElement.textContent = `Suspended (${data.suspendedPercentage}%)`;

    // Cập nhật nền pie chart
    const pieChart = document.getElementById("pie-chart");
    pieChart.style.background = `conic-gradient(var(--primary) 0% ${data.activePercentage}%, #e0e0e0 ${data.activePercentage}% 100%)`;
}

// Hàm cập nhật thống kê cuộc họp
function updateMeetingStats(data) {
    // Cập nhật tổng số cuộc họp trong tháng
    const monthlyMeetingCountElement = document.getElementById("monthly-meeting-count");
    monthlyMeetingCountElement.textContent = data.monthlyMeetings;

    // Cập nhật biểu đồ
    const barChartElement = document.getElementById("bar-chart");
    barChartElement.innerHTML = ""; // Xóa biểu đồ cũ trước khi cập nhật

    // Duyệt qua mảng meetingsPerMonth và tạo các phần tử cho biểu đồ
    data.meetingsPerMonth.forEach((value, index) => {
        const barItem = document.createElement("div");
        barItem.classList.add("bar-item");

        const bar = document.createElement("div");
        bar.classList.add("bar");
        bar.style.height = `${(value / Math.max(...data.meetingsPerMonth)) * 100}%`; // Tính chiều cao cột dựa trên tỷ lệ phần trăm

        const barValue = document.createElement("span");
        barValue.classList.add("bar-value");
        barValue.textContent = value;

        const barLabel = document.createElement("span");
        barLabel.classList.add("bar-label");
        barLabel.textContent = `T${index + 1}`;

        bar.appendChild(barValue);
        barItem.appendChild(bar);
        barItem.appendChild(barLabel);

        barChartElement.appendChild(barItem);
    });
}

// Tính tổng số cuộc họp trong tháng
data.monthlyMeetings = data.meetingsPerMonth.reduce((total, currentValue) => total + currentValue, 0);

// Gọi hàm để cập nhật dữ liệu
updateStatCard(data);
updateMeetingStats(data);
