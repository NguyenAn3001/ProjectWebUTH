function goBack() {
    window.history.back();
}

let currentPage = 1; 
const pageSize = 10; 

        // Function to search mentors with flexible parameters (page, pageSize, searchText, sortBy)
        async function searchMentors({ page = 1, pageSize = 10, searchText = '', sortBy = '', skillsFilter = '' } = {}) {
            const apiUrl = `http://localhost:5076/api/SearchAndSort/Search?page=${page}&pageSize=${pageSize}&searchText=${encodeURIComponent(searchText)}&sortBy=${encodeURIComponent(sortBy)}`;
            const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzNjU2N2JmYy03ZmU2LTQ0NGMtNDY1Ny0wOGRkMDRlNDNlYTEiLCJ1bmlxdWVfbmFtZSI6InRkaGlsMTkiLCJqdGkiOiI1NDNjMDY5Ni1jZWJjLTQwMmYtODI0YS05MGQ4OTFkMzljZGEiLCJyb2xlIjoiU3R1ZGVudCIsIm5iZiI6MTczMTc3OTkxMywiZXhwIjoxNzMxNzkwNzEzLCJpYXQiOjE3MzE3Nzk5MTMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNDciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTQ3In0.Z_4xj_aXqx2oxeFAk5l7aLknyv24ELy9N6enQ7KkDus'; // Replace with a valid token

            try {
                const response = await fetch(apiUrl, {
                    method: 'GET',
                    headers: {
                        'accept': '*/*',
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }

                const data = await response.json();

                // Log the entire response to console for debugging
                console.log(data);

                // Filter mentors based on the search text, skills, and name
                const filteredMentors = data.articles.filter(mentor => 
                    (mentor.firstName.toLowerCase().includes(searchText.toLowerCase()) || // Match first name
                    mentor.lastName.toLowerCase().includes(searchText.toLowerCase()) ||  // Match last name
                    mentor.skillName.some(skill => skill.toLowerCase().includes(searchText.toLowerCase()))) && // Match skill or name
                    (skillsFilter === '' || mentor.skillName.some(skill => skill.toLowerCase() === skillsFilter.toLowerCase())) // Match selected skill filter
                );

                // Return the filtered list of mentors for UI rendering
                return {
                    ...data,
                    articles: filteredMentors
                };

            } catch (error) {
                console.error('Error searching mentors:', error);
            }
        }

        // Handle search when button is clicked
        async function handleSearch() {
            const searchText = document.getElementById('searchTextInput').value;
            const skillsFilter = document.getElementById('skillsFilter').value;
            
            // Call the searchMentors function with search text and selected skill filter
            const data = await searchMentors({
                page: currentPage,          // Current page
                pageSize,                   // Page size (10 mentors per page)
                searchText,                 // Get search text from input
                sortBy: 'firstName',        // Default sorting by first name
                skillsFilter               // Get selected skill from dropdown
            });

            // Clear previous results
            const mentorList = document.getElementById('mentorList');
            mentorList.innerHTML = data.articles.map(mentor => `
                <div>
                    <h3>${mentor.firstName} ${mentor.lastName}</h3>
                    <p>Skills: ${mentor.skillName.join(', ')}</p>
                </div>
            `).join('');

            // Update pagination controls
            updatePagination(data);
        }
        // Update pagination controls based on total pages
        function updatePagination(data) {
            const pagination = document.getElementById('pagination');
            const totalPages = data.totalPages;

            let paginationHTML = '';

            // Previous button
            paginationHTML += `<button onclick="changePage('prev')" ${currentPage === 1 ? 'disabled' : ''}>Previous</button>`;

            // Page buttons
            for (let i = 1; i <= totalPages; i++) {
                paginationHTML += `<button onclick="changePage(${i})" ${i === currentPage ? 'disabled' : ''}>${i}</button>`;
            }

            // Next button
            paginationHTML += `<button onclick="changePage('next')" ${currentPage === totalPages ? 'disabled' : ''}>Next</button>`;

            pagination.innerHTML = paginationHTML;
        }

        // Change page based on user action
        function changePage(page) {
            if (page === 'prev') {
                if (currentPage > 1) currentPage--;
            } else if (page === 'next') {
                currentPage++;
            } else {
                currentPage = page;
            }

            handleSearch();
        }

        // Call the initial search when the page loads
        handleSearch();

        function displayMentors(mentorList) {
            const mentorListContainer = document.getElementById('mentorList');
            mentorListContainer.innerHTML = ''; // Clear the list before adding new items
        
            mentorList.forEach(mentor => {
                const mentorCard = document.createElement('div');
                mentorCard.classList.add('mentor-card');
        
                mentorCard.innerHTML = `
                    <div class="mentor-header">
                        <div class="mentor-info">
                            <h4>${mentor.name}</h4>
                        </div>
                    </div>
                    <div class="mentor-skills">
                        ${mentor.skills.map(skill => `<span class="skill-tag">${skill}</span>`).join('')}
                    </div>
                `;
        
                mentorListContainer.appendChild(mentorCard); // Add the mentor card to the container
            });
        }
        
        // Call the function to display the mentors when the page loads
        window.onload = () => {
            displayMentors(mentors);
        };