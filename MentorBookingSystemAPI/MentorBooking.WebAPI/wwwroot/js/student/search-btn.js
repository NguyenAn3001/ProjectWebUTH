function handleSearch(event) {
    event.preventDefault();
    
    const searchText = document.getElementById('searchInput').value.trim();
    
    if (searchText) {
        // Lưu từ khóa tìm kiếm vào sessionStorage để duy trì khi load lại trang
        sessionStorage.setItem('lastSearchText', searchText);
        window.location.href = `../../../views/student/search/search-and-filter.html?searchText=${encodeURIComponent(searchText)}`;//`/views/student/search/search-and-filter.html?searchText=${encodeURIComponent(searchText)}`;
    }
}

// Thêm event listener cho form
document.getElementById('search-form').addEventListener('submit', handleSearch);