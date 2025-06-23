// Handle sidebar toggle for mobile
document.addEventListener('DOMContentLoaded', function() {
    const toggleBtn = document.querySelector('.navbar-toggler');
    const sidebar = document.getElementById('sidebar');
    const closeBtn = document.querySelector('.btn-close');

    // Close sidebar when clicking outside of it
    document.addEventListener('click', function(event) {
        if (window.innerWidth <= 767) {
            const isClickInside = sidebar.contains(event.target) ||
                toggleBtn.contains(event.target);

            if (!isClickInside && sidebar.classList.contains('active')) {
                sidebar.classList.remove('active');
            }
        }
    });
});