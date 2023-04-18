

window.onload = function () {
    page_start();

}

function page_start() {

    add_click_listeners();

}

// listen to mouse-clicks
function add_click_listeners() {

//for logout btn
    let logoutBtn = document.getElementsByClassName("logoutBtn");
    logoutBtn[0].addEventListener('click', on_logout_click);


    let addToCartBtn = document.getElementsByClassName("addToCartBtn");
    let searchBtn = document.getElementsByClassName("searchBtn");
    let searchInput = document.getElementsByClassName("searchInput");
    let cartBtn = document.getElementsByClassName("cartBtn");
    for (let i = 0; i < addToCartBtn.length; i++) {

        addToCartBtn[i].addEventListener('click', on_add_click);

    }
    searchBtn[0].addEventListener('click', on_search_click);
    cartBtn[0].addEventListener('click', on_myCart_click);
    searchInput[0].addEventListener('keypress', on_input);

}

async function on_logout_click(event) {
    //$('#myModal').modal('hide');
    await fetch('/logout', {
        method: 'GET',

    })
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.log(error));

    window.location.replace('/gallery')
}

function on_input(event) {
    if (event.key == 'Enter' || event.keyCode== 13) {
        redirect(event.target.value);
    }
   
}
function on_myCart_click(event) {

    window.location.replace('/cart');
 
}
function on_search_click(event) {
    let inputValue = document.getElementsByClassName("searchInput");
    let value = inputValue[0].value;
    redirect(value);
}
function redirect(value) {
    window.location.replace('/Gallery/Index?keyword='+value);
}
function on_add_click(event) {

    var ele = event.target;
    var value = ele["firstElementChild"].innerText;

    fetch('addToCart?' + new URLSearchParams({
        addProductId: value
    })).then(data => data.text())
        .then((text) => {
            console.log('request succeeded with JSON response', text)
            window.location.replace("/");

        }).catch(function (error) {
            console.log('request failed', error)
        });

}