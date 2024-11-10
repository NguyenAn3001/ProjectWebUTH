// Dữ liệu mẫu cho mentors
        const mentors = [
            {
                id: 1,
                name: "Nguyễn Văn An",
                title: "Senior Software Engineer tại Google",
                avatar: "/api/placeholder/100/100",
                skills: ["JavaScript", "React", "Node.js"],
                experience: "5+ năm",
                rating: 4.9,
                reviews: 128,
                sessions: 256
            },
            {
                id: 2,
                name: "Trần Thị Bình",
                title: "Tech Lead tại Microsoft",
                avatar: "/api/placeholder/100/100",
                skills: ["Python", "Machine Learning", "AWS"],
                experience: "8+ năm",
                rating: 4.8,
                reviews: 95,
                sessions: 180
            },
            {
                id: 3,
                name: "Lê Hoàng Cường",
                title: "Full Stack Developer tại Facebook",
                avatar: "/api/placeholder/100/100",
                skills: ["TypeScript", "Angular", "MongoDB"],
                experience: "3-5 năm",
                rating: 4.7,
                reviews: 75,
                sessions: 150
            },
            {
                id: 4,
                name: "Phạm Minh Dương",
                title: "DevOps Engineer tại Amazon",
                avatar: "/api/placeholder/100/100",
                skills: ["Docker", "Kubernetes", "AWS"],
                experience: "5+ năm",
                rating: 4.9,
                reviews: 112,
                sessions: 220
            },
            {
                id: 5,
                name: "Hoàng Thị Em",
                title: "Mobile Developer tại Grab",
                avatar: "/api/placeholder/100/100",
                skills: ["React Native", "iOS", "Android"],
                experience: "3-5 năm",
                rating: 4.6,
                reviews: 68,
                sessions: 134
            },
            {
                id: 6,
                name: "Vũ Thanh Giang",
                title: "Data Scientist tại VNG",
                avatar: "/api/placeholder/100/100",
                skills: ["Python", "TensorFlow", "Data Analysis"],
                experience: "5+ năm",
                rating: 4.8,
                reviews: 89,
                sessions: 176
            },
            {
                id: 7,
                name: "Đặng Hải Hà",
                title: "Backend Developer tại Tiki",
                avatar: "/api/placeholder/100/100",
                skills: ["Java", "Spring Boot", "PostgreSQL"],
                experience: "3-5 năm",
                rating: 4.7,
                reviews: 82,
                sessions: 164
            },
            {
                id: 8,
                name: "Ngô Bảo Long",
                title: "Frontend Developer tại Shopee",
                avatar: "/api/placeholder/100/100",
                skills: ["Vue.js", "Nuxt.js", "CSS"],
                experience: "1-3 năm",
                rating: 4.5,
                reviews: 45,
                sessions: 98
            },
            {
                id: 9,
                name: "Trịnh Mai Phương",
                title: "UI/UX Designer tại Momo",
                avatar: "/api/placeholder/100/100",
                skills: ["Figma", "Adobe XD", "UI Design"],
                experience: "5+ năm",
                rating: 4.9,
                reviews: 156,
                sessions: 312
            },
            {
                id: 10,
                name: "Lý Quốc Trung",
                title: "Security Engineer tại FPT",
                avatar: "/api/placeholder/100/100",
                skills: ["Cybersecurity", "Ethical Hacking", "Network Security"],
                experience: "8+ năm",
                rating: 4.8,
                reviews: 134,
                sessions: 268
            },
            {
                id: 11,
                name: "Mai Thu Thảo",
                title: "Product Manager tại Lazada",
                avatar: "/api/placeholder/100/100",
                skills: ["Product Management", "Agile", "Scrum"],
                experience: "5+ năm",
                rating: 4.7,
                reviews: 92,
                sessions: 184
            },
            {
                id: 12,
                name: "Đỗ Văn Khoa",
                title: "Cloud Architect tại AWS",
                avatar: "/api/placeholder/100/100",
                skills: ["AWS", "Cloud Architecture", "DevOps"],
                experience: "8+ năm",
                rating: 4.9,
                reviews: 178,
                sessions: 356
            },
            {
                id: 13,
                name: "Lâm Yến Nhi",
                title: "Game Developer tại VNG",
                avatar: "/api/placeholder/100/100",
                skills: ["Unity", "C#", "Game Design"],
                experience: "3-5 năm",
                rating: 4.6,
                reviews: 64,
                sessions: 128
            },
            {
                id: 14,
                name: "Bùi Quang Minh",
                title: "Blockchain Developer tại Binance",
                avatar: "/api/placeholder/100/100",
                skills: ["Solidity", "Web3.js", "Smart Contracts"],
                experience: "3-5 năm",
                rating: 4.8,
                reviews: 86,
                sessions: 172
            },
            {
                id: 15,
                name: "Nguyễn Thanh Tùng",
                title: "AI Engineer tại VinAI",
                avatar: "/api/placeholder/100/100",
                skills: ["Deep Learning", "Computer Vision", "PyTorch"],
                experience: "5+ năm",
                rating: 4.7,
                reviews: 98,
                sessions: 196
            }
        ];

        const itemsPerPage = 10;
        let currentPage = 1;
        // Dữ liệu cho bộ lọc
        const filters = {
            skills: ["JavaScript", "Python", "Java", "React", "Node.js", "AWS", "Machine Learning"],
            experience: ["1-3 năm", "3-5 năm", "5+ năm", "8+ năm"],
            availability: ["Sáng", "Chiều", "Tối", "Cuối tuần"]
        };

        const activeFilters = new Set();

        // Render mentors grid
        function renderMentors(mentorsList = mentors) {
            const startIndex = (currentPage - 1) * itemsPerPage;
            const endIndex = startIndex + itemsPerPage;
            const paginatedMentors = mentorsList.slice(startIndex, endIndex);
            const grid = document.getElementById('mentorsGrid');
            grid.innerHTML = paginatedMentors.map(mentor => `
                <div class="mentor-card">
                    <div class="mentor-header"></div>
                    <div class="mentor-info">
                        <img src="${mentor.avatar}" alt="${mentor.name}" class="mentor-avatar">
                        <div class="mentor-content">
                            <h3 class="mentor-name">${mentor.name}</h3>
                            <p class="mentor-title">${mentor.title}</p>
                            <div class="mentor-skills">
                                ${mentor.skills.map(skill => `
                                    <span class="skill-tag">${skill}</span>
                                `).join('')}
                            </div>
                            <div class="mentor-stats">
                                <div class="stat">
                                    <div class="stat-value">${mentor.rating}</div>
                                    <div class="stat-label">Đánh giá</div>
                                </div>
                                <div class="stat">
                                    <div class="stat-value">${mentor.reviews}</div>
                                    <div class="stat-label">Nhận xét</div>
                                </div>
                                <div class="stat">
                                    <div class="stat-value">${mentor.sessions}</div>
                                    <div class="stat-label">Buổi học</div>
                                </div>
                            </div>
                            <button class="book-button" onclick="openBookingModal(${mentor.id})">
                                Đặt lịch học
                            </button>
                        </div>
                    </div>
                </div>
            `).join('');
            renderPagination(mentorsList.length);
        }

        function renderPagination(totalItems) {
            const totalPages = Math.ceil(totalItems / itemsPerPage);
            const pagination= document.getElementById('pagination');

            let paginationHTML = '';
            if(currentPage > 1) {
                paginationHTML += `
                    <button class="page-button" onclick="changePage(${currentPage - 1})">
                        <i class="fas fa-chevron-left"></i>
                    </button>
                `;
            }
            for (let i =1; totalPages >= i; i++) {
                    paginationHTML += `
                        < class="page-button ${i === currentPage ? 'active' : ''}"
                            onclick="changePage(${i})">
                            ${i}
                        </button>
                    `;
                }
                if(currentPage < totalPages) {
                    paginationHTML += `
                        <button class="page-button" onclick="changePage(${currentPage + 1})">
                            <i class="fas fa-chevron-right"></i>
                        </button>
                    `;
                }
            }

        function renderFilterOptions() {
            const skillFilters = document.getElementById('skillFilters');
            const experienceFilters = document.getElementById('experienceFilters');
            const availabilityFilters = document.getElementById('availabilityFilters');

            skillFilters.innerHTML = filters.skills.map(skill => `
                <div class="filter-option" onclick="toggleFilter('skill', '${skill}')">
                    ${skill}
                </div>
            `).join('');

            experienceFilters.innerHTML = filters.experience.map(exp => `
                <div class="filter-option" onclick="toggleFilter('experience', '${exp}')">
                    ${exp}
                </div>
            `).join('');

            availabilityFilters.innerHTML = filters.availability.map(time => `
                <div class="filter-option" onclick="toggleFilter('availability', '${time}')">
                    ${time}
                </div>
            `).join('');
        }
        function changePage(page) {
            currentPage = page;
            renderMentors();
        }
        // Toggle filter selection
        function toggleFilter(type, value) {
            const filterKey = `${type}:${value}`;
            const element = event.target;
            
            if (activeFilters.has(filterKey)) {
                activeFilters.delete(filterKey);
                element.classList.remove('active');
            } else {
                activeFilters.add(filterKey);
                element.classList.add('active');
            }
        }

        // Apply filters
        function applyFilters() {
            renderActiveFilters();
            // Implement filter logic here
            closeFilterModal();
        }

        // Render active filters
        function renderActiveFilters() {
            const container = document.getElementById('activeFilters');
            container.innerHTML = Array.from(activeFilters).map(filter => {
                const [type, value] = filter.split(':');
                return `
                    <div class="filter-tag">
                        ${value}
                        <button class="remove-filter" onclick="removeFilter('${filter}')">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                `;
            }).join('');
        }

        // Remove filter
        function removeFilter(filter) {
            activeFilters.delete(filter);
            renderActiveFilters();
            // Re-apply filters
        }

        // Modal controls
        function openFilterModal() {
            document.getElementById('filterModal').classList.add('active');
        }

        function closeFilterModal() {
            document.getElementById('filterModal').classList.remove('active');
        }

        function openBookingModal(mentorId) {
            const modal = document.getElementById('bookingModal');
            const slotsContainer = document.getElementById('bookingSlots');
            
            const currentDate = new Date();
            const currentWeek = getWeekDates(currentDate);

            slotsContainer.innerHTML = `
                <div class="calendar-section">
                    <div class="week-picker">
                        <button class="week-nav" onclick="changeWeek(-1)">
                            <i class="fas fa-chevron-left"></i>
                        </button>
                        <span class="week-display" id="weekDisplay"></span>
                        <button class="week-nav" onclick="changeWeek(1)">
                            <i class="fas fa-chevron-right"></i>
                        </button>
                    </div>
                    <div class="weekday-slots">
                        ${['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'].map(day => `
                            <div class="weekday">${day}</div>
                        `).join('')}
                    </div>
                    <div class="time-slots-container">
                        ${generateWeekTimeSlots()}
                    </div>
                </div>
            `;
            
            updateWeekDisplay(currentDate);
            modal.classList.add('active');
        }

        
        let currentWeekStart = new Date();

        function changeWeek(offset) {
            currentWeekStart.setDate(currentWeekStart.getDate() + (offset * 7));
            updateWeekDisplay(currentWeekStart);
            document.querySelector('.time-slots-container').innerHTML = generateWeekTimeSlots();
        }

        function updateWeekDisplay(date) {
            const weekDates = getWeekDates(date);
            const startDate = weekDates[0].toLocaleDateString('vi-VN');
            const endDate = weekDates[6].toLocaleDateString('vi-VN');
            document.getElementById('weekDisplay').textContent = `${startDate} - ${endDate}`;
        }

        function getWeekDates(date) {
            const sunday = new Date(date);
            sunday.setDate(date.getDate() - date.getDay());
            
            return Array.from({ length: 7 }, (_, i) => {
                const day = new Date(sunday);
                day.setDate(sunday.getDate() + i);
                return day;
            });
        }

        function generateWeekTimeSlots() {
            const timeSlots = ['9:00', '10:00', '11:00', '14:00', '15:00', '16:00', '17:00'];
            const weekDays = getWeekDates(currentWeekStart);
            
            return weekDays.map(date => `
                <div class="day-slots">
                    ${timeSlots.map(time => `
                        <div class="time-slot" onclick="toggleTimeSlot(this)" 
                             data-date="${date.toLocaleDateString('vi-VN')}"
                             data-time="${time}">
                            ${time}
                        </div>
                    `).join('')}
                </div>
            `).join('');
        }

        function toggleTimeSlot(element) {
            const prevSelected = document.querySelector('.time-slot.active');
            if (prevSelected) {
                prevSelected.classList.remove('active');
            }
            element.classList.add('active');
        }

        function confirmBooking() {
            const selectedSlot = document.querySelector('.time-slot.active');
            if (selectedSlot) {
                const date = selectedSlot.dataset.date;
                const time = selectedSlot.dataset.time;
                alert(`Đặt lịch thành công cho ngày ${date} lúc ${time}`);
                closeBookingModal();
            } else {
                alert('Vui lòng chọn thời gian học');
            }
        }

        // Initialize
        document.addEventListener('DOMContentLoaded', function() {
            renderMentors();
            renderFilterOptions();
        });

        function closeBookingModal() {
            document.getElementById('bookingModal').classList.remove('active');
        }

        function toggleTimeSlot(element) {
            document.querySelectorAll('.time-slot').forEach(slot => {
                slot.classList.remove('active');
            });
            element.classList.add('active');
        }

        function confirmBooking() {
            const selectedSlot = document.querySelector('.time-slot.active');
            if (selectedSlot) {
                alert('Đặt lịch thành công cho ' + selectedSlot.textContent);
                closeBookingModal();
            } else {
                alert('Vui lòng chọn thời gian học');
            }
        }

        function generateTimeSlots() {
            const slots = [];
            for (let hour = 9; hour <= 20; hour++) {
                slots.push(`${hour}:00 - ${hour + 1}:00`);
            }
            return slots;
        }

        // Search functionality
        document.getElementById('searchInput').addEventListener('input', function(e) {
            const searchTerm = e.target.value.toLowerCase();
            const filteredMentors = mentors.filter(mentor => {
                return mentor.name.toLowerCase().includes(searchTerm) ||
                       mentor.skills.some(skill => skill.toLowerCase().includes(searchTerm));
            });
            renderMentors(filteredMentors);
        });
