//// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.
//document.addEventListener('DOMContentLoaded', function () {

//    // Handle add to cart button clicks
//    document.querySelectorAll('.add-to-cart').forEach(button => {
//        button.addEventListener('click', async function () {
//            const movieId = this.getAttribute('data-movie-id');
//            const price = this.getAttribute('data-price');

//            try {
//                const response = await fetch('/Order/AddToCart', {
//                    method: 'POST',
//                    headers: {
//                        'Content-Type': 'application/json',
//                    },
//                    body: JSON.stringify({
//                        movieId: parseInt(movieId),
//                        price: parseFloat(price)
//                    })
//                });

//                if (response.ok) {
//                    const result = await response.json();
//                    updateCartUI(result.totalItems, result.totalPrice);

//                    // Show a quick notification
//                    const notification = document.createElement('div');
//                    notification.className = 'position-fixed bottom-0 end-0 mb-3 me-3 p-2 bg-warning text-dark rounded shadow';
//                    notification.style.zIndex = '1000';
//                    notification.innerHTML = '<i class="bi bi-check-circle-fill me-1"></i> Added to cart!';
//                    document.body.appendChild(notification);

//                    setTimeout(() => {
//                        notification.remove();
//                    }, 2000);
//                }
//            } catch (error) {
//                console.error('Error:', error);
//            }
//        });
//    });

//    // Function to update cart UI
//    function updateCartUI(totalItems, totalPrice) {
//        const cartButton = document.querySelector('.cart-button');
//        if (cartButton) {
//            cartButton.innerHTML = `
//                    <i class="bi bi-cart4 me-2"></i>
//                    ${totalItems > 0 ? `<span class="badge bg-danger rounded-pill">${totalItems}</span>` : ''}
//                    ${totalPrice > 0 ? `<span class="ms-1">${totalPrice} Kr</span>` : ''}
//                `;
//        }
//    }
//});
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    // Load initial quantities for all movie cards
    loadMovieQuantities();

    // Handle add to cart button clicks
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            const price = this.getAttribute('data-price');

            try {
                const response = await fetch('/Order/AddToCart', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        movieId: parseInt(movieId),
                        price: parseFloat(price)
                    })
                });

                if (response.ok) {
                    const result = await response.json();
                    updateCartUI(result.totalItems, result.totalPrice);
                    updateMovieCard(movieId, result.itemQuantity);
                    showNotification('Added to cart!');
                }
            } catch (error) {
                console.error('Error:', error);
            }
        });
    });

    // Handle quantity increase buttons
    document.querySelectorAll('.increase-quantity').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            await updateQuantity(movieId, 'increase');
        });
    });

    // Handle quantity decrease buttons
    document.querySelectorAll('.decrease-quantity').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            await updateQuantity(movieId, 'decrease');
        });
    });
});

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

async function updateQuantity(movieId, action) {
    try {
        const response = await fetch('/Order/UpdateQuantity', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                movieId: parseInt(movieId),
                action: action
            })
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
    }
}

function updateMovieCard(movieId, quantity) {
    const movieCard = document.querySelector(`[data-movie-id="${movieId}"]`).closest('.card');
    const addButton = movieCard.querySelector('.add-to-cart');
    const quantityControls = movieCard.querySelector('.quantity-controls');
    const quantityInput = movieCard.querySelector('.quantity-input');

    if (quantity > 0) {
        // Hide add button, show quantity controls
        addButton.classList.add('d-none');
        quantityControls.classList.remove('d-none');
        quantityInput.value = quantity;
    } else {
        // Show add button, hide quantity controls
        addButton.classList.remove('d-none');
        quantityControls.classList.add('d-none');
        quantityInput.value = 1;
    }
}

function updateCartUI(totalItems, totalPrice) {
    const cartButton = document.querySelector('.cart-button');
    if (cartButton) {
        if (totalItems > 0) {
            cartButton.innerHTML = `
                <i class="bi bi-cart4 me-2"></i>
                <span class="badge bg-danger rounded-pill me-1">${totalItems}</span>
                <span>${totalPrice.toFixed(2)} Kr</span>
            `;
        } else {
            cartButton.innerHTML = `
                <i class="bi bi-cart4 me-2"></i>Shopping Cart
            `;
        }
    }
}

function showNotification(message) {
    const notification = document.createElement('div');
    notification.className = 'position-fixed bottom-0 end-0 mb-3 me-3 p-2 bg-warning text-dark rounded shadow';
    notification.style.zIndex = '1000';
    notification.innerHTML = `<i class="bi bi-check-circle-fill me-1"></i> ${message}`;
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 2000);
}