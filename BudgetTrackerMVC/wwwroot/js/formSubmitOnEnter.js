document.addEventListener('DOMContentLoaded', (event) => {
    document.addEventListener('keypress', (event) => {
        if (event.key === 'Enter') {
            const activeElement = document.activeElement;
            const form = activeElement.closest('form');
            if (form) {
                event.preventDefault();
                form.submit();
            }
        }
    });
});
