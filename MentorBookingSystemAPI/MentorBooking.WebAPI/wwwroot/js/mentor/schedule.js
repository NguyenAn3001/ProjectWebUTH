let currentDate = new Date();
let selectedTimeSlots = new Map(); // Store selected time slots with their dates

// Initialize the schedule
function initializeSchedule() {
    updateWeekDisplay();
    generateTimeSlots();
    loadSelectedTimeSlots();
}

// Cập nhật thời gian hiển thị tuần
function updateWeekDisplay() {
    const startDate = getWeekStartDate(currentDate);
    const endDate = new Date(startDate);
    endDate.setDate(endDate.getDate() + 4);
    
    const startDateStr = startDate.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    const endDateStr = endDate.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
    
    document.getElementById('currentWeek').textContent = `${startDateStr} - ${endDateStr}`;
}

// Hàm lấy ngày thứ Hai của tuần hiện tại
function getWeekStartDate(date) {
    const mondayDate = new Date(date);
    const day = mondayDate.getDay();
    const diff = mondayDate.getDate() - day + (day === 0 ? -6 : 1);
    mondayDate.setDate(diff);
    return mondayDate;
}

// Lấy các thời gian đã chọn
function getSelectedTimeSlots() {
    return Array.from(selectedTimeSlots.keys());
}

// Gửi các thời gian đã chọn tới API
function sendSelectedTimeSlots() {
    const selectedSlots = getSelectedTimeSlots();
    
    const requestBody = {
        timeSlots: selectedSlots // Đây là mảng chứa các thời gian đã chọn
    };

    fetch('http://localhost:5076/api/schedules/add', { // URL cần thay đổi theo API thực tế
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
    })
    .then(response => response.json())
    .then(data => {
        console.log('Request thành công:', data);
        // Xử lý sau khi gửi thành công, ví dụ ẩn modal hoặc thông báo thành công
        alert('Time slots saved successfully');
    })
    .catch((error) => {
        console.error('Lỗi:', error);
        alert('There was an error while saving time slots');
    });
}

// Hàm khởi tạo lịch (ví dụ với các khoảng thời gian cố định)
function generateTimeSlots() {
    const timeSlots = [
        "08:00 - 09:30", "09:45 - 11:15", "11:30 - 13:00",
        "13:15 - 14:45", "15:00 - 16:30", "16:45 - 18:15"
    ];

    const scheduleBody = document.getElementById('scheduleBody');
    scheduleBody.innerHTML = ''; // Reset lịch

    timeSlots.forEach(timeSlot => {
        const row = document.createElement('tr');
        const timeCell = document.createElement('td');
        timeCell.textContent = timeSlot;
        timeCell.className = 'time-header';
        row.appendChild(timeCell);

        // Vòng lặp để tạo các ô cho từng ngày trong tuần
        for (let i = 0; i < 5; i++) {
            const cell = document.createElement('td');
            const cellDate = new Date(getWeekStartDate(currentDate));
            cellDate.setDate(cellDate.getDate() + i);

            const dateTimeKey = `${cellDate.toISOString().split('T')[0]}-${timeSlot}`;
            cell.className = 'time-slot';
            if (selectedTimeSlots.has(dateTimeKey)) {
                cell.classList.add('selected');
            } else {
                cell.classList.add('available');
            }

            cell.dataset.datetime = dateTimeKey;
            cell.onclick = () => handleTimeSlotClick(cell, dateTimeKey);
            row.appendChild(cell);
        }

        scheduleBody.appendChild(row);
    });
}

// Xử lý khi người dùng nhấn vào thời gian
function handleTimeSlotClick(cell, dateTimeKey) {
    const isSelected = cell.classList.contains('selected');
    const [dateStr, timeSlot] = dateTimeKey.split('-');
    const date = new Date(dateStr);
    const dateDisplay = date.toLocaleDateString('en-US', { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    });

    const modalTitle = document.getElementById('modalTitle');
    const modalText = document.getElementById('modalText');
    
    if (isSelected) {
        modalTitle.textContent = 'Remove Availability';
        modalText.textContent = `Are you sure you want to remove your availability for ${timeSlot} on ${dateDisplay}?`;
    } else {
        modalTitle.textContent = 'Add Availability';
        modalText.textContent = `Would you like to mark yourself as available for ${timeSlot} on ${dateDisplay}?`;
    }

    window.currentAction = {
        cell: cell,
        dateTimeKey: dateTimeKey,
        isSelected: isSelected
    };

    showModal();
}

// Hiển thị modal
function showModal() {
    document.getElementById('actionModal').style.display = 'block';
}

// Ẩn modal
function hideModal() {
    document.getElementById('actionModal').style.display = 'none';
}

// Cập nhật lịch đã chọn vào LocalStorage
function saveSelectedTimeSlots() {
    localStorage.setItem('mentorSelectedTimeSlots', JSON.stringify(Array.from(selectedTimeSlots)));
}

// Load các thời gian đã chọn từ LocalStorage
function loadSelectedTimeSlots() {
    const saved = localStorage.getItem('mentorSelectedTimeSlots');
    if (saved) {
        selectedTimeSlots = new Map(JSON.parse(saved));
        generateTimeSlots(); 
    }
}

// Gọi hàm khởi tạo khi trang được tải
document.addEventListener('DOMContentLoaded', initializeSchedule);
// Xử lý khi người dùng nhấn vào nút xác nhận trong modal
function confirmAction() {
    const { cell, dateTimeKey, isSelected } = window.currentAction;

    if (isSelected) {
        // Nếu thời gian đã được chọn, bỏ chọn nó (remove)
        selectedTimeSlots.delete(dateTimeKey);
        cell.classList.remove('selected');
        cell.classList.add('available');
    } else {
        // Nếu thời gian chưa được chọn, thêm chọn nó (add)
        selectedTimeSlots.set(dateTimeKey, true);
        cell.classList.add('selected');
        cell.classList.remove('available');
    }

    // Cập nhật lại LocalStorage sau khi thay đổi
    saveSelectedTimeSlots();

    // Đóng modal sau khi thực hiện hành động
    hideModal();

    // Gửi các thời gian đã chọn đến server
    sendSelectedTimeSlots();
}
