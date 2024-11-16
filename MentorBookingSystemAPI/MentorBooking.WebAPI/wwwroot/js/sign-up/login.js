function showUpdateModal(role) {
    if (role === 'Student') {
        document.getElementById('studentUpdateModal').style.display = 'block';
    } else if (role === 'Mentor') {
        document.getElementById('mentorUpdateModal').style.display = 'block';
    }
}

function login(event) {
    event.preventDefault();
    const userName = document.getElementById("logusernameLogin").value;
    const password = document.getElementById("logpassLogin").value;

    const data = {
        userName: userName,
        password: password
    };

    fetch("http://localhost:5076/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            console.log("API response:", data);

            if (data.status === "Success") {
                if (data.accessToken && data.refreshToken && data.userId) {
                    localStorage.setItem("accessToken", data.accessToken);
                    localStorage.setItem("refreshToken", data.refreshToken);
                    localStorage.setItem("userId", data.userId);
                    localStorage.setItem("role", data.role);

                    showUpdateModal(data.role);
                } else {
                    console.error("Tokens or user ID missing:", data);
                    alert("Login failed, tokens or user ID not received.");
                }
            } else {
                alert(data.message);
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred during login.");
        });
}

// Handle student form submission
document.getElementById('updateStudentForm').addEventListener('submit', function(e) {
    e.preventDefault();
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const studentData = {
        firstName: document.getElementById('studentFirstName').value,
        lastName: document.getElementById('studentLastName').value,
        phone: document.getElementById('studentPhone').value,
        createdAt: new Date().toISOString()
    };

    fetch(`http://localhost:5076/api/info/student?studentId=${userId}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${accessToken}`
        },
        body: JSON.stringify(studentData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.status === "Success") {
                localStorage.setItem("firstName", studentData.firstName);
                localStorage.setItem("lastName", studentData.lastName);

                document.getElementById('studentUpdateModal').style.display = 'none';
                window.location.href = "../../views/student/profile/personal-info.html";
            } else {
                alert("Failed to update student information.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred while updating student information.");
        });
});
document.getElementById('updateMentorForm').addEventListener('submit', function(e) {
    e.preventDefault();
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const mentorData = {
        firstName: document.getElementById('mentorFirstName').value,
        lastName: document.getElementById('mentorLastName').value,
        phone: document.getElementById('mentorPhone').value,
        experienceYears: parseInt(document.getElementById('experienceYears').value, 10),
        mentorDescription: document.getElementById('mentorDescription').value,
        skills: document.getElementById('skills').value.split(',').map(skill => skill.trim()),
        createdAt: new Date().toISOString()
    };

    fetch(`http://localhost:5076/api/info/mentor?mentorId=${userId}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${accessToken}`
        },
        body: JSON.stringify(mentorData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.status === "Success") {
                localStorage.setItem("firstName", mentorData.firstName);
                localStorage.setItem("lastName", mentorData.lastName);

                document.getElementById('mentorUpdateModal').style.display = 'none';
                window.location.href = "../../views/mentor/profile/personal-info.html";
            } else {
                alert("Failed to update mentor information.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred while updating mentor information.");
        });
});
window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.style.display = 'none';
    }
}