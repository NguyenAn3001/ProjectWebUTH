// Lấy tham chiếu đến các phần tử HTML
const groupNameInput = document.getElementById('groupname');
const projectTopicInput = document.getElementById('project-topic');
const memberListElement = document.getElementById('member__List');
const emailInput = document.getElementById('email');
const addMemberBtn = document.getElementById('add-member-btn');
const emailGroupDiv = document.getElementById('email-group');

// Lưu nhóm mới hoặc chỉnh sửa nhóm
async function saveGroup() {
    const groupName = groupNameInput.value.trim();
    const projectTopic = projectTopicInput.value.trim();

    if (!groupName || !projectTopic) {
        alert('Please fill out all fields!');
        return;
    }

    const members = [];
    // Chuyển các email thành viên từ danh sách thành viên vào mảng
    memberListElement.querySelectorAll('p').forEach(member => {
        members.push({ email: member.textContent.trim() });
    });

    const requestBody = {
        groupName: groupName,
        topic: projectTopic,
        members: members // Mảng thành viên được thêm vào
    };

    const token = localStorage.getItem('accessToken');

    try {
        const response = await fetch('http://localhost:5076/api/group/new-group', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(requestBody)
        });

        if (response.ok) {
            alert('Tạo nhóm thành công!');
            window.location.href = 'group-details.html';
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || 'Unable to create group!'}`);
        }
    } catch (error) {
        console.error(error);
        alert('Error connecting to the server.');
    }
}

// Chế độ chỉnh sửa: lấy thông tin nhóm và cho phép thêm/sửa thành viên
async function editGroup(groupId) {
    const token = localStorage.getItem('accessToken');

    try {
        const response = await fetch(`http://localhost:5076/api/group/${groupId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const groupData = await response.json();
            groupNameInput.value = groupData.groupName;
            projectTopicInput.value = groupData.topic;

            // Hiển thị danh sách thành viên
            memberListElement.innerHTML = groupData.members.map(member => {
                return `<p>${member.email}</p>`;  // Chuyển thành phần danh sách thành viên vào <p> element
            }).join('');

            // Hiển thị email và nút Add member nếu đang ở chế độ chỉnh sửa
            emailGroupDiv.style.display = 'block';
            addMemberBtn.style.display = 'inline-block';

            // Lưu groupId vào localStorage để sử dụng khi chỉnh sửa thành viên
            localStorage.setItem('editGroupId', groupId);
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || 'Unable to fetch group data!'}`);
        }
    } catch (error) {
        console.error(error);
        alert('Error connecting to the server.');
    }
}

// Kiểm tra nhóm đã tồn tại và hiển thị form thêm thành viên
async function checkIfGroupExists() {
    const groupId = localStorage.getItem('editGroupId');
    if (!groupId) {
        emailGroupDiv.style.display = 'none';  // Ẩn form thêm thành viên nếu chưa có nhóm
        addMemberBtn.style.display = 'none';  // Ẩn nút Add member
        return;
    }

    const token = localStorage.getItem('accessToken');

    try {
        const response = await fetch(`http://localhost:5076/api/group/${groupId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const groupData = await response.json();

            if (groupData.members.length > 0) {
                memberListElement.innerHTML = groupData.members.map(member => `<p>${member.email}</p>`).join('');
            }

            // Hiển thị form và nút "Thêm thành viên" nếu nhóm tồn tại
            emailGroupDiv.style.display = 'block';
            addMemberBtn.style.display = 'inline-block';
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || 'Unable to fetch group data!'}`);
        }
    } catch (error) {
        console.error(error);
        alert('Error connecting to the server.');
    }
}

// Thêm thành viên vào nhóm
function addMember() {
    const email = emailInput.value.trim();

    if (!email) {
        alert('Please enter a member email!');
        return;
    }

    // Hiển thị thành viên đã thêm vào danh sách
    const newMember = document.createElement('p');
    newMember.textContent = email;
    memberListElement.appendChild(newMember);

    // Làm trống trường nhập email sau khi thêm
    emailInput.value = '';
}

// Khi trang được tải, kiểm tra xem nhóm có tồn tại không
window.onload = function() {
    checkIfGroupExists();  // Kiểm tra xem nhóm đã tồn tại hay chưa
    
    const editGroupId = localStorage.getItem('editGroupId');
    if (editGroupId) {
        editGroup(editGroupId);  // Nếu có groupId, chế độ chỉnh sửa
    } else {
        emailGroupDiv.style.display = 'none';  // Chỉ ẩn email khi tạo nhóm lần đầu
        addMemberBtn.style.display = 'none';  // Ẩn nút thêm thành viên
    }
};
