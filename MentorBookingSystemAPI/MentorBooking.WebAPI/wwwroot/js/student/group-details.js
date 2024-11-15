// Lấy dữ liệu từ localStorage và hiển thị danh sách nhóm
window.onload = function() {
    displayGroups();
};

// Hàm hiển thị danh sách nhóm từ localStorage
function displayGroups() {
    const groupContainer = document.querySelector('.group__contain'); // Phần tử chứa nhóm

    // Lấy danh sách nhóm từ localStorage
    const groups = JSON.parse(localStorage.getItem('projectGroups')) || [];

    // Xóa các nhóm cũ trong container trước khi thêm lại danh sách mới
    groupContainer.innerHTML = '';

    // Nếu không có nhóm, không làm gì
    if (groups.length === 0) {
        groupContainer.innerHTML = '<p>No groups found. Please create a group.</p>';
        groupContainer.style.gridTemplateColumns = 'none';
        groupContainer.style.maxWidth = '50%';
    }

    // Lặp qua danh sách nhóm và hiển thị chúng
    groups.forEach((group, index) => {
        const groupDiv = document.createElement('div');
        groupDiv.classList.add('group__info');

        // Tạo phần tử hiển thị tên nhóm
        const groupName = document.createElement('p');
        groupName.innerHTML = `<strong>Group Name:</strong> <span>${group.name}</span>`;
        
        // Tạo phần tử hiển thị chủ đề dự án
        const projectTopic = document.createElement('p');
        projectTopic.innerHTML = `<strong>Project Topic:</strong> <span>${group.topic}</span>`;
        
        // Tạo phần tử hiển thị thành viên
        const members = document.createElement('p');
        members.innerHTML = `<strong>Members:</strong> <span>${group.members.join(', ')}</span>`;

        // Tạo phần tử div chứa các nút Edit và Delete
        const buttonDiv = document.createElement('div');
        buttonDiv.classList.add('group__btn');

        // Tạo nút chỉnh sửa
        const editButton = document.createElement('button');
        editButton.classList.add('btn-hover');
        editButton.innerText = 'Edit';
        editButton.onclick = function() {
            // Lưu index của nhóm cần chỉnh sửa vào localStorage
            localStorage.setItem('editGroupIndex', index);
            // Chuyển đến trang tạo nhóm
            window.location.href = 'create-group.html';
        };

        // Tạo nút xóa
        const deleteButton = document.createElement('button');
        deleteButton.classList.add('btn-hover');
        deleteButton.innerText = 'Delete';
        deleteButton.onclick = function() {
            deleteGroup(index); // Gọi hàm xóa nhóm theo index
        };

        // Thêm cả hai nút vào trong div group__btn
        buttonDiv.appendChild(editButton);
        buttonDiv.appendChild(deleteButton);

        // Thêm tất cả các phần tử vào nhóm
        groupDiv.appendChild(groupName);
        groupDiv.appendChild(projectTopic);
        groupDiv.appendChild(members);
        groupDiv.appendChild(buttonDiv);  // Thêm div chứa nút vào trong nhóm

        // Thêm nhóm vào container
        groupContainer.appendChild(groupDiv);
    });

    // Đảm bảo nút "Add" luôn hiển thị
    const addGroupBtn = document.createElement('a');
    addGroupBtn.href = "create-group.html";
    addGroupBtn.id = "add-group-btn";
    addGroupBtn.classList.add("btn-hover");
    addGroupBtn.innerText = "Add";
    groupContainer.appendChild(addGroupBtn);
}

// Hàm xóa nhóm khỏi localStorage
function deleteGroup(index) {
    // Lấy danh sách nhóm từ localStorage
    const groups = JSON.parse(localStorage.getItem('projectGroups')) || [];

    // Xóa nhóm tại vị trí index
    groups.splice(index, 1);

    // Cập nhật lại localStorage sau khi xóa nhóm
    localStorage.setItem('projectGroups', JSON.stringify(groups));

    // Hiển thị lại danh sách nhóm sau khi xóa
    displayGroups();
}
