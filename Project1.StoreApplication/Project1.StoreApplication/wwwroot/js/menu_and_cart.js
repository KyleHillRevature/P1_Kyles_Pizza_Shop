let orderId = 0
let totalPriceValue = 0

window.onbeforeunload = function () {
    return false;
};

if (performance.navigation.type == performance.navigation.TYPE_RELOAD) fetch(`api/orders/`, {method: 'DELETE'})
 
//document.addEventListener('visibilitychange', function () {
//    if (document.visibilityState === 'hidden')
//        if (orderId !== 0) {
//            let r = confirm("This will get rid of your cart");
//            fetch(`api/orders/${orderId}`, {
//                method: 'DELETE'
//            })
//            document.getElementById("totalPrice").style.display = "none"; document.getElementById("cart").style.display = "none";
//            orderId = 0
                

//        }
//})
//to manage the cart we need the orderID, the chosen product, and chosen action

fetch('api/products')
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));

let locationId = sessionStorage.getItem('LocationID')
fetch(`api/locationInventories/${locationId}`)
    .then(response => response.json())
    .then(data => _addStockColumn(data))

function _displayItems(data) {

    const button = document.createElement('button');
    const table = document.getElementById('menuBody');

    data.forEach(item => {

        let addButton = button.cloneNode(false);
        addButton.innerText = '+';
        addButton.setAttribute('onclick', `orderUpdate(${item.id},'add')`);

        let removeButton = button.cloneNode(false);
        removeButton.innerText = '-';
        removeButton.setAttribute('onclick', `orderUpdate(${item.id},'remove')`);

        let tr = table.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.name1)
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.description1)
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.productPrice)
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(addButton);
        td4.appendChild(removeButton);

        let td5 = tr.insertCell(4);
        td5.style.display = "none"
        let textNode5 = document.createTextNode(item.id)
        td5.appendChild(textNode5);
    })
}



function _addStockColumn(data) {

    const table = document.getElementById('menu');
    const tableRows = table.rows
    let i = 0
    while (i < tableRows.length-1) {

        let td6 = tableRows[i+1].insertCell(5);
        let textNode6 = document.createTextNode(data[i].stock)
        td6.appendChild(textNode6);
        i++
    }

    
}

function orderUpdate(productId, action) {

    if (orderId === 0) {
        let order = { LocationId: sessionStorage.LocationID, CustomerId: sessionStorage.CustomerID, ProductId: productId, Action: action }
        fetch('api/orders', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(order)
        })
            .then(response => response.json())
            .then(data => displayCart(data))
    }
    else {
        let orderUpdate1 = { OrderId: orderId, ProductId: productId, Action: action }
        fetch('api/orders', {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(orderUpdate1)
        })
            .then(response => response.json())
            .then(data => displayCart(data))
    }
}

function displayCart(data) {

    if (data.actionSucceeded === false) window.alert(data.message)
    let totalPriceElement = document.getElementById("totalPrice")
    totalPriceValue = data.totalPrice
    //handles case of failing to add to an empty cart
    if (data.orderItems.length === 0) { totalPriceElement.style.display = "none"; document.getElementById("cart").style.display = "none"; return }
    //handles case of adding first item to cart
    if (orderId === 0) orderId = data.id
    let table = document.getElementById("cartBody")
    table.innerHTML = ''
    document.getElementById("totalPrice").innerHTML = data.totalPrice
    document.getElementById("cart").style.display = "block"; totalPriceElement.style.display = "block";
    data.orderItems.forEach(item => {

        let tr = table.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.name1)
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.quantity)
        td2.appendChild(textNode2);

    })
}
    


function submitOrder() {
    if (totalPriceValue === 0) { window.alert("Your cart is empty."); return }
    let order = { OrderId: orderId }
    fetch('api/orders/submitOrder', {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(order)
    })
    window.alert("Your order had been placed!")
    window.location = "customer_home_page.html"      
}



   


