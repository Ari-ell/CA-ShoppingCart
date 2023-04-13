

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
    cartBtn[0].addEventListener('click', on_myCart_click);
    searchInput[0].addEventListener('input', on_input);

}

function on_input(event) {
    if (event.key == 'Enter' || event.keyCode== 13) {
        redirect(event.target.value);
    }
   
}
function on_myCart_click(event) {
    console.log(event.target.value);
 
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

    alert("add to cart");
}