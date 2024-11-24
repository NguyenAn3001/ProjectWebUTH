// Lấy nút quay lại
const backButton = document.querySelector('.back-button');

backButton.addEventListener('click', () => {
  window.history.back();
});
const scheduleContainer = document.getElementById("schedule-container");

async function fetchStudentSchedules() {
    try {
        const token = localStorage.getItem('accessToken'); // Get the token from localStorage
        const response = await fetch('http://localhost:5076/api/ScheduleViews/student-schedules', {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${token}`,
            },
        });

        const data = await response.json();

        // Log the full response for debugging
        console.log("API Response: ", data);

        if (data && data.length > 0) {
            scheduleContainer.innerHTML = '<p>Schedules fetched successfully!</p>';
            displaySchedules(data);
        } else {
            scheduleContainer.innerHTML = '<p>No scheduled sessions found.</p>';
        }
    } catch (error) {
        console.error('Error fetching schedules:', error);
        scheduleContainer.innerHTML = '<p>There was an error loading your schedules. Please try again later.</p>';
    }
}

// Function to display the schedules on the page
function displaySchedules(schedules) {
    scheduleContainer.innerHTML = ''; // Clear any existing content

    schedules.forEach(schedule => {
        const scheduleItem = document.createElement('div');
        scheduleItem.classList.add('schedule-item');
        
        const scheduleDate = new Date(schedule.data.date);
        const formattedDate = scheduleDate.toLocaleDateString(); // Format the date

        scheduleItem.innerHTML = `
            <p><strong>Date:</strong> ${formattedDate}</p>
            <p><strong>Start Time:</strong> ${schedule.data.startTime}</p>
            <p><strong>End Time:</strong> ${schedule.data.endTime}</p>
            <p><strong>Session ID:</strong> ${schedule.data.sessionId}</p>
            <p><strong>Status:</strong> <span data-status="${schedule.data.isActive}">${schedule.data.isActive ? 'Active' : 'Inactive'}</span></p>
        `;
        
        scheduleContainer.appendChild(scheduleItem);
    });
}

// Call the function to fetch and display the student's schedules
fetchStudentSchedules();
