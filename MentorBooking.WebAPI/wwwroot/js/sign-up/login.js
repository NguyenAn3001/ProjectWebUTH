function showUpdateModal(role) {
    const hasCompletedInitialSetup = localStorage.getItem(`hasCompletedInitialSetup_${localStorage.getItem('userId')}`);

    if (hasCompletedInitialSetup !== "true") {
        if (role === 'Student') {
            document.getElementById('studentUpdateModal').style.display = 'block';
        } else if (role === 'Mentor') {
            document.getElementById('mentorUpdateModal').style.display = 'block';
        }
    } else {
        redirectToProfile(role);
    }
}


function redirectToProfile(role) {
    if (role === 'Student') {
        window.location.href = "../../views/student/profile/personal-info.html";
    } else if (role === 'Mentor') {
        window.location.href = "../../views/mentor/profile/personal-info.html";
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
            if (data.accessToken && data.refreshToken && data.userId && data.role) {
                // Lưu thông tin đăng nhập vào localStorage
                localStorage.setItem("accessToken", data.accessToken);
                localStorage.setItem("refreshToken", data.refreshToken);
                localStorage.setItem("userId", data.userId);
                localStorage.setItem("role", data.role);

                // Kiểm tra xem người dùng đã hoàn thành cập nhật thông tin chưa
                const hasCompletedInitialSetup = localStorage.getItem(`hasCompletedInitialSetup_${data.userId}`);
                
                if (!hasCompletedInitialSetup) {
                    // Nếu là lần đăng nhập đầu tiên, hiển thị form cập nhật thông tin
                    showUpdateModal(data.role);
                } else {
                    // Nếu đã hoàn thành, chuyển hướng tới trang profile
                    redirectToProfile(data.role);
                }
            } else {
                console.error("Thiếu token hoặc user ID:", data);
                alert("Đăng nhập thất bại, thiếu token hoặc user ID.");
            }
        } else {
            alert(data.message);
        }
    })
    .catch(error => {
        console.error("Lỗi:", error);
        alert("Đã xảy ra lỗi trong quá trình đăng nhập.");
    });
}



// Xử lý form cập nhật thông tin sinh viên
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
            // Cập nhật giá trị trong localStorage và đánh dấu đã hoàn thành
            localStorage.setItem(`hasCompletedInitialSetup_${userId}`, "true");
            document.getElementById('studentUpdateModal').style.display = 'none'; // Ẩn modal
            redirectToProfile('Student'); // Chuyển hướng đến trang profile của sinh viên
        } else {
            alert("Cập nhật thông tin sinh viên thất bại.");
        }
    })
    .catch(error => {
        console.error("Lỗi:", error);
        alert("Đã xảy ra lỗi khi cập nhật thông tin sinh viên.");
    });
});

// Xử lý form cập nhật thông tin mentor
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
            // Đánh dấu đã hoàn thành cập nhật với userId cụ thể
            localStorage.setItem(`hasCompletedInitialSetup_${userId}`, "true");
            
            document.getElementById('mentorUpdateModal').style.display = 'none';
            redirectToProfile('Mentor');
        }}
    )
    .catch(error => {
        console.error("Lỗi:", error);
        alert("Đã xảy ra lỗi khi cập nhật thông tin mentor.");
    });
});

window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.style.display = 'none'; // Đóng modal khi nhấp ra ngoài
    }
};
