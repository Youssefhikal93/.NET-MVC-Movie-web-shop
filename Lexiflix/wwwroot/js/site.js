
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

//notifications
//function showNotification(message) {
//    const notification = document.createElement('div');
//    notification.className = 'position-fixed top-0 end-1 mb-3 me-3 p-3 rounded shadow';
//    notification.style.zIndex = '1000';
//    notification.style.backgroundColor = '#2a2a2a'; 
//    notification.style.border = '1px solid #1a1a1a'; 
//    notification.style.color = '#ffffff'; 
//    notification.style.maxWidth = '400px';
//    notification.style.transition = 'opacity 0.3s ease';
//    notification.innerHTML = `<i class="bi bi-check-circle-fill me-2 text-success"></i>${message}`;

//    document.body.appendChild(notification);

//    // Fade in
//    setTimeout(() => {
//        notification.style.opacity = '1';
//    }, 10);

//    // Remove after delay
//    setTimeout(() => {
//        notification.style.opacity = '0';
//        setTimeout(() => notification.remove(), 300);
//    }, 2000);
//}
function showNotification(message) {
    const notification = document.createElement('div');
    notification.className = 'position-fixed start-50 translate-middle-x mt-2 p-2 rounded shadow text-center';
    notification.style.top = '0';
    notification.style.left = '50%';
    notification.style.transform = 'translateX(-50%)';
    notification.style.zIndex = '1000';
    notification.style.backgroundColor = '#f8f9fa'; 
    notification.style.border = '1px solid #dee2e6';
    notification.style.color = '#212529';
    notification.style.maxWidth = '600px'; 
    notification.style.minWidth = '200px';
    notification.style.boxShadow = '0 4px 16px rgba(0,0,0,0.10)';
    notification.style.transition = 'opacity 0.3s ease';
    notification.style.opacity = '0';
    notification.innerHTML = `<i class="bi bi-check-circle-fill me-2 text-success"></i>${message}`;

    document.body.appendChild(notification);

    // Fade in
    setTimeout(() => {
        notification.style.opacity = '1';
    }, 10);

    // Remove after delay
    setTimeout(() => {
        notification.style.opacity = '0';
        setTimeout(() => notification.remove(), 300);
    }, 2000);
}
// Global cart management
document.addEventListener('DOMContentLoaded', function () {
    // Load cart summary on page load
    loadCartSummary();
});

async function loadCartSummary() {
    try {
        const response = await fetch('/Order/GetCartSummary');
        if (response.ok) {
            const result = await response.json();
            updateCartUI(result.totalItems, result.totalPrice);
        }
    } catch (error) {
        console.error('Error loading cart summary:', error);
    }
}


function scrollToContent() {
    document.getElementById('main-content').scrollIntoView({
        behavior: 'smooth'
    });
}