document.addEventListener("DOMContentLoaded", function () {
    // Gắn sự kiện submit cho form
    const form = document.querySelector(".info-form");
    if (form) {
      form.addEventListener("submit", saveUserInfo);
    } else {
      console.error("Form element not found! Make sure your HTML structure is correct.");
    }
  });
  
document.getElementById('updateStudentForm').addEventListener('submit', function(e) {
    e.preventDefault();
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const studentData = {
        firstName: document.getElementById('studentFirstName').value,
        lastName: document.getElementById('studentLastName').value,
        phone: document.getElementById('phone').value,
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
           
        } else {
            alert("Failed to update student information.");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        alert("An error occurred while updating student information.");
    });
});