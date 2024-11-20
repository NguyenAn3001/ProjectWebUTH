function handleLogout() {
    const token = localStorage.getItem("accessToken");
    fetch("http://localhost:5076/api/auth/logout", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.status === "Success") {
            alert("Logged out successfully.");
            window.location.href = "/MentorBookingSystemAPI/MentorBooking.WebAPI/wwwroot/index.html";
        } else {
            alert("Logout failed: " + data.message);
        }
    })
    .catch(error => {
        console.error("Error logging out:", error);
        alert("An error occurred while logging out.");
    });
}