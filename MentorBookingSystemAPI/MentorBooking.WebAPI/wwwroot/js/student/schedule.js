let currentDate = new Date();
let bookings = new Map();

// Initialize from localStorage
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
        "08:00 - 09:30",
        "09:45 - 11:15",
        "11:30 - 13:00",
        "13:15 - 14:45",
        "15:00 - 16:30",
        "16:45 - 18:15"
    ];
    
    const tbody = document.getElementById('scheduleBody');
    tbody.innerHTML = '';
    
    timeSlots.forEach(timeSlot => {
        const row = document.createElement('tr');
        
        // Time slot column
        const timeCell = document.createElement('td');
        timeCell.textContent = timeSlot;
        timeCell.className = 'time-slot';
        row.appendChild(timeCell);
        
        // Create cells for each day
        for (let i = 0; i < 5; i++) {
            const cell = document.createElement('td');
            const monday = getMonday(currentDate);
            const currentDayDate = new Date(monday);
            currentDayDate.setDate(monday.getDate() + i);
            
            // Create unique key for this time slot
            const dateStr = currentDayDate.toISOString().split('T')[0];
            const bookingKey = `${dateStr}-${timeSlot}`;
            
            // Check if this slot is booked
            if (bookings.has(bookingKey)) {
                cell.className = 'schedule-cell booked-slot';
                cell.innerHTML = `<div>Booked</div>`;
                cell.onclick = () => showCancelModal(timeSlot, i, bookingKey);
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

function showCancelModal(timeSlot, dayIndex, bookingKey) {
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    const monday = getMonday(currentDate);
    const bookingDate = new Date(monday);
    bookingDate.setDate(monday.getDate() + dayIndex);
    
    const modalText = `
        Are you sure you want to cancel this booking?<br><br>
        <strong>Date:</strong> ${formatDate(bookingDate)}<br>
        <strong>Day:</strong> ${days[dayIndex]}<br>
        <strong>Time:</strong> ${timeSlot}<br>
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

function goBack() {
    window.history.back();
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function() {
    initializeBookings();
    updateWeekDisplay();
    generateTimeSlots();
});