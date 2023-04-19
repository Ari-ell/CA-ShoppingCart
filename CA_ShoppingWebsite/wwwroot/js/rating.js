

$(document).ready(function () {
    $('.star').on('click', function () {
        var rating = $(this).data('rating');
        var productId = $(this).closest('.star-rating').data('product-id');
        submitRating(productId, rating);
    });

    $('.star').on('mouseenter', function () {
        var rating = $(this).data('rating');
        $(this).parent().find('.star').removeClass('checked');
        $(this).parent().find('.star:lt(' + rating + ')').addClass('checked');
    });

    $('.star').on('mouseleave', function () {
        var rating = $(this).closest('.star-rating').find('.star.checked').length;
        $(this).parent().find('.star').removeClass('checked');
        $(this).parent().find('.star:lt(' + rating + ')').addClass('checked');
    });

    function submitRating(productId, rating) {
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

        xhr.send("productId=" + encodeURIComponent(productId) + "&rating=" + encodeURIComponent(rating));
        window.location.replace("/MyPurchases");
    }
});