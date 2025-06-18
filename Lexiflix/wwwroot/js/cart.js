
document.addEventListener('DOMContentLoaded', function () {
    // Only initialize cart functionality if on a page with cart elements
    if (document.querySelector('.add-to-cart')) {
        initCart();
    }
});

async function initCart() {
    await loadMovieQuantities();
    setupCartEventListeners();
}

async function loadMovieQuantities() {
    const movieCards = document.querySelectorAll('.add-to-cart');

    for (const button of movieCards) {
        const movieId = button.getAttribute('data-movie-id');
        try {
            const response = await fetch(`/Order/GetItemQuantity?movieId=${movieId}`);
            if (response.ok) {
                const result = await response.json();
                if (result.quantity > 0) {
                    updateMovieCard(movieId, result.quantity);
                }
            }
        } catch (error) {
            console.error('Error loading quantity for movie', movieId, error);
        }
    }
}

function setupCartEventListeners() {
    // Handle add to cart button clicks
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            const price = this.getAttribute('data-price');
            await addToCart(movieId, price);
        });
    });

    // Handle quantity buttons
    document.querySelectorAll('.increase-quantity, .decrease-quantity').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            const action = this.classList.contains('increase-quantity') ? 'increase' : 'decrease';
            await updateQuantity(movieId, action);
        });
    });
}

async function addToCart(movieId, price) {
    try {
        const response = await fetch('/Order/AddToCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ movieId: parseInt(movieId), price: parseFloat(price) })
        });

        if (response.ok) {
            const result = await response.json();
            updateCartUI(result.totalItems, result.totalPrice);
            updateMovieCard(movieId, result.itemQuantity);
            showNotification('Added to cart!');
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Error adding to cart');
    }
}

async function updateQuantity(movieId, action) {
    try {
        const response = await fetch('/Order/UpdateQuantity', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ movieId: parseInt(movieId), action: action })
        });

        if (response.ok) {
            const result = await response.json();
            updateCartUI(result.totalItems, result.totalPrice);
            updateMovieCard(movieId, result.itemQuantity);

            if (action === 'increase') {
                showNotification('Quantity increased!');
            } else if (result.itemQuantity === 0) {
                showNotification('Item removed from cart!');
            } else {
                showNotification('Quantity decreased!');
            }
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Error updating quantity');
    }
}

function updateMovieCard(movieId, quantity) {
    const movieCard = document.querySelector(`[data-movie-id="${movieId}"]`)?.closest('.card');
    if (!movieCard) return;

    const addButton = movieCard.querySelector('.add-to-cart');
    const quantityControls = movieCard.querySelector('.quantity-controls');
    const quantityInput = movieCard.querySelector('.quantity-input');

    if (quantity > 0) {
        addButton?.classList.add('d-none');
        quantityControls?.classList.remove('d-none');
        if (quantityInput) quantityInput.value = quantity;
    } else {
        addButton?.classList.remove('d-none');
        quantityControls?.classList.add('d-none');
        if (quantityInput) quantityInput.value = 1;
    }
}

function updateCartUI(totalItems, totalPrice) {
    const cartButton = document.querySelector('.cart-button');
    if (!cartButton) return;

    if (totalItems > 0) {
        cartButton.innerHTML = `
            <i class="bi bi-cart4 me-2"></i>
            <span class="badge bg-secondary rounded-pill me-1">${totalItems}</span>
            <span>${totalPrice.toFixed(2)} Kr</span>
        `;
    } else {
        cartButton.innerHTML = `
            <i class="bi bi-cart4 me-2"></i>Shopping Cart
        `;
    }
}