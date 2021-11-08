const uri = 'api/orders/orderhistory'
let idType = sessionStorage.getItem('idType')
let id = 0
if (idType === 'location') id = sessionStorage.getItem('LocationID')
else id = sessionStorage.getItem('CustomerID')

fetch(`${uri}/idType=${idType}&id=${id}`)
    .then(response => response.json())
    .then(data => _displayOrders(data))

//function _displayCount(itemCount) {
//    const name = (itemCount === 1) ? 'order' : 'orders';

//    document.getElementById('counter').innerText = `${itemCount} ${name}`;
//}

function _displayOrders(data) {
    data.forEach(item => {
        console.log(item)
        let x = document.createElement("UL");
        x.setAttribute("id", "myUL");
        x.style.listStyleType = "none"
        document.body.appendChild(x);

        //let y = document.createElement("LI");
        //let t = document.createTextNode(item.orderDate);
        //y.appendChild(t);
        //document.getElementById("myUL").appendChild(y);
        //document.body.innerHTML += `<p>${item.orderDate}</p>`
        //document.body.innerHTML += `<p>${item.customerName}</p>`

        //document.body.innerHTML += `<ul style="list-style-type:none;">`
        //    document.body.innerHTML += `<li>${item.orderDate}</li>`
        //    document.body.innerHTML += `<li>${item.customerName}</li>`
        //    document.body.innerHTML += `<li>${item.locationName}</li>`
        //    document.body.innerHTML += `<li>${item.totalPrice}</li>`
        //    document.body.innerHTML += "<li>-------------------------------------------</li>"
        //    document.body.innerHTML += `<li>Order Items</li>`
        //for (let i = 0; i < item.orderItems.length; i++)
        //    document.body.innerHTML += `<li>${item.orderItems[i].name1}</li>`


        document.body.innerHTML += `<ul style="list-style-type:none;">`
            document.body.innerHTML += `<li>${item.orderDate}</li>`
            document.body.innerHTML += `<li>${item.customer.firstName} ${item.customer.lastName}</li>`
            document.body.innerHTML += `<li>${item.location.cityName}</li>`
            document.body.innerHTML += `<li>${item.totalPrice}</li>`
            document.body.innerHTML += "<li>-------------------------------------------</li>"
            document.body.innerHTML += `<li>Order Items</li>`
        for (let i = 0; i < item.orderItems.length; i++)
            document.body.innerHTML += `<li>${item.orderItems[i].product.name1}</li>`
        document.body.innerHTML += `</ul>`
        //console.log(item)
        





    })
}