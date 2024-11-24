// Constants
const API_BASE_URL = 'http://localhost:5076/api';
const ITEMS_PER_PAGE = 10;

// State management 
let currentPage = 1;
let currentSearchText = '';
let currentSkillsFilter = '';
window.onload = function() {
    const urlParams = new URLSearchParams(window.location.search);
    const searchText = urlParams.get('searchText');

    if (searchText) {
        // Điền text vào ô input (nếu có)
        document.getElementById('searchTextInput').value = searchText;
        
        // Gọi hàm tìm kiếm 
        handleSearch();
    }
};

async function handleSearch() {
    const searchText = document.getElementById('searchTextInput').value;
    const skillsFilter = document.getElementById('skillsFilter').value;

    currentSearchText = searchText;
    currentSkillsFilter = skillsFilter;
    currentPage = 1;

    await fetchAndDisplayMentors();
}
// Back button function
function goBack() {
    window.history.back();
}


async function handleSearch() {
    const searchText = document.getElementById('searchTextInput').value;
    const skillsFilter = document.getElementById('skillsFilter').value;

    currentSearchText = searchText;
    currentSkillsFilter = skillsFilter;
    currentPage = 1;

    await fetchAndDisplayMentors();
}

// Fetch and display mentors
async function fetchAndDisplayMentors() {
    try {
        showLoading();
        
        const data = await searchMentors({
            page: currentPage,
            pageSize: ITEMS_PER_PAGE,
            searchText: currentSearchText,
            sortBy: 'firstName',
            skillsFilter: currentSkillsFilter
        });

        displayMentors(data);
        updatePagination(data);
        
    } catch (error) {
        console.error('Error fetching mentors:', error);
        showError('Failed to load mentors. Please try again.');
    } finally {
        hideLoading();
    }
}

// Search mentors function
async function searchMentors({ page = 1, pageSize = ITEMS_PER_PAGE, searchText = '', sortBy = '', skillsFilter = '' } = {}) {
    let accessToken = localStorage.getItem('accessToken');
    
    try {
        const response = await fetch(`${API_BASE_URL}/SearchAndSort/Search?page=${page}&pageSize=${pageSize}&searchText=${encodeURIComponent(searchText)}&sortBy=${encodeURIComponent(sortBy)}`, {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${accessToken}`
            }
        });

        if (response.status === 401) {
            accessToken = await refreshAccessToken();
            return searchMentors({ page, pageSize, searchText, sortBy, skillsFilter });
        }

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        
        const filteredMentors = data.articles.filter(mentor => 
            (mentor.firstName.toLowerCase().includes(searchText.toLowerCase()) || 
             mentor.lastName.toLowerCase().includes(searchText.toLowerCase()) || 
             mentor.skillName.some(skill => skill.toLowerCase().includes(searchText.toLowerCase()))) && 
            (skillsFilter === '' || mentor.skillName.some(skill => skill.toLowerCase() === skillsFilter.toLowerCase()))
        );

        return {
            ...data,
            articles: filteredMentors
        };

    } catch (error) {
        console.error('Error searching mentors:', error);
        throw error;
    }
}

// Display mentors in the list
function displayMentors(data) {
    const mentorList = document.getElementById('mentorList');
    
    if (!data.articles || data.articles.length === 0) {
        mentorList.innerHTML = `
            <div class="no-results">
                <p>No mentors found.</p>
            </div>
        `;
        return;
    }

    mentorList.innerHTML = data.articles.map(mentor => `
        <div class="mentor-card" onclick="showMentorDetails('${mentor.mentorId}')">
            <div class="mentor-card-content">
                <h3>${mentor.firstName} ${mentor.lastName}</h3>
                <div class="skills-container">
                    ${mentor.skillName.map(skill => `
                        <span class="skill-tag">${skill}</span>
                    `).join('')}
                </div>
            </div>
        </div>
    `).join('');
}

// Function to show mentor details
async function showMentorDetails(mentorId) {
    localStorage.setItem('currentMentorId', mentorId);
    try {
        showLoading();
        const accessToken = localStorage.getItem('accessToken');
        const response = await fetch(`${API_BASE_URL}/UserProfiles/mentor-profiles?mentorId=${mentorId}`, {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${accessToken}`
            }
        });

        if (!response.ok) {
            throw new Error(`Failed to fetch mentor details. Status: ${response.status}`);
        }

        const result = await response.json();

        if (result.status === "Success" && result.data) {
            const mentor = result.data;
            const mentorDetails = document.getElementById('mentorDetails');
            const createdDate = new Date(mentor.createdAt).toLocaleDateString();

            // Hiển thị chi tiết mentor
            mentorDetails.innerHTML = `
                <div class="mentor-profile">
                    <div class="mentor-profile-header">
                        <h2>${mentor.name}</h2>
                        <button onclick="closeMentorDetails()" class="close-button">×</button>
                    </div>

                    <div class="mentor-profile-content">
                        <div class="profile-section">
                            <h3>Contact Information</h3>
                            <p><i class="fas fa-envelope"></i> ${mentor.email}</p>
                            <p><i class="fas fa-phone"></i> ${mentor.phone}</p>
                        </div>

                        <div class="profile-section">
                            <h3>Professional Information</h3>
                            <p><strong>Experience:</strong> ${mentor.experienceYears} years</p>
                            <p><strong>Member since:</strong> ${createdDate}</p>
                        </div>

                        <div class="profile-section">
                            <h3>Skills</h3>
                            <div class="skills-container">
                                ${mentor.skills.map(skill => `
                                    <span class="skill-tag">${skill}</span>
                                `).join('')}
                            </div>
                        </div>

                        <div class="profile-section">
                            <h3>About</h3>
                            <p>${mentor.mentorDescription || 'No description provided.'}</p>
                        </div>

                        <div class="profile-section">
                            <button onclick="hideMentorDetailsAndShowBookingForm('${mentor.mentorId}')">Book Mentor</button>
                        </div>
                    </div>
                </div>
            `;

            showModal(mentorDetails);
        } else {
            throw new Error('Failed to get mentor data');
        }
    } catch (error) {
        console.error('Error showing mentor details:', error);
        showError('Failed to load mentor details. Please try again.');
    } finally {
        hideLoading();
    }
}

function getMentorIdFromStorage() {
    const mentorId = localStorage.getItem('currentMentorId');
    console.log(mentorId);  // Truy cập mentorId từ localStorage
}

// Function to show the booking form inside a modal
function showBookingForm(mentorId) {
    // Create modal container only when the button is clicked
    const modalContainer = document.createElement('div');
    modalContainer.id = 'bookingModalContainer';
    modalContainer.className = 'modal-container';
    modalContainer.innerHTML = `
        <div class="modal booking-modal">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>Book Mentor</h3>
                    <span class="close-button" onclick="closeBookingModal()">×</span>
                </div>
                <div class="modal-body">
                    <form id="bookingForm">
                        <div class="form-group">
                            <label for="groupSelect">Select Group:</label>
                            <select id="groupSelect" name="groupSelect" required>
                                <option value="">Select a group...</option>
                            </select>
                        </div>

                        <div class="form-group">
                            <label for="scheduleSelect">Available Schedule:</label>
                            <select id="scheduleSelect" name="scheduleSelect" required>
                                <option value="">Select schedule...</option>
                            </select>
                        </div>

                        <div class="form-group">
                            <label for="sessionCount">Number of Sessions:</label>
                            <input type="number" id="sessionCount" name="sessionCount" min="1" required>
                        </div>

                        <div class="form-group">
                            <label for="pointPerSession">Points per Session:</label>
                            <input type="number" id="pointPerSession" name="pointPerSession" min="1" required>
                        </div>

                        <button type="submit" class="submit-btn">Book Now</button>
                    </form>
                </div>
            </div>
        </div>
    `;

    // Add modal to body only when function is called
    document.body.appendChild(modalContainer);

    // Scroll to modal
    modalContainer.scrollIntoView({ behavior: 'smooth' });

    // Form submit event listener
    const bookingFormElement = document.getElementById('bookingForm');
    bookingFormElement.onsubmit = async (event) => {
        event.preventDefault();
        await bookMentor(mentorId);
        closeBookingModal();
    };

    // Fetch dropdown data
    fetchStudentGroups();
    fetchAvailableSchedules();
}

function hideMentorDetailsAndShowBookingForm(mentorId) {
    const mentorDetails = document.getElementById('mentorDetails');
    mentorDetails.style.display = 'none'; // Hide mentor details
    showBookingForm(mentorId);
}
function closeBookingModal() {
    const modalContainer = document.getElementById('bookingModalContainer');
    if (modalContainer) {
        modalContainer.remove();
    }
}


// Close the modal
function closeModal() {
    const modalContainer = document.getElementById('modalContainer');
    modalContainer.style.display = 'none';
    modalContainer.innerHTML = ''; // Clear the content
}


function showModal(container) {
    const overlay = document.createElement('div');
    overlay.id = 'mentor-overlay';
    document.body.appendChild(overlay);
    
    container.style.display = 'block';
    document.body.style.overflow = 'hidden';
    
    overlay.addEventListener('click', closeMentorDetails);
}

function closeMentorDetails() {
    const mentorDetails = document.getElementById('mentorDetails');
    const overlay = document.getElementById('mentor-overlay');
    
    if (mentorDetails) {
        mentorDetails.style.display = 'none';
    }
    
    if (overlay) {
        overlay.remove();
    }
    
    document.body.style.overflow = 'auto';
}

function updatePagination(data) {
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = `
        <div class="pagination-controls">
            <button onclick="goToPage(${currentPage - 1})" 
                    ${currentPage === 1 ? 'disabled' : ''}>
                Previous
            </button>
            <span>Page ${currentPage} of ${data.totalPages}</span>
            <button onclick="goToPage(${currentPage + 1})" 
                    ${currentPage === data.totalPages ? 'disabled' : ''}>
                Next
            </button>
        </div>
    `;
}

async function goToPage(page) {
    currentPage = page;
    await fetchAndDisplayMentors();
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Loading and error handling
function showLoading() {
    const loader = document.getElementById('loader') || createLoader();
    loader.style.display = 'flex';
}

function hideLoading() {
    const loader = document.getElementById('loader');
    if (loader) {
        loader.style.display = 'none';
    }
}

function createLoader() {
    const loader = document.createElement('div');
    loader.id = 'loader';
    loader.innerHTML = `
        <div class="loader-spinner"></div>
    `;
    document.body.appendChild(loader);
    return loader;
}

function showError(message) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message';
    errorDiv.textContent = message;
    
    document.body.appendChild(errorDiv);
    
    setTimeout(() => {
        errorDiv.remove();
    }, 3000);
}

// Event listeners
document.addEventListener('DOMContentLoaded', () => {
    // Initial load
    fetchAndDisplayMentors();

    // Search input with debounce
    const searchInput = document.getElementById('searchTextInput');
    if (searchInput) {
        let debounceTimer;
        searchInput.addEventListener('input', () => {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(handleSearch, 300);
        });
    }

    // Skills filter
    const skillsFilter = document.getElementById('skillsFilter');
    if (skillsFilter) {
        skillsFilter.addEventListener('change', handleSearch);
    }

    // Handle escape key for modal
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            closeMentorDetails();
        }
    });
});

// Token refresh function
async function refreshAccessToken() {
    try {
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await fetch(`${API_BASE_URL}/auth/refresh-token`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ refreshToken })
        });

        if (!response.ok) {
            throw new Error('Failed to refresh token');
        }

        const data = await response.json();
        localStorage.setItem('accessToken', data.accessToken);
        return data.accessToken;
    } catch (error) {
        console.error('Error refreshing token:', error);
        window.location.href = '/login';
        throw error;
    }
}


//BOOKING MENTOR
async function fetchStudentGroups() {
    const token = localStorage.getItem('accessToken'); 
    const response = await fetch('http://localhost:5076/api/group', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'accept': '*/*',
        },
    });

    if (response.ok) {
        const data = await response.json();
        const groups = data.data;
        const groupSelect = document.getElementById('groupSelect'); 

        groups.forEach(group => {
            const option = document.createElement('option');
            option.value = group.groupId;
            option.textContent = group.groupName;
            groupSelect.appendChild(option);
        });
    } else {
        console.error('Failed to fetch groups');
    }
}
fetchStudentGroups();

const currentMentorId = localStorage.getItem('currentMentorId'); 
//available schedule
async function fetchAvailableSchedules() {
    const token = localStorage.getItem('accessToken');
    const mentorId = localStorage.getItem('currentMentorId');
    const url = `http://localhost:5076/api/ScheduleViews/available-schedules?MentorId=${mentorId}`;

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'accept': '*/*',
            },
        });

        if (response.ok) {
            const responseData = await response.json();
            const schedules = responseData.map(item => item.data);

            const scheduleSelect = document.getElementById('scheduleSelect');
            scheduleSelect.innerHTML = ''; // Clear previous options

            if (schedules.length === 0) {
                const option = document.createElement('option');
                option.textContent = 'No available schedules for this mentor.';
                scheduleSelect.appendChild(option);
            } else {
                schedules.forEach(schedule => {
                    // Use unAvailableScheduleId instead of scheduleId
                    const option = document.createElement('option');
                    option.value = schedule.unAvailableScheduleId;
                    option.textContent = `${schedule.date} - ${schedule.startTime} to ${schedule.endTime}`;
                    scheduleSelect.appendChild(option);
                });
            }
        } else {
            console.error('Failed to fetch available schedules:', response.status, response.statusText);
            Swal.fire({
                icon: 'error',
                title: 'Fetch Error',
                text: 'Unable to retrieve mentor schedules.'
            });
        }
    } catch (error) {
        console.error('Error fetching available schedules:', error);
        Swal.fire({
            icon: 'error',
            title: 'Network Error',
            text: 'An error occurred while fetching schedules.'
        });
    }
}





// document.addEventListener("DOMContentLoaded", function () {
//     const accessToken = localStorage.getItem("accessToken");
//     const userId = localStorage.getItem("userId");
//     const loadingElement = document.getElementById('loading');
//     const scheduleSelect = document.getElementById('scheduleSelect');
//     const apiHeaders = {
//         'Authorization': `Bearer ${accessToken}`,
//         'Content-Type': 'application/json',
//         'Accept': '*/*'
//     };

//     if (!accessToken) {
//         Swal.fire({
//             icon: 'error',
//             title: 'Authentication Error',
//             text: 'Access token not found. Please log in again.'
//         }).then(() => {
//             window.location.href = "../../login.html";
//         });
//         return;
//     }
// });


function bookMentor() {
    const accessToken = localStorage.getItem("accessToken");
    const loadingElement = document.getElementById('loading');
    const groupSelect = document.getElementById('groupSelect');
    const sessionCount = document.getElementById('sessionCount');
    const pointPerSession = document.getElementById('pointPerSession');
    const scheduleSelect = document.getElementById('scheduleSelect');

    // Lấy currentMentorId từ localStorage hoặc từ nơi lưu trữ khác
    const currentMentorId = localStorage.getItem("currentMentorId");

    const apiHeaders = {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
        'Accept': '*/*'
    };

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

    // Kiểm tra nếu các trường chưa được chọn
    if (!groupSelect.value || !sessionCount.value || !pointPerSession.value || !scheduleSelect.value) {
        Swal.fire({
            icon: 'error',
            title: 'Validation Error',
            text: 'Please fill in all fields.'
        });
        return;
    }

    // Lấy scheduleAvailableId từ giá trị được chọn trong scheduleSelect
    const selectedScheduleId = scheduleSelect.value;
    console.log("Selected Schedule ID:", selectedScheduleId);
    if (!selectedScheduleId) {
        Swal.fire({
            icon: 'error',
            title: 'Validation Error',
            text: 'Please select a valid schedule.'
        });
        return;
    }

    // Tạo dữ liệu đặt lịch
    const bookingData = {
        mentorId: currentMentorId,
        sessionCount: parseInt(sessionCount.value, 10),
        pointPerSession: parseInt(pointPerSession.value, 10),
        dateBookings: [selectedScheduleId],  // Now using unAvailableScheduleId
        groupId: groupSelect.value
    };

    console.log("Booking Data:", bookingData);  // Kiểm tra dữ liệu

    // Hiển thị trạng thái loading
    loadingElement.style.display = 'block';

    // Gửi yêu cầu đặt lịch
    fetch("http://localhost:5076/api/BookingMentor/booking-mentor", {
        method: 'POST',
        headers: apiHeaders,
        body: JSON.stringify(bookingData)
    })
    .then(response => response.json())
    .then(data => {
        if (data.status === "Success") {
            console.log("Session ID:", data.data.sessionId); 
            localStorage.setItem("sessionId", data.data.sessionId);
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Mentor booked successfully!'
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to book the mentor.'
            });
        }
    })
    .catch(error => {
        console.error("Network Error:", error);
        Swal.fire({
            icon: 'error',
            title: 'Network Error',
            text: 'An error occurred while booking the mentor.'
        });
    })
    .finally(() => {
        loadingElement.style.display = 'none';
    });
}

// Render scheduleSelect khi tải trang
function renderScheduleOptions() {
    const scheduleSelect = document.getElementById('scheduleSelect');
    const schedules = JSON.parse(localStorage.getItem("schedules")) || [];

    // Gắn dữ liệu vào dropdown
    schedules.forEach(schedule => {
        const option = document.createElement('option');
        option.value = schedule.id; // Lưu `scheduleAvailableId` trong value
        option.textContent = schedule.date; // Hiển thị ngày
        scheduleSelect.appendChild(option);
    });
}

// Gọi hàm renderScheduleOptions() khi trang được tải
document.addEventListener('DOMContentLoaded', renderScheduleOptions);

document.addEventListener('DOMContentLoaded', () => {
    fetchStudentGroups();
    // fetchMentorSchedules(); // Pass the selected mentorId
});
