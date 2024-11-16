// Lấy tham chiếu đến các phần tử HTML
const groupNameInput = document.getElementById('groupname');
const projectTopicInput = document.getElementById('project-topic');
const memberListElement = document.getElementById('member__List');
const userIDInput = document.getElementById('user-id');
const check = localStorage.getItem('isEditing') === 'true'; // Lấy trạng thái "isEditing"

// Mảng lưu thành viên
let members = [];

// Kiểm tra trạng thái và hiển thị giao diện tương ứng
document.getElementById('add group').style.display = check ? 'none' : 'block';
document.getElementById('edit group').style.display = check ? 'block' : 'none';

// Lưu nhóm mới hoặc chỉnh sửa nhóm
async function saveGroup() {
    const groupName = groupNameInput.value.trim();
    const projectTopic = projectTopicInput.value.trim();

    if (!groupName || !projectTopic) {
        alert('Vui lòng nhập đầy đủ thông tin!');
        return;
    }

    const requestBody = {
        groupName: groupName,
        topic: projectTopic,
        members: check ? members : [] // Nếu đang chỉnh sửa, thêm danh sách thành viên
    };

    const token = localStorage.getItem('accessToken');
    const apiUrl = check
        ? `http://localhost:5076/api/group/edit-group` // API khi chỉnh sửa nhóm
        : `http://localhost:5076/api/group/new-group`; // API khi tạo nhóm

    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(requestBody)
        });

        if (response.ok) {
            alert(check ? 'Nhóm đã được chỉnh sửa!' : 'Tạo nhóm thành công!');
            window.location.href = 'group-details.html';
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || 'Tạo group thất bại!'}`);
        }
    } catch (error) {
        console.error(error);
        alert('Error connecting to the server.');
    }
}

// Hàm thêm thành viên vào nhóm
async function addMember() {
    const userId = userIDInput.value.trim(); // Lấy giá trị từ input
    const token = localStorage.getItem('accessToken'); // Lấy token từ localStorage
    const groupId = localStorage.getItem('selectedGroupId'); // Lấy groupId từ localStorage

    if (!userId) {
        alert('Vui lòng nhập userID muốn thêm vào nhóm!');
        return;
    }

    const apiUrl = `http://localhost:5076/api/group/add-member/${groupId}`; // Định nghĩa apiUrl

    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify([{ studentId: userId }]) // Đảm bảo key là `studentId`
        });

        if (response.ok) {
            window.location.href = 'group-details.html';
            userIDInput.value = ''; // Xóa input sau khi thêm
            alert('Thêm thành viên thành công!');
        } else {
            const errorData = await response.json();
            console.error('Error:', errorData); // Log lỗi chi tiết
            alert(`Error: ${errorData.message || 'Unable to add member!'}`);
        }
    } catch (error) {
        console.error('Error adding member:', error);
        alert('Error connecting to the server.');
    }
}


// Nếu đang chỉnh sửa, tự động điền thông tin nhóm
if (check) {
    populateEditForm();
}
