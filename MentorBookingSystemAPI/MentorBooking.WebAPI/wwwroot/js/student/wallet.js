document.addEventListener("DOMContentLoaded", function() {
    const userId = localStorage.getItem("userId");
    const accessToken = localStorage.getItem("accessToken");

    // Kiểm tra nếu không có access token
    if (!accessToken) {
        alert("Access token not found. Please log in again.");
        window.location.href = "../../login.html"; // Chuyển hướng về trang login nếu không có token
        return;
    }

    // Gọi API kiểm tra số dư ví
    fetch(`http://localhost:5076/api/wallet/check-balances?userId=${userId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${accessToken}`, // Thêm Authorization với token
            'Accept': '*/*'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.status === "Success") {
            console.log("Balance fetched successfully:", data);
            // Cập nhật thông tin số dư ví
            document.getElementById('walletBalance').innerText = `${data.data.points} points`;
        } else {
            console.error("Failed to get balance:", data.message);
            alert("Failed to fetch balance.");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        alert("An error occurred while fetching balance.");
    });

    // Handle top-up points form submission
    document.querySelector("button").addEventListener('click', function(e) {
        e.preventDefault();
        const pointsToAdd = document.getElementById('points-input').value;

        // Kiểm tra điểm nhập vào
        if (!pointsToAdd || pointsToAdd <= 0) {
            document.getElementById('error-message').innerText = "Please enter a valid number of points.";
            return;
        }

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
                // Cập nhật số dư mới sau khi nạp điểm
                document.getElementById('walletBalance').innerText = `${data.data.newBalance} points`;
                document.getElementById('error-message').innerText = ""; // Xóa lỗi nếu nạp điểm thành công
            } else {
                document.getElementById('error-message').innerText = "Failed to top up points.";
            }
        })
        .catch(error => {
            console.error("Error:", error);
            document.getElementById('error-message').innerText = "An error occurred while topping up points.";
        });
    });
});
