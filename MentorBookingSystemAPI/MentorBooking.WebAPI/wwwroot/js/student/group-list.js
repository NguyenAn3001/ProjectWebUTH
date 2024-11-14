// Dữ liệu nhóm mẫu
const groups = [
    { id: 1, name: "Group 1", members: ["Alice", "Bob", "Charlie"] },
    { id: 2, name: "Group 2", members: ["David", "Eve", "Frank"] },
    { id: 3, name: "Group 3", members: ["Grace", "Heidi", "Ivan"] },
];

// Hàm để hiển thị danh sách nhóm
function displayGroups() {
    const groupListContainer = document.getElementById("groupListContainer");
    groupListContainer.innerHTML = ''; // Xóa nội dung cũ

    groups.forEach(group => {
        const groupDiv = document.createElement("div");
        groupDiv.classList.add("group__info");
        groupDiv.style.marginBottom = "1.5rem";

        // Tên nhóm
        const groupName = document.createElement("p");
        groupName.innerHTML = `<strong>Group Name:</strong> ${group.name}`;
        groupDiv.appendChild(groupName);

        // Danh sách thành viên
        const memberList = document.createElement("p");
        memberList.innerHTML = "<strong>Members:</strong>";
        
        group.members.forEach(member => {
            const memberItem = document.createElement("span");
            memberItem.textContent = member;
            memberItem.style.display = "inline-block";
            memberItem.style.marginRight = "0.5rem";
            memberList.appendChild(memberItem);
        });

        groupDiv.appendChild(memberList);

        // Nút Sửa
        const editButton = document.createElement("button");
        editButton.textContent = "Edit";
        editButton.onclick = () => editGroup(group.id);
        groupDiv.appendChild(editButton);

        groupListContainer.appendChild(groupDiv);
    });
}

// Hàm chuyển tới trang tạo nhóm với dữ liệu nhóm cần chỉnh sửa
function editGroup(groupId) {
    const group = groups.find(g => g.id === groupId);
    if (group) {
        // Lưu dữ liệu nhóm vào localStorage để chuyển sang trang "Create Group"
        localStorage.setItem("editingGroup", JSON.stringify(group));
        window.location.href = "create-group.html"; // Điều hướng đến trang tạo nhóm
    }
}

// Gọi hàm để hiển thị nhóm khi tải trang
document.addEventListener("DOMContentLoaded", displayGroups);
