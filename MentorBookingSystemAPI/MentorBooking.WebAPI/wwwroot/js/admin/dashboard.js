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
localStorage.setItem('data', JSON.stringify(data));
// Thêm dữ liệu mẫu vào localStorage
const sampleNotifications = [
    {
      icon: 'fas fa-user-graduate',
      title: 'Có một sinh viên mới đã đăng ký',
      time: '5 minutes ago',
    },
    {
      icon: 'fas fa-calendar-check',
      title: 'New appointment',
      time: '30 minutes ago',
    },
  ];
  
localStorage.setItem('notifications', JSON.stringify(sampleNotifications));
  


data.suspendedPercentage = 100 - data.activePercentage;

const statsData = {
    walletBalance: 7890,
    progress: {
      learning: { percentage: 75, value: 5917 },
      exchange: { percentage: 45, value: 1184 },
      other: { percentage: 30, value: 789 },
    },
  };

  localStorage.setItem('statsData', JSON.stringify(statsData));

  const studentsData = [
    { id: "001", name: "Trần Hil", email: "tdhil@email.com", meetings: 5, balance: 100 },
    { id: "002", name: "Nguyễn Mai", email: "mai@email.com", meetings: 3, balance: 75 },
    { id: "003", name: "Lê Bình", email: "binh@email.com", meetings: 8, balance: 150 },
    // Bạn có thể thêm nhiều sinh viên nữa
  ];
  localStorage.setItem('studentsData', JSON.stringify(studentsData));

  const mentorsData = [
    { id: "M001", name: "Kai Havertz", major: "Frontend", studentQuantity: 15, rating: "4.8/5" },
    { id: "M002", name: "Kai Cenat", major: "Backend", studentQuantity: 12, rating: "4.6/5" },
    { id: "M003", name: "John Doe", major: "Fullstack", studentQuantity: 10, rating: "4.5/5" },
    // Thêm nhiều mentor vào đây nếu cần
  ];
  
  localStorage.setItem('mentorsData', JSON.stringify(mentorsData));


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

function renderNotifications() {
 const notificationsPanel = document.getElementById('notificationsPanel');
 const storedNotifications = JSON.parse(localStorage.getItem('notifications')) || [];

 // Xóa thông báo cũ trong bảng
 notificationsPanel.innerHTML = '';

 // Kiểm tra nếu không có thông báo
 if (storedNotifications.length === 0) {
   notificationsPanel.innerHTML = '<div class="notification-item">No notifications</div>';
   return;
 }

 // Tạo thông báo mới từ dữ liệu
 storedNotifications.forEach((notification) => {
   const notificationItem = document.createElement('div');
   notificationItem.classList.add('notification-item');

   notificationItem.innerHTML = `
     <div class="notification-icon">
       <i class="${notification.icon}"></i>
     </div>
     <div class="notification-content">
       <div class="notification-title">${notification.title}</div>
       <div class="notification-time">${notification.time}</div>
     </div>
   `;

   notificationsPanel.appendChild(notificationItem);
 });
}

function updateNotificationBadge() {
 const storedNotifications = JSON.parse(localStorage.getItem('notifications')) || [];
 const badge = document.querySelector('.notification-badge');
 badge.textContent = storedNotifications.length > 0 ? storedNotifications.length : '';
}
  

document.addEventListener('DOMContentLoaded', () => {
 // Cập nhật số lượng thông báo
 updateNotificationBadge();

 // Tự động hiển thị thông báo nếu cần
 renderNotifications();
});
  

function updateStats(data) {
  // Cập nhật Wallet balance
  document.getElementById('walletBalance').innerText = `Wallet balance used: ${data.walletBalance}`;

  // Cập nhật Learning
  const learning = data.progress.learning;
  document.querySelector('#progressLearning .progress').style.width = `${learning.percentage}%`;
  document.querySelector('#progressLearning .progress-value').innerText = learning.value;

  // Cập nhật Exchange
  const exchange = data.progress.exchange;
  document.querySelector('#progressExchange .progress').style.width = `${exchange.percentage}%`;
  document.querySelector('#progressExchange .progress-value').innerText = exchange.value;

  // Cập nhật Other
  const other = data.progress.other;
  document.querySelector('#progressOther .progress').style.width = `${other.percentage}%`;
  document.querySelector('#progressOther .progress-value').innerText = other.value;
}
  
function updateStudentTable(data) {
  const tableBody = document.getElementById('studentTableBody');

  // Xóa hết các hàng cũ (nếu có)
  tableBody.innerHTML = '';

  // Thêm dữ liệu sinh viên vào bảng
  data.forEach(student => {
    const row = document.createElement('tr'); // Tạo một hàng mới

    // Tạo các cột dữ liệu cho từng sinh viên
    row.innerHTML = `
      <td>${student.id}</td>
      <td>${student.name}</td>
      <td>${student.email}</td>
      <td>${student.meetings}</td>
      <td>${student.balance}</td>
    `;

    // Thêm hàng vào bảng
    tableBody.appendChild(row);
  });
}
  
function updateMentorTable(data) {
  const tableBody = document.getElementById('mentorTableBody');

  // Xóa hết các hàng cũ (nếu có)
  tableBody.innerHTML = '';

  // Thêm dữ liệu mentor vào bảng
  data.forEach(mentor => {
    const row = document.createElement('tr'); // Tạo một hàng mới

    // Tạo các cột dữ liệu cho từng mentor
    row.innerHTML = `
      <td>${mentor.id}</td>
      <td>${mentor.name}</td>
      <td>${mentor.major}</td>
      <td>${mentor.studentQuantity}</td>
      <td>${mentor.rating}</td>
    `;

    // Thêm hàng vào bảng
    tableBody.appendChild(row);
  });
}
  
  

  


// Gọi hàm để cập nhật dữ liệu
updateStatCard(data);
updateMeetingStats(data);
updateStudentTable(studentsData);
updateStats(statsData);
updateMentorTable(mentorsData);