// Constants
const API_BASE_URL = 'http://localhost:5076/api';
const ITEMS_PER_PAGE = 10;

// State management 
let currentPage = 1;
let currentSearchText = '';
let currentSkillsFilter = '';

// Back button function
function goBack() {
    window.history.back();
}

// Main search function 
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

// Show mentor details
async function showMentorDetails(mentorId) {
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
                            <button onclick="bookMentor('${mentor.mentorId}')">Book Mentor</button>
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

async function bookMentor(mentorId) {
    const accessToken = localStorage.getItem('accessToken');
    const userId = localStorage.getItem('userId');  // Assuming the user ID is stored in localStorage
    const groupId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";  // Example Group ID, adjust as needed
    
    const bookingData = {
        mentorId: mentorId,
        sessionCount: 0,
        pointPerSession: 0,
        dateBookings: [mentorId], // Assuming the booking is tied to the mentor ID
        groupId: groupId
    };

    try {
        const response = await fetch(`${API_BASE_URL}/Bookings`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${accessToken}`
            },
            body: JSON.stringify(bookingData)
        });

        if (!response.ok) {
            throw new Error(`Booking failed. Status: ${response.status}`);
        }

        const result = await response.json();
        if (result.status === 'Success') {
            alert('Booking successful!');
        } else {
            showError('Failed to book mentor. Please try again.');
        }
    } catch (error) {
        console.error('Error booking mentor:', error);
        showError('Failed to book mentor. Please try again.');
    }
}


// Modal handling
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

// Pagination
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
        // Redirect to login page
        window.location.href = '/login';
        throw error;
    }
}
