const searchModal = document.getElementById('searchModal');
const openSearchBtn = document.getElementById('openSearch');
const searchInput = document.getElementById('searchInputModal');
const searchResults = document.getElementById('searchResults');
const controller = new AbortController();
// Event Listeners
openSearchBtn.addEventListener('click', (e) => {
    e.preventDefault();
    searchModal.style.display = 'flex';
    searchInput.focus();
});

searchModal.addEventListener('click', (e) => {
    if (e.target === searchModal) {
        searchModal.style.display = 'none';
    }
});

document.addEventListener('keydown', (e) => {
    if (e.key === "Escape") {
        searchModal.style.display = 'none';
    }
});

searchInput.addEventListener('input', async function () {
    const query = this.value.trim();
    searchResults.innerHTML = ''; 

    
    if (query.length >= 3) {
        try {
            const movies = await fetchSearchResults(query);
            displaySearchResults(movies);
        } catch (error) {
            console.error("Search error:", error);
            searchResults.innerHTML = "<p class='text-danger'>Error loading results.</p>";
        }
    }
});

// Async function to fetch search results
async function fetchSearchResults(query) {
   
    const response = await fetch(`/Movie/Search?query=${encodeURIComponent(query)}`);
    if (!response.ok) {
        throw new Error('response was not ok');
    }
    return await response.json();
}

// Function to display search results
function displaySearchResults(movies) {
    if (movies.length === 0) {
        searchResults.innerHTML = "<p class='text-light'>No results found.</p>";
        return;
    }

    movies.forEach(movie => {
        const resultItem = document.createElement('div');
        resultItem.classList.add('d-flex', 'align-items-center', 'gap-3', 'mb-2', 'p-2', 'search-result-item');

        resultItem.innerHTML = `
            <a href="/Movie/Details/${movie.id}" class="text-decoration-none text-light d-flex align-items-center gap-3 w-100">
                <img src="${movie.posterUrl ?? 'N/A'}" 
                     alt="${movie.title}" 
                     class="search-poster">
                <span class="fw-semibold">${movie.title}</span>
            </a>
        `;
        searchResults.appendChild(resultItem);
    });
}