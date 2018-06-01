$(document).ready(function () {
  
    $.ajax({
        url: '/Order/GetOrdersCount',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(),
        dataType: "json",
        success: function (response) {
            if (response != 0) {
                var a = $('#orders').find('a');
                var div = $('.orders-count');
                if (!div.length) {
                    div = $('<div></div>').attr('id', 'top-counter');
                    div.addClass('orders-count');
                    a.prepend(div);
                }
                div.text(response);
                
                a.prepend(div);
            }
        }
    });

    $.ajax({
        url: '/Message/GetMessagessCount',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(),
        dataType: "json",
        success: function (response) {
            drowMessage(response);
        }
    });


})



