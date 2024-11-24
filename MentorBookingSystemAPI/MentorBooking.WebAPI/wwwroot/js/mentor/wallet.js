// The element where the balance will be displayed
const balanceAmount = document.getElementById("balanceAmount");

// Function to fetch the mentor's wallet balance
async function fetchWalletBalance() {
    try {
        const userId = localStorage.getItem('userId'); // Replace with the actual user ID (e.g., from localStorage or context)
        const token = localStorage.getItem('accessToken'); // Get the token from localStorage
        const response = await fetch(`http://localhost:5076/api/wallet/check-balances?userId=${userId}`, {
            method: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': `Bearer ${token}`,
            },
        });

        const data = await response.json();

        // Log the full response for debugging
        console.log("API Response: ", data);

        if (data.status === "Success") {
            // Update the balance in the UI
            balanceAmount.textContent = data.data.points;
        } else {
            balanceAmount.textContent = 'Error retrieving points.';
        }
    } catch (error) {
        console.error('Error fetching wallet balance:', error);
        balanceAmount.textContent = 'Error fetching balance.';
    }
}

// Call the function to fetch the wallet balance
fetchWalletBalance();

document.querySelector("button").addEventListener('click', function (e) {
    e.preventDefault();
    const pointsToAdd = document.getElementById('points-input').value;

    if (!pointsToAdd || pointsToAdd <= 0) {
        document.getElementById('error-message').innerText = "Please enter a valid number of points.";
        return;
    }

    // loadingTopUp.style.display = 'block';

    const topUpData = {
        userId: userId,
        points: pointsToAdd
    };

    fetch("http://localhost:5076/api/wallet/add-point", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(topUpData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.status === "Success") {
                fetchWalletBalance();
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Points topped up successfully!'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Top Up Failed',
                    text: 'Failed to top up points.'
                });
            }
        })
        .catch(() => {
            Swal.fire({
                icon: 'error',
                title: 'Network Error',
                text: 'An error occurred while topping up points.'
            });
        })
        .finally(() => {
            loadingTopUp.style.display = 'none';
        });
});

document.getElementById("withdrawButton").addEventListener('click', function (e) {
    e.preventDefault();
    const pointsToWithdraw = document.getElementById('pointsInput').value;
    const paypalAddress = document.getElementById('paypalInput').value;
    const loadingWithdraw = document.getElementById('loading-withdraw');
    const accessToken = localStorage.getItem("accessToken");


    if (!pointsToWithdraw || pointsToWithdraw <= 0 || !paypalAddress) {
        alert("Please enter a valid number of points and a PayPal address.");
        return;
    }

    loadingWithdraw.style.display = 'block';

    const withdrawData = {
        points: pointsToWithdraw,
        paypalAddress: paypalAddress
    };

    fetch("http://localhost:5076/api/wallet/cash-point", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(withdrawData)
    })
        .then(response => {
            // Kiểm tra nếu mã HTTP không phải 200
            if (!response.ok && response.status !== 304) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json(); // Nếu là 200 hoặc 304 thì xử lý JSON
        })
        .then(data => {
            if (data.status === "Success") {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: data.message
                });
                fetchWalletBalance(); // Cập nhật số dư
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Withdrawal Failed',
                    text: data.message || 'Failed to withdraw points.'
                });
            }
        })
        .catch(error => {
            console.error("Error:", error); // Log lỗi chi tiết
            Swal.fire({
                icon: 'error',
                title: 'Network Error',
                text: 'An error occurred while withdrawing points.'
            });
        });
    
});