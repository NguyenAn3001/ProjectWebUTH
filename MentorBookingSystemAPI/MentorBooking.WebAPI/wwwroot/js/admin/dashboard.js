function toggleNotifications() {
  const panel = document.getElementById('notificationsPanel');
  if (panel.style.display === 'block') {
      panel.style.display = 'none';
  } else {
      panel.style.display = 'block';
  }
} 


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

updateStudentTable(studentsData);

updateMentorTable(mentorsData);