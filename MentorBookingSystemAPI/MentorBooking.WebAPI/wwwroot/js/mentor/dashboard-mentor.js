const apiBaseUrl = "http://localhost:5076/api/BookingMentor";
const accessToken = localStorage.getItem("accessToken");
const sessionId = localStorage.getItem("sessionId"); // Retrieve the dynamic session ID
const sessionsContainer = document.getElementById("sessionsContainer");

if (!accessToken || !sessionId) {
    Swal.fire({
        icon: "error",
        title: "Authentication Error",
        text: "Access token or session ID not found. Please log in again.",
    }).then(() => {
        window.location.href = "../../login.html";
    });
} else {
    console.log("Access Token:", accessToken);
    console.log("SessionId:", sessionId);
}

// Fetch booked sessions for the mentor
function fetchSessions() {
    fetch(`${apiBaseUrl}/get-booking-mentor-session?SessionId=${sessionId}`, {
        method: "GET",
        headers: {
            Authorization: `Bearer ${accessToken}`,
            Accept: "*/*",
        },
    })
        .then((response) => {
            console.log("Fetch Sessions Response Status:", response.status);
            return response.json();
        })
        .then((data) => {
            console.log("Fetch Sessions Response Data:", data);
            if (data.status === "Success") {
                displaySessions(data.data);
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Failed to fetch sessions.",
                });
            }
        })
        .catch((error) => {
            console.error("Network Error:", error);
            Swal.fire({
                icon: "error",
                title: "Network Error",
                text: "An error occurred while fetching sessions.",
            });
        });
}

// Display fetched sessions
// Display fetched sessions
// Display the session for a single group (one session per mentor)
function displaySessions(session) {
    sessionsContainer.innerHTML = ""; // Clear existing sessions

    if (session) {
        const sessionElement = document.createElement("div");
        sessionElement.classList.add("session");
        sessionElement.innerHTML = `
            <p><strong>Session ID:</strong> ${session.sessionId}</p>
            <p><strong>Group ID:</strong> ${session.groupId}</p>
            <p><strong>Session Count:</strong> ${session.sessionCount}</p>
            <p><strong>Points per Session:</strong> ${session.pointPerSession}</p>
            <div class="buttons" id="buttons-${session.sessionId}">
                <button class="btn accept" onclick="updateSessionStatus('${session.sessionId}', true, this)">Accept</button>
                <button class="btn reject" onclick="updateSessionStatus('${session.sessionId}', false, this)">Reject</button>
            </div>
        `;
        sessionsContainer.appendChild(sessionElement);
    } else {
        sessionsContainer.innerHTML = "<p>No sessions available for this mentor.</p>";
    }
}

// Accept or reject a session
function updateSessionStatus(sessionId, isAccepted, button) {
    console.log("Updating session status:", sessionId, isAccepted);

    fetch(`${apiBaseUrl}/accept-booking?SessionId=${sessionId}&accept=${isAccepted}`, {
        method: "GET",
        headers: {
            Authorization: `Bearer ${accessToken}`,
            Accept: "*/*",
        },
    })
        .then((response) => {
            console.log("Update Session Response Status:", response.status);
            return response.json();
        })
        .then((data) => {
            console.log("Update Session Response Data:", data);

            if (data.status === "Success") {
                Swal.fire({
                    icon: "success",
                    title: "Success",
                    text: isAccepted
                        ? "Session accepted successfully."
                        : "Session rejected successfully.",
                });

                // Option 1: Disable buttons
                const buttonsContainer = document.getElementById(`buttons-${sessionId}`);
                if (buttonsContainer) {
                    buttonsContainer.innerHTML = `<p>${isAccepted ? "Accepted" : "Rejected"}</p>`;
                }

                // Option 2: Remove session element
                // const sessionElement = document.querySelector(`.session[data-id="${sessionId}"]`);
                // if (sessionElement) {
                //     sessionElement.remove();
                // }

            } else {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Failed to update session status.",
                });
            }
        })
        .catch((error) => {
            console.error("Network Error:", error);
            Swal.fire({
                icon: "error",
                title: "Network Error",
                text: "An error occurred while updating the session status.",
            });
        });
}


// Debugging step: validate SessionId format
console.log("Initial Debug - SessionId:", sessionId);

// Check if SessionId matches expected format
const validSessionIdPattern = /^[a-zA-Z0-9-_]+$/; // Replace with your specific format
if (!validSessionIdPattern.test(sessionId)) {
    console.error("Invalid SessionId format:", sessionId);
    Swal.fire({
        icon: "error",
        title: "Validation Error",
        text: "Session ID is not in a valid format. Please check and try again.",
    });
}

// Initial fetch of sessions
fetchSessions();
