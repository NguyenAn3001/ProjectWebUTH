document.addEventListener("DOMContentLoaded", function () {
    const accessToken = localStorage.getItem("accessToken");
    const loadingElement = document.getElementById('loading');
    const scheduleList = document.getElementById('scheduleList');
    const freeDayInput = document.getElementById('freeDay');
    const startTimeInput = document.getElementById('startTime');
    const endTimeInput = document.getElementById('endTime');
    const addScheduleButton = document.getElementById('addSchedule');
    const viewSchedulesButton = document.getElementById('viewSchedules');
    const userId = localStorage.getItem("userId"); 

    const backButton = document.getElementById('backButton');

    document.addEventListener("DOMContentLoaded", function () {
        const backButton = document.getElementById('backButton');
    
        // Thêm sự kiện cho nút Back
        backButton.addEventListener('click', function () {
            // Quay lại trang trước đó
            window.history.back();
        });
    
        // Các phần khác của mã...
    })
    
    if (!accessToken) {
        Swal.fire({
            icon: 'error',
            title: 'Authentication Error',
            text: 'Access token not found. Please log in again.'
        }).then(() => {
            window.location.href = "../../login.html";
        });
        return;
    }

    const apiHeaders = {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
        'Accept': '*/*'
    };

    // Add a new schedule
    addScheduleButton.addEventListener('click', function (e) {
        e.preventDefault();
        const freeDay = freeDayInput.value;
        const startTime = startTimeInput.value;
        const endTime = endTimeInput.value;

        if (!freeDay || !startTime || !endTime) {
            Swal.fire({
                icon: 'error',
                title: 'Validation Error',
                text: 'Please fill in all fields.'
            });
            return;
        }

        const scheduleData = [
            {
                freeDay: freeDay,
                startTime: `${startTime}:00`,
                endTime: `${endTime}:00`
            }
        ];

        loadingElement.style.display = 'block';
        console.log("Sending schedule data:", scheduleData);
        fetch("http://localhost:5076/api/schedules/add", {
            method: 'POST',
            headers: apiHeaders,
            body: JSON.stringify(scheduleData)
        })
            .then(response => response.json())
            .then(data => {
                console.log("API Response:", data);
                if (data.status === "Success") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: data.message
                    });
                    displaySchedule(data.data);  // Hiển thị lịch mới sau khi thêm
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to add schedule.'
                    });
                }
            })
            .catch(() => {
                Swal.fire({
                    icon: 'error',
                    title: 'Network Error',
                    text: 'An error occurred while adding the schedule.'
                });
            })
            .finally(() => {
                loadingElement.style.display = 'none';
            });
    });

    function displaySchedule(schedules) {
        scheduleList.innerHTML = ''; // Clear the current list
        if (schedules.length === 0) {
            Swal.fire({
                icon: 'info',
                title: 'No Schedules Found',
                text: 'There are no schedules to display.'
            });
        } else {
            schedules.forEach(schedule => {
                const listItem = document.createElement('li');
                listItem.textContent = `Date: ${schedule.freeDay}, Time: ${schedule.startTime} - ${schedule.endTime}`;
                listItem.dataset.scheduleId = schedule.scheduleAvailableId;
                listItem.setAttribute('data-schedule-id', schedule.scheduleAvailableId); // Set data attribute for identification
                const deleteButton = document.createElement('button');
                deleteButton.textContent = 'Delete';
                deleteButton.onclick = function () {
                    deleteSchedule(schedule.scheduleAvailableId); // Call the function to delete schedule
                };
                listItem.appendChild(deleteButton);
                scheduleList.appendChild(listItem);
            });
        }
    }

    // Function to delete a schedule
    function deleteSchedule(scheduleAvailableId) {
        loadingElement.style.display = 'block';

        fetch(`http://localhost:5076/api/schedules?scheduleAvailableId=${scheduleAvailableId}`, {
            method: 'DELETE',
            headers: apiHeaders
        })
        .then(response => response.json())
        .then(data => {
            if (data.status === "Success") {
                displaySchedule(data.data);
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: data.message
                });
                // Cập nhật lại giao diện sau khi xóa
                removeScheduleFromList(scheduleAvailableId);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to delete schedule.'
                });
            }
        })
        .catch(error => {
            console.error("Network Error:", error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'An error occurred while deleting schedule.'
            });
        })
        .finally(() => {
            loadingElement.style.display = 'none';
        });
    }

    // Cập nhật lại giao diện sau khi xóa lịch
    function removeScheduleFromList(scheduleAvailableId) {
        const scheduleListItems = document.querySelectorAll('#scheduleList li');
        scheduleListItems.forEach(item => {
            if (item.dataset.scheduleId === scheduleAvailableId.toString()) {
                item.remove();  // Xóa item khỏi list
            }
        });
    }

    // View schedules button logic
    // View schedules button logic
    viewSchedulesButton.addEventListener('click', function () {
        loadingElement.style.display = 'block';
    
        fetch(`http://localhost:5076/api/schedules/schedules-mentor?mentorId=${userId}`, {
            method: 'GET',
            headers: apiHeaders
        })
        .then(response => response.json())
        .then(data => {
            console.log("Schedules retrieved:", data);
    
            if (data.status === "Success") {
                // Lưu các scheduleAvailableId vào localStorage
                const scheduleIds = data.data.map(schedule => schedule.scheduleAvailableId);
                localStorage.setItem('scheduleIds', JSON.stringify(scheduleIds));
    
                console.log("Schedule IDs saved to localStorage:", scheduleIds);
                displaySchedule(data.data); // Hiển thị lịch trên giao diện
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to retrieve schedules.'
                });
            }
        })
        .catch(error => {
            console.error("Network Error:", error);
            Swal.fire({
                icon: 'error',
                title: 'Network Error',
                text: 'An error occurred while retrieving schedules.'
            });
        })
        .finally(() => {
            loadingElement.style.display = 'none';
        });
    });
    
    // Lấy lại các scheduleAvailableId từ localStorage khi cần
    const storedScheduleIds = JSON.parse(localStorage.getItem('scheduleIds')) || [];
    console.log("Retrieved Schedule IDs from localStorage:", storedScheduleIds);
});
