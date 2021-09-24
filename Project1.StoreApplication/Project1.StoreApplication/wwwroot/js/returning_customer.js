const uri = 'api/customers';
let userType = sessionStorage.getItem('newOrReturning');
let userPrompt = document.getElementById("userPrompt")
let failureNotice = document.getElementById("failure_notice")
if (userType === 'new') userPrompt.innerHTML = "Enter a unique name to create your new account:"
else userPrompt.innerHTML = "Sign in:"
//will query DB to see if user exists. if so, method will return their id. Otherwise, will return -1.
//Id will then be placed in session storage and Customer dashboard will be loaded with appropriate data.
//For -1, user will be alerted to no bueno sign in.
//var customerExistsResponse;
function confirmExists() {
    const firstName = document.getElementById('add-first-name').value.trim();
    const lastName = document.getElementById('add-last-name').value.trim();

    fetch(`${uri}/firstName=${firstName}&lastName=${lastName}&userType=${userType}`, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            if (data >= 0) { sessionStorage.setItem('CustomerID', data); clearInputs(); window.location = "customer_home_page.html"; }
            if (data === -1) failureNotice.innerHTML = "That name has already been taken."
            if (data === -3) failureNotice.innerHTML = "We couldn't find you in the system."
            if (data === -2) failureNotice.innerHTML = "Each name can have a max of 50 charactes."
        });
}

function clearInputs() {
    document.getElementById('add-first-name').value="";
    document.getElementById('add-last-name').value="";
}

function addCustomer(firstName, lastName) {

    const Customer = {
        Id: 0,
        FirstName: firstName,
        LastName: lastName
    };

    fetch(`${uri}/why/firstName=${firstName}&lastName=${lastName}`, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            sessionStorage.setItem('CustomerID', data); console.log(data);
        });
        
    //fetch(uri, {
    //    method: 'POST',
    //    headers: {
    //        'Accept': 'application/json',
    //        'Content-Type': 'application/json'
    //    },
    //    body: JSON.stringify(Customer)
    //})
}

