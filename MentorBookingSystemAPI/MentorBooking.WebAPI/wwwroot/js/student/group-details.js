// Hàm lấy dữ liệu nhóm từ API
async function fetchGroupData() {
    try {
        const apiUrl = 'http://localhost:5076/api/group'; 
        const token = localStorage.getItem('accessToken');  // Lấy token từ localStorage

        if (!token) {
            console.error('Không tìm thấy access token');
            return;
        }

        // Gửi yêu cầu đến API với header Authorization
        const response = await fetch(apiUrl, {
            method: 'GET',  // Đảm bảo là phương thức GET
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,  // Thêm token vào header
            }
        });

        // Kiểm tra nếu response OK (mã trạng thái 200)
        if (!response.ok) {
            throw new Error(`Không thể lấy dữ liệu nhóm: ${response.statusText}`);
        }

        // Phân tích dữ liệu JSON trả về
        const groupData = await response.json();
        console.log('Dữ liệu nhóm đã lấy:', groupData);  // Ghi log dữ liệu để kiểm tra

        // Gọi hàm để hiển thị dữ liệu nhóm
        displayGroupData(groupData);
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu:', error);
    }
}

// Hàm hiển thị dữ liệu nhóm trên trang
function displayGroupData(response) {
    const groupContainer = document.querySelector('.group__contain'); // Tham chiếu đến phần tử chứa nhóm

    // Xóa bất kỳ dữ liệu cũ nào
    groupContainer.innerHTML = '';

    const groups = response.data || [];  // Lấy mảng nhóm từ đối tượng response

    if (groups.length === 0) {
        groupContainer.innerHTML = '<p>Không có nhóm. Hãy tạo nhóm mới.</p>';
    } else {
        groups.forEach((group, index) => {
            const groupDiv = document.createElement('div');
            groupDiv.classList.add('group__info');

            // Hiển thị tên nhóm
            const groupNameElement = document.createElement('p');
            groupNameElement.innerHTML = `<strong>Group Name:</strong> ${group.groupName}`;

            // Hiển thị chủ đề dự án
            const projectTopicElement = document.createElement('p');
            projectTopicElement.innerHTML = `<strong>Topic:</strong> ${group.topic}`;

            // Hiển thị người tạo nhóm (Creator)
            const creatorElement = document.createElement('p');
            creatorElement.innerHTML = `<strong>Creator:</strong> ${group.creator.name}`;

            // Tạo danh sách thành viên (Members)
            const membersElement = document.createElement('p');
            const members = group.members || [];  // Lấy danh sách thành viên, nếu có

            // Gộp thành viên vào một chuỗi
            let membersList = members.map(member => `${member.name}`).join(', ');

            // Hiển thị tất cả thành viên trên cùng một dòng
            if (membersList) {
                membersElement.innerHTML = `<strong>Members:</strong> ${membersList}`;
            } else {
                membersElement.innerHTML = `<strong>Members:</strong> Không có thành viên trong nhóm.`;
            }

            // Thêm các phần tử vào groupDiv
            groupDiv.appendChild(groupNameElement);
            groupDiv.appendChild(projectTopicElement);
            groupDiv.appendChild(creatorElement);
            groupDiv.appendChild(membersElement);

            // Tạo div chứa các nút "Sửa" và "Xóa"
            const buttonDiv = document.createElement('div');
            buttonDiv.classList.add('group__btn');  // Thêm class cho div chứa nút

            // Tạo nút "Sửa"
            const editButton = document.createElement('button');
            editButton.classList.add('btn-hover');
            editButton.innerText = 'Add & Edit';
            editButton.onclick = function () {
                // Lưu dữ liệu nhóm cần chỉnh sửa vào localStorage
                localStorage.setItem('editGroupData', JSON.stringify(group));  // Lưu toàn bộ nhóm vào localStorage
                // Chuyển đến trang tạo nhóm
                window.location.href = 'create-group.html';
            };

            // Tạo nút "Xóa"
            const deleteButton = document.createElement('button');
            deleteButton.classList.add('btn-hover');
            deleteButton.innerText = 'Delete';
            deleteButton.onclick = function () {
                deleteGroup(group.groupId); // Gọi hàm xóa nhóm theo groupId
            };

            // Thêm cả hai nút vào trong div button-container
            buttonDiv.appendChild(editButton);
            buttonDiv.appendChild(deleteButton);

            // Thêm buttonDiv vào groupDiv
            groupDiv.appendChild(buttonDiv);

            // Thêm groupDiv vào groupContainer
            groupContainer.appendChild(groupDiv);
        });
    }

    // Thêm nút "Thêm" ở cuối
    const addGroupBtn = document.createElement('a');
    addGroupBtn.href = 'create-group.html';
    addGroupBtn.id = 'add-group-btn';
    addGroupBtn.classList.add('btn-hover');
    addGroupBtn.innerText = 'Add Group';
    groupContainer.appendChild(addGroupBtn);
}

// Hàm xóa nhóm
async function deleteGroup(groupId) {
    try {
        const apiUrl = `http://localhost:5076/api/group/your-groups/${groupId}`;
        const token = localStorage.getItem('accessToken');  // Lấy token từ localStorage

        if (!token) {
            console.error('Không tìm thấy access token');
            return;
        }

        // Gửi yêu cầu xóa nhóm đến API
        const response = await fetch(apiUrl, {
            method: 'DELETE',  // Phương thức xóa
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,  // Thêm token vào header
            }
        });

        // Kiểm tra nếu xóa thành công
        if (!response.ok) {
            throw new Error(`Không thể xóa nhóm: ${response.statusText}`);
        }

        // Cập nhật lại giao diện sau khi xóa
        alert('Xóa nhóm thành công!');
        
        // Reset trang (reload lại trang để tải lại dữ liệu)
        window.location.reload();  // Hoặc bạn có thể gọi lại fetchGroupData() để tải lại dữ liệu mà không reload trang

    } catch (error) {
        console.error('Lỗi khi xóa nhóm:', error);
    }
}


// Hàm lưu dữ liệu nhóm vào localStorage
function saveGroupToLocal() {
    const groupName = document.getElementById('groupname').value.trim();
    const projectTopic = document.getElementById('project-topic').value.trim();

    if (!groupName || !projectTopic) {
        alert('Please fill out all fields!');
        return;
    }

    // Tạo đối tượng nhóm
    const groupData = {
        groupName: groupName,
        topic: projectTopic,
        members: []  // Bạn có thể thêm danh sách thành viên ở đây nếu cần
    };

    // Lưu vào localStorage
    localStorage.setItem('newGroupData', JSON.stringify(groupData));

    alert('Group data saved to localStorage!');
    // Nếu muốn chuyển trang hoặc làm gì đó sau khi lưu, có thể thêm mã ở đây
    // Ví dụ: window.location.href = 'create-group.html'; // Chuyển trang
}

// Lắng nghe sự kiện click của nút "Add Group"
document.getElementById('add-group-btn').addEventListener('click', saveGroupToLocal);

// Gọi hàm lấy dữ liệu khi trang tải xong
window.onload = fetchGroupData;
