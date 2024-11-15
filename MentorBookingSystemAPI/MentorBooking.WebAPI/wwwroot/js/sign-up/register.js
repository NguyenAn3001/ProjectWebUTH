function register(event) {
    event.preventDefault();

    // Lấy dữ liệu và trim() để loại bỏ khoảng trắng thừa
    const userName = document.getElementById("username").value.trim();
    const email = document.getElementById("logemailSignup").value.trim();
    const password = document.getElementById("logpassSignup").value;
    const confirmPassword = document.getElementById("confirmPass").value;
    const roleName = document.getElementById("roleName").value;

    // Validate dữ liệu trước khi gửi API
    if (!userName || !email || !password || !confirmPassword || !roleName) {
        alert("Please fill in all required fields.");
        return;
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        alert("Please enter a valid email address.");
        return;
    }

    // Validate password length
    if (password.length < 8) {
        alert("Password must be at least 8 characters long.");
        return;
    }

    // Validate password match
    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return;
    }

    // Disable submit button while processing
    const submitButton = event.target;
    submitButton.disabled = true;

    const data = {
        userName,
        email,
        password,
        confirmPassword,
        roleName
    };

    fetch("http://localhost:5076/api/auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
    .then(async response => {
        // Parse JSON response
        const data = await response.json();
        
        // Check if response is ok (status in 200-299 range)
        if (!response.ok) {
            throw new Error(data.message || "Registration failed");
        }
        
        return data;
    })
    .then(data => {
        if (data.status === "Success") {
            alert("Registration successful! Please log in.");
            window.location.href = "../../views/auth/login.html";
        } else {
            // Handle other success scenarios if needed
            throw new Error(data.message || "Registration failed");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        
        // Show specific error message if available
        if (error.message) {
            alert(error.message);
        } else {
            alert("An error occurred during registration. Please try again.");
        }
    })
    .finally(() => {
        // Re-enable submit button
        submitButton.disabled = false;
    });
}

// Xóa hàm validateSignup vì không cần thiết