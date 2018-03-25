$(document).ready(function () {
    $.ajax({
        url: '/Order/GetOrdersCount',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(),
        dataType: "json",
        success: function (response) {
            if (response != 0) {
                var li = $('#orders');
                var div = $('<div></div>');
                div.text(response);
                div.addClass('orders-count');
                li.prepend(div);
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
            if (response != 0) {
                var li = $('#messagess');
                var div = $('<div></div>');
                div.text(response);
                div.addClass('messagess-count');
                li.prepend(div);
            }
        }
    });
})
$(document).ready(function () {
    //$(".nav-item a").each(function () {

    //    if (this.href == window.location.href) {
    //        $(this.parentNode).addClass("current");
    //    }
    //});
    $("#account-list a").each(function () {

        if (this.href.split('/', 4)[3] == window.location.href.split('/', 4)[3]) {
            $(this.parentNode).addClass("active");
        }
    });
    $("#account-inner-list a").each(function () {
        if (this.href == window.location.href) {

            $(this).addClass("active");
        }
    });

});