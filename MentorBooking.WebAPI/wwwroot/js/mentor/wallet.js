document.addEventListener("DOMContentLoaded", function () {
    // Lấy userId từ localStorage
    const userId = localStorage.getItem('userId'); // Key cần phải đúng

    if (!userId) {
        console.error("User ID not found in localStorage");
        return;
    }

    // URL API với userId động
    const apiUrl = `http://localhost:5076/api/wallet/check-balances?userId=${userId}`;
    const authToken = `Bearer ${localStorage.getItem('authToken')}`;

    // Element để hiển thị điểm
    const balanceElement = document.getElementById("balanceAmount");

    // Fetch API
    fetch(apiUrl, {
        method: 'GET',
        headers: {
            'accept': '*/*',
            'Authorization': authToken,
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        // Cập nhật điểm từ response
        if (data.status === "Success") {
            balanceElement.textContent = data.data.points || 0; // Điểm mặc định là 0
        } else {
            console.error("Failed to fetch balance:", data.message);
        }
    })
    .catch(error => {
        console.error("Error fetching balance:", error);
    });
});
