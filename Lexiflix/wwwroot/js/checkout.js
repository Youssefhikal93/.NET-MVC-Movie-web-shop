
document.addEventListener('DOMContentLoaded', function () {
    // Only initialize checkout functionality if on checkout page
    if (document.getElementById('same-as-billing')) {
        initCheckout();
    }
});

function initCheckout() {
    // Initialize delivery address toggle
    const sameAsBilling = document.getElementById('same-as-billing');
    if (sameAsBilling) {
        sameAsBilling.addEventListener('change', toggleDeliveryAddress);
        toggleDeliveryAddress(); // Initialize state
    }

    // Initialize quantity buttons
    document.querySelectorAll('.increase-quantity').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            await updateCheckoutQuantity(movieId, 'increase');
        });
    });

    document.querySelectorAll('.decrease-quantity').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            await updateCheckoutQuantity(movieId, 'decrease');
        });
    });

    // Initialize remove buttons
    document.querySelectorAll('.remove-item').forEach(button => {
        button.addEventListener('click', async function () {
            const movieId = this.getAttribute('data-movie-id');
            await removeItem(movieId);
        });
    });
}

function toggleDeliveryAddress() {
    const sameAsBilling = document.getElementById('same-as-billing').checked;
    const deliveryFields = document.getElementById('delivery-address-fields');

    if (sameAsBilling) {
        deliveryFields.style.display = 'none';
        // Copy billing values to delivery fields
        document.getElementById('delivery-address').value = document.getElementById('billing-address').value;
        document.getElementById('delivery-city').value = document.getElementById('billing-city').value;
        document.getElementById('delivery-zip').value = document.getElementById('billing-zip').value;
    } else {
        deliveryFields.style.display = 'block';
    }
}

async function updateCheckoutQuantity(movieId, action) {
    try {
        const response = await fetch('/Order/UpdateQuantity', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ movieId: parseInt(movieId), action: action })
        });

        if (response.ok) {
            const result = await response.json();
            updateCheckoutDisplay(result);
            showNotification(action === 'increase' ? 'Quantity increased!' : 'Quantity decreased!');

            if (result.itemQuantity === 0) {
                location.reload();
            }
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Error updating quantity');
    }
}

async function removeItem(movieId) {
    try {
        const response = await fetch('/Order/RemoveFromCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ movieId: parseInt(movieId), removeAll: true })
        });

        if (response.ok) {
            const result = await response.json();
            updateCheckoutDisplay(result);
            showNotification('Item removed from cart!');
            location.reload();
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Error removing item');
    }
}

function updateCheckoutDisplay(result) {
    // Safely update elements if they exist
    const quantityDisplay = document.querySelector(`.quantity-display[data-movie-id="${result.movieId}"]`);
    if (quantityDisplay) quantityDisplay.textContent = result.itemQuantity;

    const subtotalElement = document.querySelector('.subtotal');
    const totalPriceElement = document.querySelector('.total-price');
    if (subtotalElement) subtotalElement.textContent = `Kr ${result.totalPrice.toFixed(2)}`;
    if (totalPriceElement) totalPriceElement.textContent = `Kr ${result.totalPrice.toFixed(2)}`;

    updateCartButton(result.totalItems, result.totalPrice);
}

function updateCartButton(totalItems, totalPrice) {
    const cartButton = document.querySelector('.cart-button');
    if (!cartButton) return;

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

async function checkCustomerEmail() {
    const email = document.getElementById('customer-email').value;
    if (!email) {
        showNotification('Please enter your email first');
        return;
    }

    const checkButton = document.querySelector('button[onclick="checkCustomerEmail()"]');
    const originalText = checkButton.innerHTML;
    checkButton.innerHTML = '<i class="bi bi-spinner spinner-border spinner-border-sm"></i> Checking...';
    checkButton.disabled = true;

    try {
        const response = await fetch('/Customer/CheckCustomerEmail?email=' + encodeURIComponent(email));
        if (response.ok) {
            const result = await response.json();
            handleCustomerResponse(result);
        } else {
            throw new Error('Failed to check customer email');
        }
    } catch (error) {
        console.error('Error checking email:', error);
        showNotification('Error checking customer information. Please try again.');
    } finally {
        checkButton.innerHTML = originalText;
        checkButton.disabled = false;
    }
}

function handleCustomerResponse(result) {
    const customerDetails = document.getElementById('customer-details');
    const existingMsg = document.getElementById('existing-customer-message');
    const newMsg = document.getElementById('new-customer-message');

    customerDetails.style.display = 'block';

    if (result.isExisting && result.customer) {
        // Existing customer
        existingMsg.style.display = 'block';
        newMsg.style.display = 'none';
        populateCustomerFields(result.customer);
        showNotification(`Welcome back ${result.customer.firstName || ''}! Your details have been loaded.`);
    } else {
        // New customer
        existingMsg.style.display = 'none';
        newMsg.style.display = 'block';
        clearCustomerFields();
        showNotification('New customer detected. Please fill in your details below.');
    }
}

function populateCustomerFields(customer) {
    document.getElementById('first-name').value = customer.firstName || '';
    document.getElementById('last-name').value = customer.lastName || '';
    document.getElementById('phone').value = customer.phone || '';
    document.getElementById('billing-address').value = customer.billingAddress || '';
    document.getElementById('billing-city').value = customer.billingCity || '';
    document.getElementById('billing-zip').value = customer.billingZip || '';
    document.getElementById('delivery-address').value = customer.deliveryAddress || '';
    document.getElementById('delivery-city').value = customer.deliveryCity || '';
    document.getElementById('delivery-zip').value = customer.deliveryZip || '';

    const sameAddress = customer.billingAddress === customer.deliveryAddress &&
        customer.billingCity === customer.deliveryCity &&
        customer.billingZip === customer.deliveryZip;
    document.getElementById('same-as-billing').checked = sameAddress;
    toggleDeliveryAddress();
}

function clearCustomerFields() {
    document.getElementById('first-name').value = '';
    document.getElementById('last-name').value = '';
    document.getElementById('phone').value = '';
    document.getElementById('billing-address').value = '';
    document.getElementById('billing-city').value = '';
    document.getElementById('billing-zip').value = '';
    document.getElementById('delivery-address').value = '';
    document.getElementById('delivery-city').value = '';
    document.getElementById('delivery-zip').value = '';
    document.getElementById('same-as-billing').checked = false;
    toggleDeliveryAddress();
}

