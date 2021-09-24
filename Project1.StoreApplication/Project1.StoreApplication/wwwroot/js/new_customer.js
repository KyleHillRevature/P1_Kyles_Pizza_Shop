function addItem() {
    const addFirstNameTextbox = document.getElementById('add-first-name');
    const addLastNameTextbox = document.getElementById('add-last-name');

    const item = {
        firstName: addFirstNameTextbox.value.trim(),
        lastName: addLastNameTextbox.value.trim()
    };

    fetch('https://localhost:44319/api/customers', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
}