function goBack() {
    window.history.back();  // Quay lại trang trước
}

async function searchMentors({ page = 1, pageSize = 10, searchText = '', sortBy = '', skillsFilter = '' } = {}) {
    let accessToken = localStorage.getItem('accessToken'); // Lấy accessToken từ localStorage

    try {
        const response = await fetch(`http://localhost:5076/api/SearchAndSort/Search?page=${page}&pageSize=${pageSize}&searchText=${encodeURIComponent(searchText)}&sortBy=${encodeURIComponent(sortBy)}`, {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${accessToken}`
            }
        });

        if (response.status === 401) {
            accessToken = await refreshAccessToken();
            return searchMentors({ page, pageSize, searchText, sortBy, skillsFilter }); // Gửi lại yêu cầu với token mới
        }

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        console.log('Response Body:', data);

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
    }
}

async function refreshAccessToken() {
    const refreshToken = localStorage.getItem('refreshToken'); 

    if (!refreshToken) {
        throw new Error('Refresh token not found!');
    }

    const response = await fetch('http://localhost:5076/api/Authentication/refresh-token', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ refreshToken })
    });

    if (!response.ok) {
        throw new Error('Failed to refresh access token');
    }

    const data = await response.json();
    const newAccessToken = data.accessToken;

    localStorage.setItem('accessToken', newAccessToken);

    return newAccessToken;
}


async function handleSearch() {
    const searchText = document.getElementById('searchTextInput').value; 
    const skillsFilter = document.getElementById('skillsFilter').value; 

    const data = await searchMentors({
        page: 1,              
        pageSize: 10,         
        searchText,          
        sortBy: 'firstName', 
        skillsFilter         
    });

    const mentorList = document.getElementById('mentorList');
    mentorList.innerHTML = data.articles.map(mentor => `
        <div class="mentor-card" onclick="showMentorDetails(${mentor.id})" style="cursor: pointer; border: 1px solid #ddd; padding: 10px; margin-bottom: 10px;">
            <h3>${mentor.firstName} ${mentor.lastName}</h3>
            <p>Skills: ${mentor.skillName.join(', ')}</p>
        </div>
    `).join('');

    updatePagination(data);
}

function updatePagination(data) {
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = `
        <button onclick="goToPage(${data.currentPage - 1})" ${data.currentPage === 1 ? 'disabled' : ''}>Previous</button>
        <span>Page ${data.currentPage} of ${data.totalPages}</span>
        <button onclick="goToPage(${data.currentPage + 1})" ${data.currentPage === data.totalPages ? 'disabled' : ''}>Next</button>
    `;
}

function goToPage(pageNumber) {
    handleSearch(pageNumber);
}

// document.getElementById('searchButton').addEventListener('click', handleSearch);
document.addEventListener('DOMContentLoaded', async () => {
    // Lấy từ khóa tìm kiếm từ URL hoặc sessionStorage
    const urlParams = new URLSearchParams(window.location.search);
    let searchText = urlParams.get('searchText') || sessionStorage.getItem('lastSearchText') || '';

    // Hiển thị từ khóa trong ô input (nếu có ô input trên trang)
    const searchInput = document.getElementById('searchTextInput');
    if (searchInput) {
        searchInput.value = searchText;
    }

    // Gọi hàm tìm kiếm nếu có từ khóa
    if (searchText) {
        await handleSearch(); // Gọi hàm xử lý tìm kiếm đã viết trước đó
    }
});


async function showMentorDetails(mentorId) {
    try {
        const accessToken = localStorage.getItem('accessToken');
        const response = await fetch(`http://localhost:5076/api/Mentors/${mentorId}`, {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${accessToken}`
            }
        });

        if (!response.ok) {
            throw new Error(`Failed to fetch mentor details. Status: ${response.status}`);
        }

        const mentor = await response.json();

        // Hiển thị thông tin mentor
        const mentorDetailsContainer = document.getElementById('mentorDetails');
        mentorDetailsContainer.innerHTML = `
            <h2>${mentor.firstName} ${mentor.lastName}</h2>
            <p>Email: ${mentor.email}</p>
            <p>Skills: ${mentor.skills.join(', ')}</p>
            <h3>Schedule:</h3>
            <ul>
                ${mentor.schedule.map(slot => `<li>${slot.date}: ${slot.time}</li>`).join('')}
            </ul>
            <button onclick="closeMentorDetails()">Close</button>
        `;

        mentorDetailsContainer.style.display = 'block'; // Hiển thị container chi tiết

    } catch (error) {
        console.error('Error fetching mentor details:', error);
    }
}
