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

