// Lấy tham chiếu đến các phần tử HTML
const groupNameInput = document.getElementById('groupname');
const projectTopicInput = document.getElementById('project-topic');
const emailInput = document.getElementById('email');
const memberListContainer = document.getElementById('member__List');

// Mảng lưu các thành viên
let members = [];

// Thêm thành viên vào danh sách
function addMember() {
    const email = emailInput.value.trim();
    if (email && groupNameInput.value.trim() && projectTopicInput.value.trim()) {
        members.push(email);
        updateMemberList();
        emailInput.value = '';  // Xóa trường email
    } else {
        alert('Vui lòng nhập đầy đủ thông tin!');
    }
}

// Cập nhật danh sách thành viên hiển thị
function updateMemberList() {
    memberListContainer.innerHTML = '';  // Xóa danh sách hiện tại
    members.forEach((email, index) => {
        const memberItem = document.createElement('div');
        memberItem.classList.add('member-item');
        memberItem.innerHTML = `
            <p>${email}</p>
            <button type="button" class="btn-hover" onclick="removeMember(${index})">Remove</button>
        `;
        memberListContainer.appendChild(memberItem);
    });
}

// Xóa thành viên
function removeMember(index) {
    members.splice(index, 1);
    updateMemberList();
}

// Lưu thông tin nhóm vào LocalStorage
function saveGroup() {
    const groupName = groupNameInput.value.trim();
    const projectTopic = projectTopicInput.value.trim();
    if (groupName && projectTopic && members.length > 0) {
        const groupData = { name: groupName, topic: projectTopic, members };
        const existingGroups = JSON.parse(localStorage.getItem('projectGroups')) || [];
        existingGroups.push(groupData);
        localStorage.setItem('projectGroups', JSON.stringify(existingGroups));
        alert('Tạo nhóm thành công!');
        window.location.href = 'group-details.html';
    } else {
        alert('Vui lòng nhập đầy đủ thông tin!');
    }
}

// Điền thông tin vào form nếu đang chỉnh sửa nhóm
function populateEditForm(index) {
    const group = JSON.parse(localStorage.getItem('projectGroups'))[index];
    groupNameInput.value = group.name;
    projectTopicInput.value = group.topic;
    emailInput.value = '';  // Không cần email khi chỉnh sửa
    members = [...group.members];
    updateMemberList();
}

// Kiểm tra xem có thông tin chỉnh sửa từ localStorage không
window.onload = function() {
    const editGroupIndex = localStorage.getItem('editGroupIndex');
    if (editGroupIndex !== null) {
        populateEditForm(editGroupIndex);
    }
};

// Lưu hoặc cập nhật nhóm
function saveGroup() {
    const groupName = groupNameInput.value.trim();
    const projectTopic = projectTopicInput.value.trim();
    if (!groupName || !projectTopic || members.length === 0) {
        alert('Vui lòng điền đầy đủ thông tin!');
        return;
    }

    const groups = JSON.parse(localStorage.getItem('projectGroups')) || [];
    const editGroupIndex = localStorage.getItem('editGroupIndex');

    if (editGroupIndex !== null) {
        groups[editGroupIndex] = { name: groupName, topic: projectTopic, members };
        localStorage.removeItem('editGroupIndex');
    } else {
        groups.push({ name: groupName, topic: projectTopic, members });
    }

    localStorage.setItem('projectGroups', JSON.stringify(groups));
    window.location.href = 'group-details.html';
}
