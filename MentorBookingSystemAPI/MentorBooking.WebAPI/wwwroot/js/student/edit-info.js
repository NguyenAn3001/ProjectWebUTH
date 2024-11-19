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

    // Simple phone number validation (adjust based on your expected format)
    const phoneRegex = /^[+]?[0-9]{10,15}$/;
    if (!phoneRegex.test(studentData.phone)) {
        alert("Invalid phone number format. Please enter a valid phone number.");
        return;
    }

    fetch(`http://localhost:5076/api/info/student?studentId=${userId}`, {
        method: "POST",  // Reverted back to POST
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${accessToken}`
        },
        body: JSON.stringify(studentData)
    })
    .then(response => response.json())
    .then(data => {
        if (data && !data.errors) {  // Adjust condition based on how the API returns success
            localStorage.setItem("firstName", studentData.firstName);
            localStorage.setItem("lastName", studentData.lastName);
            alert("Cập nhật thông tin thành công!");
            window.location.href = "personal-info.html";
        } else {
            if (data.errors && data.errors.length > 0) {
                // If the API returns specific error messages
                alert(`Error: ${data.errors.join(", ")}`);
            } else {
                alert("Failed to update student information.");
            }
        }
    })
    .catch(error => {
        console.error("Error:", error);
        alert("An error occurred while updating student information.");
    });
});
