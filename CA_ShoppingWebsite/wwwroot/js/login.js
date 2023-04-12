
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
            $.ajax({
                url: '/userlogin',
                type: 'GET',
                dataType: 'json',
                async: false,
                headers: {
                    'username': username.value,
                    'password': password.value
                },
                contentType: 'application/json; charset=utf-8',
                success: function (result) {

                    window.location.href = '/gallery';
                },
                error: function (error) {
                    if (error.status != 200) {

                        alert(error.statusText);
                    }
                

                }
            });
           
          
        }
      
        
    });


}

//$(document).ajaxComplete(function (event, xhr, settings) {
//    console.log("ajaxComplete  ")
//    redirectHandle(xhr);
//})

//function redirectHandle(xhr) {

//    //获取后台返回的参数
//    var url = xhr.getResponseHeader("redirectUrl");
//    console.log("redirectUrl = " + url);
//    var enable = xhr.getResponseHeader("enableRedirect");

//    if ((enable == "true") && (url != "")) {
//        var win = window;
//        while (win != win.top) {
//            win = win.top;
//        }
//        win.location.href = url;
//    }
//}