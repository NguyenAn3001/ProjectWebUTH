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
        
        if (data.status === "Success") {  // Kiểm tra status
            alert("Login successful!");

            // Kiểm tra xem dữ liệu có tồn tại accessToken, refreshToken, userId hay không
            if (data.accessToken && data.refreshToken && data.userId) {
                // Lưu accessToken, refreshToken và userId vào localStorage
                localStorage.setItem("accessToken", data.accessToken);
                localStorage.setItem("refreshToken", data.refreshToken);
                localStorage.setItem("userId", data.userId);
                localStorage.setItem("role", data.role);  // Lưu vai trò của người dùng (Student)

                // Điều hướng dựa trên role
                if (data.role === 'Admin') {
                    window.location.href = "../../views/admin/dashboard.html";
                } else if (data.role === 'Mentor') {
                    window.location.href = "../../views/mentor/profile/personal-info.html";
                } else if (data.role === 'Student') {
                    window.location.href = "../../views/student/profile/personal-info.html";
                } else {
                    console.error("Unknown role:", data.role);
                    alert("Unknown role, cannot navigate.");
                }
            } else {
                console.error("Tokens or user ID missing:", data);
                alert("Login failed, tokens or user ID not received.");
            }
        } else {
            alert(data.message);  // Hiển thị thông báo lỗi
        }
    })
    .catch(error => {
        console.error("Error:", error);
        alert("An error occurred during login.");
    });
}

const formLogin = document.getElementById("card-front");
if (formLogin) {
    formLogin.addEventListener("submit", login);
}
