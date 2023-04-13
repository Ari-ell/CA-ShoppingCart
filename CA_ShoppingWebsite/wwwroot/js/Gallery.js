

window.onload = function () {
    page_start();

}

function page_start() {

    add_click_listeners();

}

// listen to mouse-clicks
function add_click_listeners() {

    let addToCartBtn = document.getElementsByClassName("addToCartBtn");
    let searchBtn = document.getElementsByClassName("searchBtn");
    let searchInput = document.getElementsByClassName("searchInput");
    let cartBtn = document.getElementsByClassName("cartBtn");
    for (let i = 0; i < addToCartBtn.length; i++) {

        addToCartBtn[i].addEventListener('click', on_add_click);

    }
    searchBtn[0].addEventListener('click', on_search_click);
    cartBtn[0].addEventListener('click', on_myCart_click)

}

function on_myCart_click(event) {
    alert("My Cart");
}
function on_search_click(event) {
    alert("Search");
}
function on_add_click(event) {
    alert("add to cart");
}