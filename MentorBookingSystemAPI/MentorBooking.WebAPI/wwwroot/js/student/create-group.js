const addButton = document.querySelector('.btn-hover');
      const deleteButton = document.querySelector(".btn-delete");
      const groupNameInput = document.getElementById('groupname');
      const memberEmailInput = document.getElementById('email');
      const displayGroupName = document.getElementById('displayGroupName');
      const memberList = document.getElementById('memberList');
    
      addButton.addEventListener('click', function (e) {
          e.preventDefault();
    
          const groupName = groupNameInput.value;
          const memberEmail = memberEmailInput.value;
    
          if (groupName) {
              displayGroupName.textContent = groupName;
          }
    
          if (memberEmail) {
              const listItem = document.createElement('li');
              listItem.textContent = memberEmail;
              listItem.setAttribute('data-email', memberEmail); // Gán email vào thuộc tính data-email
              memberList.appendChild(listItem);
    
              memberEmailInput.value = '';
          }
      });
    
      function deleteMemberByEmail() {
        const emailToDelete = memberEmailInput.value.trim(); // Lấy email từ ô nhập
        const members = memberList.querySelectorAll("li");
    
        let memberFound = false;
    
        members.forEach(member => {
          if (member.getAttribute("data-email") === emailToDelete) { // Kiểm tra email trong thuộc tính data-email
            memberList.removeChild(member);
            memberFound = true;
          }
        });
    
        if (!memberFound) {
          alert("Không tìm thấy thành viên này.");
        } else {
          memberEmailInput.value = ""; // Xóa email đã nhập sau khi xóa thành viên
        }
      }
    
      // Gán sự kiện click cho nút "Delete member"
      deleteButton.addEventListener("click", deleteMemberByEmail);
      const profileButton = document.querySelector('.action .profile');
      const menu = document.querySelector('.action .menu');

      profileButton.addEventListener('click', (event) => {
        event.stopPropagation(); 
        menu.classList.toggle('active');
      });
      document.addEventListener('click', (event) => {
        if (!menu.contains(event.target) && !profileButton.contains(event.target)) {
          menu.classList.remove('active');
        }
      });