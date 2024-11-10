let currentDate = new Date();
let bookings = new Map();

// Khởi tạo từ localStorage
function initializeBookings() {
    const savedBookings = localStorage.getItem('bookings');
    if (savedBookings) {
        const bookingsObj = JSON.parse(savedBookings);
        bookings = new Map(Object.entries(bookingsObj));
    }
    
    const savedDate = localStorage.getItem('currentDate');
    if (savedDate) {
        currentDate = new Date(savedDate);
    }
}

function saveBookings() {
    const bookingsObj = Object.fromEntries(bookings);
    localStorage.setItem('bookings', JSON.stringify(bookingsObj));
    localStorage.setItem('currentDate', currentDate.toISOString());
}

function getMonday(date) {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1);
    return new Date(date.setDate(diff));
}

function formatDate(date) {
    return date.toLocaleDateString('en-US', { 
        month: 'short', 
        day: 'numeric',
        year: 'numeric'
    });
}

function updateWeekDisplay() {
    const monday = getMonday(currentDate);
    const friday = new Date(monday);
    friday.setDate(monday.getDate() + 4);
    
    document.getElementById('currentWeek').textContent = 
        `${formatDate(monday)} - ${formatDate(friday)}`;
    
    saveBookings();
}

function generateTimeSlots() {
    const timeSlots = [
        "08:00", "09:00", "10:00", "11:00", "12:00",
        "13:00", "14:00", "15:00", "16:00", "17:00",
        "18:00", "19:00", "20:00"
    ];
    
    const tbody = document.getElementById('scheduleBody');
    tbody.innerHTML = '';
    
    timeSlots.forEach(timeSlot => {
        const row = document.createElement('tr');
        
        const timeCell = document.createElement('td');
        timeCell.textContent = timeSlot;
        timeCell.className = 'time-slot';
        row.appendChild(timeCell);
        
        for (let i = 0; i < 5; i++) {
            const cell = document.createElement('td');
            const bookingKey = `${timeSlot}-${i}`;
            
            if (bookings.has(bookingKey)) {
                cell.className = 'schedule-cell booked-slot fade-in';
                cell.innerHTML = `
                    <div>Booked</div>
                    <button class="cancel-button" onclick="showCancelModal('${timeSlot}', ${i})">
                        <i class="fas fa-times"></i> Cancel
                    </button>
                `;
            } else {
                cell.className = 'schedule-cell empty-slot';
                cell.innerHTML = `<div>--</div>`;
            }
            row.appendChild(cell);
        }
        
        tbody.appendChild(row);
    });
}

function previousWeek() {
    currentDate.setDate(currentDate.getDate() - 7);
    updateWeekDisplay();
    generateTimeSlots();
}

function nextWeek() {
    currentDate.setDate(currentDate.getDate() + 7);
    updateWeekDisplay();
    generateTimeSlots();
}

function showCancelModal(time, dayIndex) {
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    const bookingKey = `${time}-${dayIndex}`;
    
    const modalText = `
        Are you sure you want to cancel this booking?<br><br>
        <strong>Day:</strong> ${days[dayIndex]}<br>
        <strong>Time:</strong> ${time}<br>
    `;
    
    document.getElementById('cancelModalText').innerHTML = modalText;
    document.getElementById('cancelModal').style.display = 'flex';
    
    window.currentCancellation = bookingKey;
}

function hideModal() {
    document.getElementById('cancelModal').style.display = 'none';
}

function confirmCancel() {
    if (window.currentCancellation) {
        bookings.delete(window.currentCancellation);
        saveBookings();
        hideModal();
        generateTimeSlots();
    }
}

document.addEventListener('DOMContentLoaded', function() {
    initializeBookings();
    updateWeekDisplay();
    generateTimeSlots();
    
    // Thêm một số booking mẫu nếu chưa có booking nào
    if (bookings.size === 0) {
        bookings.set("09:00-0", { time: "09:00", day: "Monday" });
        bookings.set("14:00-2", { time: "14:00", day: "Wednesday" });
        saveBookings();
        generateTimeSlots();
    }
});