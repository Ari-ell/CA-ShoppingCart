window.onload = function () {
    page_start();

}

function page_start() {

    add_click_listeners();

}

// listen to mouse-clicks
function add_click_listeners() {

    let minus = document.getElementsByClassName("minusBtn");
   
    let plus = document.getElementsByClassName("plusBtn");

    for (let i = 0; i < minus.length; i++) {
        minus[i].addEventListener('click', on_minus_click);
        plus[i].addEventListener('click', on_plus_click);

    };
}



function on_plus_click(event) {
    let ele = event.currentTarget.parentNode;
    let qty = 0;
    qty = parseInt(ele.children[1].defaultValue) + 1;

    var productID = ele.children[2].innerText;
    fetchController(qty, productID);
    //int qty, string userID, string productID


}
function on_minus_click(event) {
    let ele = event.currentTarget.parentNode;
    let qty = 0;
    qty = parseInt(ele.children[1].defaultValue) - 1;
    var productID = ele.children[2].innerText;
    fetchController(qty, productID);

}
async function fetchController(qty, productID) {


        await fetch('editQty?' + new URLSearchParams({
            qty: qty,
            productID: productID

        })).then((text) => {
            console.log('request succeeded with JSON response', text)


        }).catch(function (error) {
            console.log('request failed', error)
        });


   window.location.replace("/cart");
}
