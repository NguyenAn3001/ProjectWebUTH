

let currentDate = new Date();
let selectedTimeSlots = new Map(); // Store selected time slots with their dates

// Initialize the schedule
function initializeSchedule() {
    updateWeekDisplay();
    generateTimeSlots();
    loadSelectedTimeSlots();
}

// Update the week display
function updateWeekDisplay() {
    const startDate = getWeekStartDate(currentDate);
    const endDate = new Date(startDate);
    endDate.setDate(endDate.getDate() + 4);
    
    const startDateStr = startDate.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    const endDateStr = endDate.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
    
    document.getElementById('currentWeek').textContent = `${startDateStr} - ${endDateStr}`;
}

// Get Monday of the current week
function getWeekStartDate(date) {
    const mondayDate = new Date(date);
    const day = mondayDate.getDay();
    const diff = mondayDate.getDate() - day + (day === 0 ? -6 : 1);
    mondayDate.setDate(diff);
    return mondayDate;
}

// Generate time slots
function generateTimeSlots() {
    const scheduleBody = document.getElementById('scheduleBody');
    scheduleBody.innerHTML = '';
    
    const timeSlots = [
        "08:00 - 09:30", "09:45 - 11:15", "11:30 - 13:00",
        "13:15 - 14:45", "15:00 - 16:30", "16:45 - 18:15"
    ];

    timeSlots.forEach(timeSlot => {
        const row = document.createElement('tr');
        
        // Add time column
        const timeCell = document.createElement('td');
        timeCell.textContent = timeSlot;
        timeCell.className = 'time-header';
        row.appendChild(timeCell);
        
        // Add day columns
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

// Handle time slot click
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

    // Store the current cell and dateTimeKey for use in confirmAction
    window.currentAction = {
        cell: cell,
        dateTimeKey: dateTimeKey,
        isSelected: isSelected
    };

    showModal();
}

// Confirm action (add/remove availability)
function confirmAction() {
    const { cell, dateTimeKey, isSelected } = window.currentAction;
    
    if (isSelected) {
        cell.classList.remove('selected');
        cell.classList.add('available');
        selectedTimeSlots.delete(dateTimeKey);
    } else {
        cell.classList.remove('available');
        cell.classList.add('selected');
        selectedTimeSlots.set(dateTimeKey, true);
    }
    
    saveSelectedTimeSlots();
    hideModal();
}

// Navigation functions
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

function goBack() {
    window.history.back();
}

// Modal functions
function showModal() {
    document.getElementById('actionModal').style.display = 'block';
}

function hideModal() {
    document.getElementById('actionModal').style.display = 'none';
}

// Local storage functions
function saveSelectedTimeSlots() {
    localStorage.setItem('mentorSelectedTimeSlots', JSON.stringify(Array.from(selectedTimeSlots)));
}

function loadSelectedTimeSlots() {
    const saved = localStorage.getItem('mentorSelectedTimeSlots');
    if (saved) {
        selectedTimeSlots = new Map(JSON.parse(saved));
        generateTimeSlots(); // Refresh display with loaded data
    }
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', initializeSchedule);


function goBack() {
    window.history.back(); // Quay lại trang trước
}

document.addEventListener('DOMContentLoaded', function() {
    const backButton = document.querySelector('.btn');
    if (backButton) {
        backButton.addEventListener('click', goBack);
    }
});