// Hàm lấy dữ liệu nhóm từ API
async function fetchGroupData() {
    try {
        const apiUrl = 'http://localhost:5076/api/group';
        const token = localStorage.getItem('accessToken'); // Lấy token từ localStorage

        if (!token) {
            console.error('Không tìm thấy access token');
            return;
        }

        // Gửi yêu cầu đến API với header Authorization
        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Thêm token vào header
            }
        });

        // Kiểm tra nếu response không OK
        if (!response.ok) {
            throw new Error(`Không thể lấy dữ liệu nhóm: ${response.statusText}`);
        }

        // Phân tích dữ liệu JSON trả về
        const groupData = await response.json();
        console.log('Dữ liệu nhóm đã lấy:', groupData);

        // Gọi hàm hiển thị dữ liệu nhóm
        displayGroupData(groupData);
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu:', error);
    }
}

// Hàm hiển thị dữ liệu nhóm trên trang
function displayGroupData(response) {
    const groupContainer = document.querySelector('.group__contain'); // Tham chiếu đến phần tử chứa nhóm

    // Xóa dữ liệu cũ
    groupContainer.innerHTML = '';

    const groups = response.data || []; // Lấy mảng nhóm từ response

    if (groups.length === 0) {
        groupContainer.innerHTML = '<p>Không có nhóm. Hãy tạo nhóm mới.</p>';
    } else {
        groups.forEach((group) => {
            const groupDiv = document.createElement('div');
            groupDiv.classList.add('group__info');

            // Thông tin nhóm
            groupDiv.innerHTML = `
                <p><strong>Group Name:</strong> ${group.groupName}</p>
                <p><strong>Topic:</strong> ${group.topic}</p>
                <p><strong>Creator:</strong> ${group.creator.name}</p>
                <p><strong>Members:</strong> ${group.members?.map(member => member.name).join(', ') || 'Không có thành viên trong nhóm.'}</p>
            `;

            // Tạo các nút "Add & Edit" và "Delete"
            const buttonDiv = document.createElement('div');
            buttonDiv.classList.add('group__btn');

            // Nút "Add & Edit"
            const editButton = document.createElement('button');
            editButton.classList.add('btn-hover');
            editButton.innerText = 'Add & Edit';
            editButton.onclick = function () {
                saveGroupToLocal(group.groupId, true); // Lưu groupId vào localStorage và đặt isEditing = true
                window.location.href = 'create-group.html'; // Điều hướng
            };

            // Nút "Delete"
            const deleteButton = document.createElement('button');
            deleteButton.classList.add('btn-hover');
            deleteButton.innerText = 'Delete';
            deleteButton.onclick = function () {
                deleteGroup(group.groupId); // Gọi hàm xóa nhóm
            };

            // Thêm nút vào buttonDiv
            buttonDiv.appendChild(editButton);
            buttonDiv.appendChild(deleteButton);
            groupDiv.appendChild(buttonDiv);

            // Thêm groupDiv vào groupContainer
            groupContainer.appendChild(groupDiv);
        });
    }

    // Nút "Add Group"
    const addGroupBtn = document.createElement('a');
    addGroupBtn.href = 'create-group.html';
    addGroupBtn.id = 'add-group-btn';
    addGroupBtn.classList.add('btn-hover');
    addGroupBtn.innerText = 'Add Group';
    addGroupBtn.onclick = function () {
        saveGroupToLocal(null, false); // Xóa groupId và đặt isEditing = false
    };
    groupContainer.appendChild(addGroupBtn);
}

// Hàm lưu Group ID vào localStorage và thiết lập isEditing
function saveGroupToLocal(groupId, isEditing) {
    if (groupId) {
        localStorage.setItem('selectedGroupId', groupId); // Lưu ID vào localStorage
    } else {
        localStorage.removeItem('selectedGroupId'); // Xóa nếu không có groupId
    }
    localStorage.setItem('isEditing', isEditing); // Lưu trạng thái isEditing
    console.log(`Group ID ${groupId} và isEditing=${isEditing} đã được lưu.`);
}

// Hàm xóa nhóm
async function deleteGroup(groupId) {
    try {
        // Hiển thị câu hỏi xác nhận với SweetAlert2
        const result = await Swal.fire({
            title: 'Bạn có chắc muốn xóa nhóm này?',
            text: 'Hành động này không thể hoàn tác!',
            icon: 'warning',
            showCancelButton: true, // Hiển thị nút hủy
            confirmButtonText: 'Có, xóa nhóm!',
            cancelButtonText: 'Không, hủy bỏ!',
        });

        // Nếu người dùng nhấn 'Có, xóa nhóm!'
        if (result.isConfirmed) {
            const apiUrl = `http://localhost:5076/api/group/your-groups/${groupId}`;
            const token = localStorage.getItem('accessToken'); // Lấy token từ localStorage

            if (!token) {
                console.error('Không tìm thấy access token');
                return;
            }

            // Gửi yêu cầu xóa nhóm
            const response = await fetch(apiUrl, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                }
            });

            // Kiểm tra nếu xóa không thành công
            if (!response.ok) {
                throw new Error(`Không thể xóa nhóm: ${response.statusText}`);
            }

            // Thông báo thành công với SweetAlert2
            Swal.fire({
                icon: 'success',
                title: 'Xóa nhóm thành công!',
                showConfirmButton: false,
                timer: 1500
            });

            window.location.reload(); // Tải lại trang
        } else {
            // Nếu người dùng nhấn 'Không, hủy bỏ!'
            console.log('Hành động xóa nhóm đã bị hủy bỏ.');
        }
    } catch (error) {
        // Thông báo lỗi với SweetAlert2
        Swal.fire({
            icon: 'error',
            title: 'Lỗi khi xóa nhóm',
            text: error.message,
        });
    }
}


// Xử lý khi trang tải xong
window.onload = function () {
    localStorage.setItem('isEditing', false); // Đặt giá trị mặc định
    fetchGroupData(); // Lấy dữ liệu nhóm
};
