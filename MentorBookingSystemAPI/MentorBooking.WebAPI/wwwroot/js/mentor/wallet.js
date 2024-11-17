document.addEventListener("DOMContentLoaded", function () {
    // Lấy userId từ localStorage
    const userId = localStorage.getItem('userId'); // Key cần phải đúng

    if (!userId) {
        console.error("User ID not found in localStorage");
        return;
    }

    // URL API với userId động
    const apiUrl = `http://localhost:5076/api/wallet/check-balances?userId=${userId}`;
    const authToken = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwMjZmOGZjNS03YTE2LTQxZDMtYWIzZC0wOGRkMDU5ZTQxMGMiLCJ1bmlxdWVfbmFtZSI6InRkaGlsbWVudG9yMSIsImp0aSI6ImM4OWI2N2EyLTAxMmEtNDBkOC05NmU3LTA3NTc4ZjFhODliMiIsInJvbGUiOiJNZW50b3IiLCJuYmYiOjE3MzE3NTgyNzgsImV4cCI6MTczMTc2OTA3OCwiaWF0IjoxNzMxNzU4Mjc4LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTQ3IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE0NyJ9.IotmP_AjWZ1CWSMiPwXsAjw0RsCxKAY0a5vPqj6mRhw';

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
