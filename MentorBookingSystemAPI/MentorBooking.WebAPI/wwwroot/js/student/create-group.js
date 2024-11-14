// Get references to the input fields and buttons
const groupNameInput = document.getElementById('groupname');
const projectTopicInput = document.getElementById('project-topic');
const emailInput = document.getElementById('email');
const addMemberBtn = document.querySelector('.group__btn button[type="button"]:first-child');
const completeBtn = document.querySelector('.group__btn button[type="button"]:last-child');
const memberList = document.getElementById('member__List');

// Create an array to store members (emails only)
let members = [];

// Add event listener to the "Add member" button
addMemberBtn.addEventListener('click', function() {
    const groupName = groupNameInput.value.trim();
    const projectTopic = projectTopicInput.value.trim();
    const email = emailInput.value.trim();

    if (groupName && projectTopic && email) {
        // Add the new member's email to the members array
        members.push(email);
        
        // Update the member list display
        updateMemberList();

        // Clear the email input field after adding the member
        emailInput.value = '';
    } else {
        alert('Please fill in all fields');
    }
});

// Update the member list to show emails with a remove button
function updateMemberList() {
    memberList.innerHTML = ''; // Clear existing list
    members.forEach((email, index) => {
        const memberItem = document.createElement('div');
        memberItem.classList.add('member-item');
        
        // Create the email text node
        const emailText = document.createElement('p');
        emailText.textContent = email;

        // Create the remove button
        const removeButton = document.createElement('button');
        removeButton.type = 'button';
        removeButton.classList.add('btn-hover');
        removeButton.textContent = 'Remove';
        removeButton.onclick = function() {
            removeMember(index); // Remove member by index
        };

        // Append the email and remove button to the member item
        memberItem.appendChild(emailText);
        memberItem.appendChild(removeButton);

        // Append the member item to the member list
        memberList.appendChild(memberItem);
    });
}

// Remove member by index
function removeMember(index) {
    // Remove the member from the members array
    members.splice(index, 1);
    
    // Update the member list display after removal
    updateMemberList();
}

// Add event listener to the "Complete" button
completeBtn.addEventListener('click', function() {
    if (members.length > 0) {
        alert('Group creation completed successfully!');
    } else {
        alert('Please add at least one member');
    }
});
