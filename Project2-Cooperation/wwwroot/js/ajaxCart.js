//ajax cart
var ajaxAddToCart = (prodId) => {
    $.ajax({
        url: '/api/cart',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: prodId
    }).done((prod) => {
        //console.log(product);
        //on success ...
        document.getElementById('cartItems').innerHTML = "";
        ajaxGetCart();
        popUpCart();
    });
};

var ajaxDeleteFromCart = (prodId) => {
    $.ajax({
        url: '/api/cart',
        type: 'DELETE',
        contentType: 'application/json',
        dataType: 'json',
        data: prodId
    }).done((prod) => {
        //on success ...
        document.getElementById('cartItems').innerHTML = "";
        ajaxGetCart();
        popUpCart();
    });
};

var popUpCart = () => {
    var toggleLayout = document.getElementById('cartNav');
    var toggleContent = document.getElementById('cartItems');
    toggleLayout.className += ' show';
    toggleContent.className += ' show';
};

var ajaxGetCart = () => {
    $.ajax({
        url: 'api/cart',
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
    }).done((cart) => {
        //on success ...
        var total = 0.00;
        cart.cartItems.forEach((item) => {
            cartHtml(item);
            total += item.product.salePrice * item.quantity;
        });
        if (total != 0) {
            addButtons();
        }
        else {
            emptyCart();
        }
        document.getElementById('cartCount').innerHTML = total.toFixed(2) + " &euro;";
    });
};

var emptyCart = () => {
    var buttons = document.createElement('li');

    var buttonsa1 = document.createElement('a');
    buttonsa1.className = 'text-center p-1';
    buttons.appendChild(buttonsa1);

    var buttonsa1h6 = document.createElement('h6');
    buttonsa1h6.innerHTML = 'Cart is empty';
    buttonsa1.appendChild(buttonsa1h6);

    var layout = document.getElementById('cartItems');
    layout.appendChild(buttons);
};

var addButtons = () => {
    var divider = document.createElement('li');
    divider.className = 'dropdown-divider';

    var buttons = document.createElement('li');

    var buttonsa1 = document.createElement('a');
    buttonsa1.className = 'text-center pull-left pt-2 pb-1 ml-3 btn btn-xs btn-secondary';
    buttonsa1.setAttribute('href', '/cart');
    buttons.appendChild(buttonsa1);

    var buttonsa1h6 = document.createElement('h6');
    buttonsa1h6.innerHTML = 'View Cart';
    buttonsa1.appendChild(buttonsa1h6);

    var buttonsa2 = document.createElement('a');
    buttonsa2.className = 'text-center pull-right pt-2 pb-1 mr-3 btn btn-xs btn-primary';
    buttonsa2.setAttribute('href', '/order/checkout');
    buttons.appendChild(buttonsa2);

    var buttonsa2h6 = document.createElement('h6');
    buttonsa2h6.innerHTML = 'Check Out';
    buttonsa2.appendChild(buttonsa2h6);

    var clearfix = document.createElement('div');
    clearfix.className = 'clearfix';

    var layout = document.getElementById('cartItems');
    layout.appendChild(divider);
    layout.appendChild(buttons);
    layout.appendChild(clearfix);
};

var cartHtml = (item, productId) => {

    var cartItem = document.createElement('li');
    cartItems.appendChild(cartItem);

    var cartItemSpan = document.createElement('span');
    cartItemSpan.className = 'item';
    cartItem.appendChild(cartItemSpan);

    var cartItemSpanLeft = document.createElement('span');
    cartItemSpanLeft.className = 'item-left';
    cartItemSpan.appendChild(cartItemSpanLeft);

    var cartItemSpanRight = document.createElement('span');
    cartItemSpanRight.className = 'item-right remove-from-cart';
    cartItemSpanRight.addEventListener('click', function (e) {
        var productId = e.target.parentElement.getAttribute('product-id');
        ajaxDeleteFromCart(productId);
    });
    cartItemSpan.appendChild(cartItemSpanRight);

    var cartItemRemoveHtml = document.createElement('a');
    cartItemRemoveHtml.className = 'btn btn-xs pull-right';
    cartItemRemoveHtml.setAttribute('product-id', item.productId);
    cartItemSpanRight.appendChild(cartItemRemoveHtml);

    var cartItemRemoveIcon = document.createElement('i');
    cartItemRemoveIcon.className = 'fa fa-close';
    cartItemRemoveHtml.appendChild(cartItemRemoveIcon);

    var cartItemQuantHtml = document.createElement('a');
    cartItemQuantHtml.className = 'pull-right mr-4';
    cartItemQuantHtml.setAttribute('product-id', item.productId);
    cartItemSpanRight.appendChild(cartItemQuantHtml);

    var cartItemQuantIcon = document.createElement('i');
    cartItemQuantIcon.innerHTML = 'x' + item.quantity;
    cartItemQuantHtml.appendChild(cartItemQuantIcon);

    var cartItemImage = document.createElement('img');
    cartItemImage.className = 'cart-img';
    cartItemImage.setAttribute('src', item.product.imageUrl);
    cartItemSpanLeft.appendChild(cartItemImage);

    var cartItemTitleSpan = document.createElement('span');
    cartItemTitleSpan.className = 'item-info';
    cartItemSpanLeft.appendChild(cartItemTitleSpan);

    var cartItemTitleHtml = document.createElement('span');
    cartItemTitleHtml.innerHTML = item.product.title;
    cartItemTitleSpan.appendChild(cartItemTitleHtml);

    var cartItemPriceHtml = document.createElement('span');
    cartItemPriceHtml.innerHTML = item.product.salePrice + " &euro;";
    cartItemTitleSpan.appendChild(cartItemPriceHtml);

    document.getElementById('cartItems').appendChild(cartItem);
};

var eventlist = document.querySelectorAll('.add-to-cart');
eventlist.forEach((elem) => {
    elem.addEventListener('click', (e) => {
        var prodId = e.target.parentElement.getAttribute('id');
        ajaxAddToCart(prodId);
    });
});

ajaxGetCart();