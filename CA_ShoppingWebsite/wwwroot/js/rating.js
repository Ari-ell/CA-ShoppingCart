window.onload = function () {

    add_click_listener();

}



function add_click_listener() {
    // Get all the radio buttons
    var radioButtons = document.getElementsByName('star');
    
    // Loop through all the radio buttons
    for (var i = 0; i < radioButtons.length; i++) {

        // Add an event listener to each radio button
        radioButtons[i].addEventListener('click', function () {
            var productId = this.getAttribute('data-product-id')
            var ratingValue = this.value;// Get the selected rating value
            submitRating(productId,ratingValue);
        })
    }
}

function submitRating(productId,ratingValue) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/MyPurchases/SetUserRating");
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status != 200) {
                return;
            }
        }
    }

    xhr.send("productId=" + encodeURIComponent(productId) + "&rating=" + encodeURIComponent(ratingValue));
}