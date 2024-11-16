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
    fetch('http://localhost:5076/api/Authentication/refresh', {
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
            getPoints();
        } else {
            console.log("Failed to refresh token:", data.message);
        }
    })
    .catch(error => console.error("Error refreshing token:", error));
}

function handleTopUp() {
    const pointsInput = document.getElementById("points-input").value; // Lấy giá trị từ ô nhập liệu
    const errorMessage = document.getElementById("error-message");
    const currentPointsElement = document.getElementById("current-points");

    // Kiểm tra giá trị nhập liệu
    if (!pointsInput || pointsInput <= 0) {
        errorMessage.textContent = "Please enter a valid number of points."; // Hiển thị thông báo lỗi
        return;  // Dừng nếu không có giá trị hợp lệ
    }

    // Gửi yêu cầu POST để thêm điểm
    const userId = "36567bfc-7fe6-444c-4657-08dd04e43ea1";  // Thay bằng userId thực tế
    const accessToken = localStorage.getItem('accessToken');

    fetch('http://localhost:5076/api/wallet/add-points', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`,
        },
        body: JSON.stringify({
            userId: userId,
            points: parseInt(pointsInput),  // Chuyển giá trị nhập thành số nguyên
        }),
    })
    .then(response => response.json())  // Chuyển về JSON
    .then(data => {
        if (data.status === "Success") {
            const newPoints = data.data;  // Dữ liệu trả về là số điểm hiện tại
            currentPointsElement.innerText = `Current Points: ${newPoints}`;
            errorMessage.textContent = "";  // Xóa thông báo lỗi nếu có
            alert(data.message);  // Hiển thị thông báo thành công
        } else {
            errorMessage.textContent = data.message;  // Hiển thị lỗi từ server
        }
    })
    .catch(error => {
        errorMessage.textContent = "Error adding points."; // Hiển thị lỗi nếu có sự cố với API
    });
}

// Cập nhật điểm mặc định là 100 khi trang được tải
document.addEventListener("DOMContentLoaded", () => {
    const currentPointsElement = document.getElementById("current-points");
    currentPointsElement.innerText = "Current Points: 100";  // Điểm mặc định là 100
});