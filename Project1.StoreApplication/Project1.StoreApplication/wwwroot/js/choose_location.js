fetch('api/locations')
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));

function _displayItems(data)
{
    const button = document.createElement('button');
    const listItem = document.createElement('LI')
    const locationList = document.getElementById('locationList');

    data.forEach(item => {

        let listItem1 = listItem.cloneNode(false);
        let button1 = button.cloneNode(false);
        button1.innerText = `${item.cityName}`;
        //button1.addEventListener("click", menuAndCart(item.id))
        button1.setAttribute('onclick', `branchToNextPage(${item.id})`);
        listItem1.appendChild(button1)
        locationList.appendChild(listItem1)
    })
}

function branchToNextPage(locationId) {
    sessionStorage.setItem('LocationID', locationId);
    if (sessionStorage.getItem('chooseLocationReason') === 'view past orders'){ sessionStorage.setItem('idType', 'location'); window.location = "view_past_orders.html"; }
    else window.location = "menu_and_cart.html";
}
