
// Lấy tham chiếu đến các phần tử HTML 
const groupNameInput = document.getElementById('groupname'); 
const projectTopicInput = document.getElementById('project-topic'); 
const memberListElement = document.getElementById('member__List'); 
const userIDInput = document.getElementById('user-id'); 
const check = localStorage.getItem('isEditing') === 'true';

// Mảng lưu thành viên 
let members = []; 

// Custom class cho các loại thông báo
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
});

// Hàm hiển thị thông báo thành công dạng toast
function showSuccessToast(message) {
    Toast.fire({
        icon: 'success',
        title: message
    });
}

// Hàm hiển thị thông báo lỗi dạng toast
function showErrorToast(message) {
    Toast.fire({
        icon: 'error',
        title: message
    });
}

// Hàm hiển thị thông báo xác nhận
async function showConfirmDialog(title, text) {
    return await Swal.fire({
        title: title,
        text: text,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy',
        heightAuto: false,
        customClass: {
            container: 'my-swal'
        }
    });
}

// Hàm hiển thị loading
function showLoading(message = 'Đang xử lý...') {
    Swal.fire({
        title: message,
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

// Kiểm tra trạng thái và hiển thị giao diện tương ứng 
document.getElementById('add group').style.display = check ? 'none' : 'block'; 
document.getElementById('edit group').style.display = check ? 'block' : 'none'; 

// Lưu nhóm mới hoặc chỉnh sửa nhóm 
async function saveGroup() { 
    const groupName = groupNameInput.value.trim(); 
    const projectTopic = projectTopicInput.value.trim(); 

    if (!groupName || !projectTopic) { 
        showErrorToast('Please enter the complete information!');
        return; 
    } 

    // Hiển thị dialog xác nhận
    const result = await showConfirmDialog(
        'Confirm',
        check ? 'Are you sure you want to update the group information?' : 'Are you sure you want to create a new group?'
    );
    
    if (!result.isConfirmed) {
        return;
    }

    const requestBody = { 
        groupName: groupName, 
        topic: projectTopic, 
        members: check ? members : []
    }; 

    const token = localStorage.getItem('accessToken'); 
    const apiUrl = check 
        ? `http://localhost:5076/api/group/edit-group`
        : `http://localhost:5076/api/group/new-group`; 

    try { 
        showLoading();

        const response = await fetch(apiUrl, { 
            method: 'POST', 
            headers: { 
                'Content-Type': 'application/json', 
                'Authorization': `Bearer ${token}` 
            }, 
            body: JSON.stringify(requestBody) 
        }); 

        Swal.close(); // Đóng loading

        if (response.ok) { 
            await Swal.fire({
                icon: 'success',
                title: check ? 'Edit successful!' : 'Group created successfully!',
                text: 'You will be redirected to the group details page..',
                timer: 2000,
                showConfirmButton: false
            });
            window.location.href = 'group-details.html';
        } else { 
            const errorData = await response.json(); 
            Swal.fire({
                icon: 'error',
                title: 'An error occurred!',
                text: errorData.message || 'Unable to perform the action!'
            });
        } 
    } catch (error) { 
        console.error(error); 
        Swal.fire({
            icon: 'error',
            title: 'Connection error.',
            text: 'Cannot connect to the server!'
        });
    } 
} 

// Hàm thêm thành viên vào nhóm 
async function addMember() { 
    const userId = userIDInput.value.trim();
    const token = localStorage.getItem('accessToken');
    const groupId = localStorage.getItem('selectedGroupId');

    if (!userId) { 
        showErrorToast('Please enter the userID you want to add to the group!');
        return; 
    } 

    // Hiển thị dialog xác nhận
    const result = await showConfirmDialog(
        'Add member.',
        `Are you sure you want to add the member with ID: ${userId} to the group?`
    );

    if (!result.isConfirmed) {
        return;
    }

    const apiUrl = `http://localhost:5076/api/group/add-member/${groupId}`;

    try { 
        showLoading('Adding member...');

        const response = await fetch(apiUrl, { 
            method: 'POST', 
            headers: { 
                'Content-Type': 'application/json', 
                'Authorization': `Bearer ${token}` 
            }, 
            body: JSON.stringify([{ studentId: userId }])
        }); 

        Swal.close(); // Đóng loading

        if (response.ok) { 
            userIDInput.value = '';
            await Swal.fire({
                icon: 'success',
                title: 'Member added successfully!',
                text: 'The page will reload to update the information.',
                timer: 2000,
                showConfirmButton: false
            });
            window.location.href = 'group-details.html';
        } else { 
            const errorData = await response.json(); 
            console.error('Error:', errorData);
            Swal.fire({
                icon: 'error',
                title: 'Không thể thêm thành viên',
                text: errorData.message || 'Có lỗi xảy ra khi thêm thành viên!'
            });
        } 
    } catch (error) { 
        console.error('Error adding member:', error); 
        Swal.fire({
            icon: 'error',
            title: 'Lỗi kết nối',
            text: 'Không thể kết nối đến máy chủ!'
        });
    } 
} 

// CSS để custom SweetAlert2 container
const style = document.createElement('style');
style.textContent = `
    .my-swal {
        z-index: 1400 !important;
    }
`;
document.head.appendChild(style);

// Nếu đang chỉnh sửa, tự động điền thông tin nhóm 
if (check) { 
    populateEditForm(); 
}