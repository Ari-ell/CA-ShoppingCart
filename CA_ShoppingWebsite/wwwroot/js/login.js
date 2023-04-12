
window.onload = function () {
    page_start();
}

function page_start() {

    add_click_listeners();
    
}

// listen to mouse-clicks
  function add_click_listeners() {

    let loginBtn = document.getElementsByClassName("loginBtn");
      loginBtn[0].addEventListener('click', on_submit_click);   

}


function on_submit_click(event) {

    let username = document.getElementById("username");
    let password = document.getElementById("password");

    if (username.value == "" || password.value == "") {
        alert("Ensure you input a value in both fields!");
    }
    else {
        $.ajax({
            url: '/userlogin',
            type: 'GET',
            dataType: 'json',
            async: false,
            headers: {
                'username': username.value,
                'password': password.value
            },
            success: function (result) {
                flag = true;
                alert("Welcome back " + result.username);

            },
            error: function (error) {

                return alert(error.statusText)

            }
        });


    }

    console.log(flag);
    if (flag) {
        
        //location.href = "https://localhost:7052/gallery";
        window.location.replace("https://localhost:7052/gallery");

    }
}


function redirect() {
    location.href = "gallery";

}