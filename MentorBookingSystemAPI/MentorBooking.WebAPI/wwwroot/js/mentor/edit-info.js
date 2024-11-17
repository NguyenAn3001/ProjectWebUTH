document.getElementById('updateStudentForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const mentorData = {
        firstName: document.getElementById('mentorFirstName').value,
        lastName: document.getElementById('mentorLastName').value,
        phone: document.getElementById('mentorPhone').value,
        experienceYears: parseInt(document.getElementById('mentorExperien').value, 10), // Ensure this is correctly named and parsed
        description: document.getElementById('mentorDescription').value,
        skills: document.getElementById('mentorSkills').value.split(',').map(skill => skill.trim()),
        createdAt: new Date().toISOString()
    };

    // Simple phone number validation (adjust based on your expected format)
    const phoneRegex = /^[+]?[0-9]{10,15}$/;
    if (!phoneRegex.test(mentorData.phone)) {
        alert("Invalid phone number format. Please enter a valid phone number.");
        return;
    }

    // Check that experienceYears is valid
    if (isNaN(mentorData.experienceYears) || mentorData.experienceYears <= 0) {
        alert("Please enter a valid experience number.");
        return;
    }

    fetch(`http://localhost:5076/api/info/mentor?mentorId=${userId}`, {
        method: "POST",  // Reverted back to POST
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${accessToken}`
        },
        body: JSON.stringify(mentorData)
    })
    .then(response => response.json())
    .then(data => {
        if (data && !data.errors) {
            localStorage.setItem("firstName", mentorData.firstName);
            localStorage.setItem("lastName", mentorData.lastName);
            alert("Cập nhật thông tin thành công!");
            window.location.href = "info-user.html";
        } else {
            if (data.errors && data.errors.length > 0) {
                console.log(`Error: ${data.errors.join(", ")}`);
            } else {
                console.log("Failed to update student information.");
            }
        }
    })
    .catch(error => {
        console.error("Error:", error);
        console.log("An error occurred while updating student information.");
    });
});
