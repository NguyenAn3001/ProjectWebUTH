function showError(message) {
    alert(message);
}

// Hàm kiểm tra định dạng email
function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Hàm kiểm tra độ dài và độ phức tạp của mật khẩu
function validatePassword(password) {
    if (password.length < 8 || password.length > 16) {
        showError('Mật khẩu phải có độ dài từ 8 đến 16 ký tự');
        return false;
    }
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).+$/;
    if (!passwordRegex.test(password)) {
        showError('Mật khẩu phải có ít nhất một chữ hoa, một chữ thường và một ký tự đặc biệt');
        return false;
    }
    return true;
}

// Hàm đăng nhập
async function handleLogin(username, password) {
    try {
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            const errorData = await response.json();
            showError(errorData.message || "Login failed. Please try again.");
            return;
        }

        const data = await response.json();

        if (data.token) {
            localStorage.setItem('token', data.token);
            alert("Login successful!");
            window.location.href = "../page/home/home.html";
        } else {
            showError("Login failed. Please check your email and password.");
        }
    } catch (error) {
        console.error('Login failed:', error);
    }
}

// Hàm xử lý sự kiện đăng nhập
document.querySelector('.form-group').addEventListener('submit', async (e) => {
    e.preventDefault();
    const username = document.getElementById('logemailLogin').value;
    const password = document.getElementById('logpassLogin').value;

    await handleLogin(email, password);
});

// Hàm đăng ký
function validateSignup() {
    const username = document.getElementById("username").value;
    const email = document.getElementById("logemailSignup").value;
    const password = document.getElementById("logpassSignup").value;
    const confirmPassword = document.getElementById("confirmPass").value;

    if (!username || !email || !password || !confirmPassword) {
        showError("Please fill in all fields.");
        return false;
    }

    if (!validateEmail(email)) {
        showError("Invalid email format.");
        return false;
    }

    if (!validatePassword(password)) {
        return false;
    }

    if (password !== confirmPassword) {
        showError("Passwords do not match.");
        return false;
    }

    alert("Sign up successful!");
    window.location.href = "../../page/home/home.html";
}

// Modal handling
function showModal() {
    modal.style.display = "block";
}

function closeModal() {
    modal.style.display = "none";
}

function showMentorForm() {
    modal.style.display = "none";
    mentorForm.style.display = "block";
}

function backToMain() {
    mentorForm.style.display = "none";
    mainForms.style.display = "block";
}

function submitMentorForm() {
    const specialty = document.getElementById("specialty").value;
    const experience = document.getElementById("experience").value;
    const introduction = document.getElementById("introduction").value;
    const cvLink = document.getElementById("cvLink").value;

    if (specialty && experience && introduction && cvLink) {
        alert("Đăng ký mentor thành công!");
        showMentorForm();
    } else {
        showError("Vui lòng điền đầy đủ thông tin!");
    }
}

// Modal close on outside click
window.onclick = function(event) {
    if (event.target == modal) {
        closeModal();
    }
}
