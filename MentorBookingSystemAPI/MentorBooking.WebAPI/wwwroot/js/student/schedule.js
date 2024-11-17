let currentDate = new Date();
let bookings = new Map();

// Replace with your actual token for authorization
const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI4YWI5MzE5Mi0xMzg1LTQyMmEtMWNmNi0wOGRkMDcyNGRkYjEiLCJ1bmlxdWVfbmFtZSI6InRkaGlsMTkiLCJqdGkiOiI1ZTQ1ZTY5MS0xYzc4LTRmMTUtOGIyMi1mZmVjYzkyMGJkYjEiLCJyb2xlIjoiU3R1ZGVudCIsIm5iZiI6MTczMTg2MTYzNSwiZXhwIjoxNzMxODcyNDM1LCJpYXQiOjE3MzE4NjE2MzUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNDciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTQ3In0.oEiarW416Un2298dtAxCNrpYqdnoOqM7XbJ8PiZXvvg';

function initializeBookings() {
    const savedBookings = localStorage.getItem('bookings');
    if (savedBookings) {
        const bookingsObj = JSON.parse(savedBookings);
        bookings = new Map(Object.entries(bookingsObj));
        console.log('Loaded bookings:', bookings);
    }

    const savedDate = localStorage.getItem('currentDate');
    if (savedDate) {
        currentDate = new Date(savedDate);
        console.log('Loaded currentDate:', currentDate);
    }
}

function saveBookings() {
    const bookingsObj = Object.fromEntries(bookings);
    localStorage.setItem('bookings', JSON.stringify(bookingsObj));
    localStorage.setItem('currentDate', currentDate.toISOString());
    console.log('Saved bookings:', bookingsObj);
    console.log('Saved currentDate:', currentDate);
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
    console.log('Week updated to:', formatDate(monday), 'to', formatDate(friday));
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

        const timeCell = document.createElement('td');
        timeCell.textContent = timeSlot;
        timeCell.className = 'time-slot';
        row.appendChild(timeCell);

        for (let i = 0; i < 5; i++) {
            const cell = document.createElement('td');
            const monday = getMonday(currentDate);
            const currentDayDate = new Date(monday);
            currentDayDate.setDate(monday.getDate() + i);

            const dateStr = currentDayDate.toISOString().split('T')[0];
            const bookingKey = `${dateStr}-${timeSlot}`;

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
    console.log('Generated time slots');
}

function previousWeek() {
    currentDate.setDate(currentDate.getDate() - 7);
    updateWeekDisplay();
    generateTimeSlots();
    console.log('Navigated to previous week:', currentDate);
}

function nextWeek() {
    currentDate.setDate(currentDate.getDate() + 7);
    updateWeekDisplay();
    generateTimeSlots();
    console.log('Navigated to next week:', currentDate);
}

function showCancelModal(timeSlot, dayIndex, bookingKey) {
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    const monday = getMonday(currentDate);
    const bookingDate = new Date(monday);
    bookingDate.setDate(monday.getDate() + dayIndex);

    const modalText = `Are you sure you want to cancel this booking?<br><br>
        <strong>Date:</strong> ${formatDate(bookingDate)}<br>
        <strong>Day:</strong> ${days[dayIndex]}<br>
        <strong>Time:</strong> ${timeSlot}<br>`;

    document.getElementById('cancelModalText').innerHTML = modalText;
    document.getElementById('cancelModal').style.display = 'flex';

    window.currentCancellation = bookingKey;
    console.log('Displaying cancel modal for booking:', bookingKey);
}

function hideModal() {
    document.getElementById('cancelModal').style.display = 'none';
    console.log('Cancel modal hidden');
}

function confirmCancel() {
    if (window.currentCancellation) {
        bookings.delete(window.currentCancellation);
        saveBookings();
        hideModal();
        generateTimeSlots();
        console.log('Booking cancelled:', window.currentCancellation);
    }
}

function goBack() {
    window.history.back();
    console.log('Navigating back');
}

function fetchStudentSchedules() {
    const url = 'http://localhost:5076/api/ScheduleViews/student-schedules';
    const headers = {
        'Accept': '*/*',
        'Authorization': `Bearer ${token}`
    };

    fetch(url, { method: 'GET', headers: headers })
        .then(response => response.json())
        .then(data => {
            console.log('Fetched schedules:', data);
            if (data.length > 0) {
                // Populate the bookings map or schedule display with fetched data
                data.forEach(schedule => {
                    const bookingKey = `${schedule.date}-${schedule.time}`;
                    bookings.set(bookingKey, schedule);
                });
                saveBookings();
                generateTimeSlots(); // Re-render the schedule with updated bookings
            }
        })
        .catch(error => {
            console.error('Error fetching schedules:', error);
        });
}

document.addEventListener('DOMContentLoaded', function () {
    initializeBookings();
    updateWeekDisplay();
    generateTimeSlots();
    fetchStudentSchedules();
});
