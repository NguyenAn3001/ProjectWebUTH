function handleSearch(event) {
    event.preventDefault();
    
    const searchText = document.getElementById('searchInput').value.trim();
    
    if (searchText) {
        sessionStorage.setItem('lastSearchText', searchText);
        window.location.href = `../../../views/student/search/search-and-filter.html?searchText=${encodeURIComponent(searchText)}`;//`/views/student/search/search-and-filter.html?searchText=${encodeURIComponent(searchText)}`;
    }
}

document.getElementById('search-form').addEventListener('submit', handleSearch);