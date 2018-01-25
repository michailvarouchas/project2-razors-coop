//==============
//THE RAZORS JS (Eshop page)
//ADMIN show order details
$('.viewOrder').click(function (e) {
    e.preventDefault();
    var selectedOrder = $(this).attr('id');
    var modalToTrigger = "#modal_" + selectedOrder;
    $(modalToTrigger).modal('show');
});


//EDIT PRODUCT IMAGE PREVIEW
$('#imageInput').change(function (event) {
    var tmppath = URL.createObjectURL(event.target.files[0]);
    $('#imagePreview').fadeIn("fast").attr('src', tmppath);
});

//WISHLIST API
var ajaxAddToWishList = (productId) => {
    $.ajax({
        url: '/api/wishlist',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: productId
    }).done((prod) => {
        //console.log(product);
        //on success ...
    });
};

var addToWishListList = document.querySelectorAll('.add-to-wishlist');
addToWishListList.forEach((elem) => {
    elem.addEventListener('click', (e) => {
        var prodId = e.target.parentElement.getAttribute('w-id');
        ajaxAddToWishList(prodId);
    });
});