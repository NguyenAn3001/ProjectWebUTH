function register(event) {
    event.preventDefault();

    const userName = document.getElementById("username").value.trim();
    const email = document.getElementById("logemailSignup").value.trim();
    const password = document.getElementById("logpassSignup").value;
    const confirmPassword = document.getElementById("confirmPass").value;
    const roleName = document.getElementById("roleName").value;

    if (!userName || !email || !password || !confirmPassword || !roleName) {
        alert("Please fill in all required fields.");
        return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        alert("Please enter a valid email address.");
        return;
    }

    if (password.length < 8) {
        alert("Password must be at least 8 characters long.");
        return;
    }

    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return;
    }

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
        
        if (!response.ok) {
            throw new Error(data.message || "Registration failed");
        }
        
        return data;
    })
    .then(data => {
        if (data.status === "Success") {
            Swal.fire({
                icon: 'success',
                title: 'Hoàn tất!',
                text: 'Dữ liệu của bạn đã được lưu thành công.',
            });
            window.location.href = "../../views/auth/login.html";
        } else {
            throw new Error(data.message || "Registration failed");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        
        if (error.message) {
            alert(error.message);
        } else {
            alert("An error occurred during registration. Please try again.");
        }
    })
    .finally(() => {
        submitButton.disabled = false;
    });
}

