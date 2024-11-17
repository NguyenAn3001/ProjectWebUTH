document.addEventListener("DOMContentLoaded", function () {
    const userId = localStorage.getItem("userId");
    const accessToken = localStorage.getItem("accessToken");

    if (!accessToken) {
        alert("Access token not found. Please log in again.");
        window.location.href = "../../login.html";
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

        const loadingIndicator = document.getElementById('loading');
        loadingIndicator.style.display = 'block';

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
                    document.getElementById('error-message').innerText = "";
                } else {
                    document.getElementById('error-message').innerText = "Failed to top up points.";
                }
            })
            .catch(error => {
                document.getElementById('error-message').innerText = "An error occurred while topping up points.";
            })
            .finally(() => {
                loadingIndicator.style.display = 'none';
            });
    });
});
