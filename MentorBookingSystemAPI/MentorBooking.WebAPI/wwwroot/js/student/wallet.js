function getPoints() {
    const userId = "36567bfc-7fe6-444c-4657-08dd04e43ea1";  // Thay bằng userId thực tế
    const accessToken = localStorage.getItem('accessToken');

    fetch(`http://localhost:5076/api/wallet/check-balances?userId=${userId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${accessToken}`,
        },
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        if (data.status === "Success") {
            const points = data.data.points;
            document.getElementById("points").innerText = `${points} points`;
        } else {
            console.log("Failed to get points:", data.message);
        }
    })
    .catch(error => {
        console.error("Error fetching data:", error);
        document.getElementById("points").innerText = "Error loading points";
        //nếu token hết hạn
        refreshToken();
    });
}

window.onload = getPoints;

function refreshToken() {
    const refreshToken = localStorage.getItem('refreshToken');

    // yc post với refresh token
    fetch('http://localhost:5076/api/Authentication/refresh-token', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            refreshToken: refreshToken,  
        }),
    })
    .then(response => response.json())  
    .then(data => {
        if (data.status === "Success") {
            const newAccessToken = data.accessToken;
            localStorage.setItem('accessToken', newAccessToken);
            getPoints();  // Cập nhật điểm khi refresh token thành công
        } else {
            console.log("Failed to refresh token:", data.message);
        }
    })
    .catch(error => console.error("Error refreshing token:", error));
}

function handleTopUp() {
    const pointsInput = document.getElementById('points-input');
    const pointsToAdd = parseInt(pointsInput.value);
    const pointsElement = document.getElementById('points');
    const errorMessage = document.getElementById('error-message');

    // Clear previous error message
    errorMessage.textContent = '';

    // Validate the input points
    if (isNaN(pointsToAdd) || pointsToAdd <= 0) {
        // Show error if points input is invalid
        errorMessage.textContent = 'Please enter a valid number greater than 0.';
    } else {
        // Get the current user ID (for simplicity, let's assume user is logged in)
        const userId = localStorage.getItem('userId'); // Example: unique user identifier
        if (!userId) {
            // If no user is logged in, show an error or ask to login
            errorMessage.textContent = 'User is not logged in.';
            return;
        }

        // Get the current balance for this user from localStorage
        let currentBalance = parseInt(localStorage.getItem(userId + '_balance')) || 100; // Default balance is 100

        // Add points to current balance
        currentBalance += pointsToAdd;

        // Update the balance in localStorage
        localStorage.setItem(userId + '_balance', currentBalance);

        // Update balance display on page
        pointsElement.textContent = currentBalance;
    }
}

// On page load, display the balance for the current user
window.onload = function() {
    // Simulate logging in a user (for this example, we set a userId manually)
    // In a real-world app, the userId should come from the login process
    let userId = localStorage.getItem('userId');
    if (!userId) {
        userId = 'user1'; // Default user for demo purposes
        localStorage.setItem('userId', userId);
    }

    // Display the balance for the current user
    const pointsElement = document.getElementById('points');
    let currentBalance = parseInt(localStorage.getItem(userId + '_balance')) || 100; // Default balance is 100
    pointsElement.textContent = currentBalance;
};
