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

    const requestBody = {
        groupName: groupName,
        topic: projectTopic,
        members: [] // Khi tạo nhóm lần đầu không có thành viên
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
            memberListElement.innerHTML = groupData.members.map(member => `${member.name} (${member.email})`).join(', ');

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