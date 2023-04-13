
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
        redirect();
   

    }

}


function redirect() {


    //let xhr = new XMLHttpRequest();

    //xhr.open("GET", "/userlogin", true);
    //xhr.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    //xhr.setRequestHeader('username', username.value);
    //xhr.setRequestHeader('password', password.value);
    //xhr.send();
    //xhr.onload = () => {
    //    if (xhr.readyState == 4 && xhr.status == 200) {
    //        alert("Welcome back " + result.username);
    //        window.location = "/gallery";
    //    } else {
    //        console.log(`Error: ${xhr.status}`);
    //    }
    //};

    $.ajax({
        url: '/userlogin',
        type: 'GET',
        async: false,
        headers: {
            'username': username.value,
            'password': password.value
        },
        success: function (result) {
            flag = true;
            alert("Welcome back " + result.username);
            window.location.replace('/Gallery');

        },
        error: function (error) {

           alert(error.statusText);

        }
    });



}

$(document).ajaxComplete(function (event, xhr, settings) {
        var win = window;
        while (win != win.top) {
            win = win.top;
        }
        win.location.href = "/gallery";
  
});

