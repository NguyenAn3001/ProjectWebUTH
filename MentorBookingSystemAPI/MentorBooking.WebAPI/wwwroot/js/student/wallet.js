function addPoints() {
    const input = document.getElementById("points-input");
    const pointsToAdd = parseInt(input.value, 10);
    const balanceElement = document.querySelector(".wallet-balance");
    const errorMessage = document.getElementById("error-message");
    let currentBalance = parseInt(balanceElement.textContent.split(" ")[0], 10);

    errorMessage.textContent = "";
    if (!isNaN(pointsToAdd) && pointsToAdd > 0) {
        currentBalance += pointsToAdd;
        balanceElement.textContent = currentBalance + " points";
        localStorage.setItem("currentBalance", currentBalance); 
        input.value = ""; 
    } else {
        errorMessage.textContent="Please enter a valid points";
    }
}

document.addEventListener("DOMContentLoaded", function() {
    const balanceElement = document.querySelector(".wallet-balance");
    const savedBalance = localStorage.getItem("currentBalance");

    if (savedBalance) {
        balanceElement.textContent = savedBalance + " points";
    }
});
