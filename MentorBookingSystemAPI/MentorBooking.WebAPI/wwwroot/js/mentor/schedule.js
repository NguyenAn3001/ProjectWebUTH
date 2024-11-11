function toggleAvailability(button) {
    if (button.textContent === 'Free time') {
        button.textContent = 'Busy';
        button.style.backgroundColor = 'red';
        button.style.color = 'white';
    } else {
        button.textContent = 'Free time';
        button.style.backgroundColor = 'initial';
        button.style.color = 'initial';
    }
}