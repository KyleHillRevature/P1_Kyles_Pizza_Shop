window.onhashchange = function () { console.log("The page changed")}()
function returningUser() { sessionStorage.setItem('newOrReturning', 'returning'); window.location = "html/returning_customer.html" }
function newUser() { sessionStorage.setItem('newOrReturning', 'new'); window.location = "html/returning_customer.html"}

