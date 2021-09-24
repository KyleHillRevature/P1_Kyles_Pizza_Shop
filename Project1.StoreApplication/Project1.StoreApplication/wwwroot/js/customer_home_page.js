document.getElementById("viewPastOrders").addEventListener("click", function () { sessionStorage.setItem('idType', 'customer'); window.location = "view_past_orders.html" });

function chooseLocation() { sessionStorage.setItem('chooseLocationReason', 'view past orders'); window.location = "choose_location.html" }

function placeAnOrder() { sessionStorage.setItem('chooseLocationReason', 'place an order'); window.location = "choose_location.html" }
function signOut() { window.location = "../index.html" }