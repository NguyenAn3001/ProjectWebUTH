document.addEventListener("DOMContentLoaded", function () {
    const userId = localStorage.getItem("userId");
    const accessToken = localStorage.getItem("accessToken");
    const loadingWithdraw = document.getElementById('loading-withdraw');
    const loadingTopUp = document.getElementById('loading-topup');
    if (!accessToken) {
        Swal.fire({
            icon: 'error',
            title: 'Authentication Error',
            text: 'Access token not found. Please log in again.'
        }).then(() => {
            window.location.href = "../../login.html";
        });
        return;
    }

    function fetchBalance() {
        fetch(`http://localhost:5076/api/wallet/check-balances?userId=${userId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${accessToken}`,
                'Accept': '*/*'
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.status === "Success") {
                    document.getElementById('walletBalance').innerText = `${data.data.points} points`;
                } else {
                    alert("Failed to fetch balance.");
                }
            })
            .catch(error => {
                alert("An error occurred while fetching balance.");
            })
            .finally(() => {
                loadingTopUp.style.display = 'none'; 
            });
    }

    fetchBalance();

    document.querySelector("button").addEventListener('click', function (e) {
        e.preventDefault();
        const pointsToAdd = document.getElementById('points-input').value;

        if (!pointsToAdd || pointsToAdd <= 0) {
            document.getElementById('error-message').innerText = "Please enter a valid number of points.";
            return;
        }

        loadingTopUp.style.display = 'block';

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
                    fetchBalance();
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
            .then(response => response.json())
            .then(data => {
                if (data.status === "Success") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: data.message
                    });
                    fetchBalance();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Withdrawal Failed',
                        text: 'Failed to withdraw points.'
                    });
                }
            })
            .catch(() => {
                Swal.fire({
                    icon: 'error',
                    title: 'Network Error',
                    text: 'An error occurred while withdrawing points.'
                });
            })
            .finally(() => {
                loadingWithdraw.style.display = 'none';
            });
    });
});
