
window.onload = function () {
    page_start();
}

function page_start() {

    add_click_listeners();
    
}

// listen to mouse-clicks
function add_click_listeners() {
    let loginBtn = document.getElementsByClassName("loginBtn");
    loginBtn[0].addEventListener('click', function onClick() {
        let username = document.getElementById("username");
        let password = document.getElementById("password");

        if (username.value == "" || password.value == "") {
            alert("Ensure you input a value in both fields!");
        }
        else {
            document.location.href = '/product'
            alert(`This form has a username of ${username.value} and password of ${password.value}`);
            
          
        }

            username.value = "";
            password.value = "";
    });

}

