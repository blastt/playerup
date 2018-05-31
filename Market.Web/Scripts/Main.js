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
                    div.text(count);
                    a.prepend(div);
                }
                div.text(count);
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

//function SetDialogsClickable() {
//    $(".clickable").each(function () {
//        $(this).on('click', function (e) {
//            var id = $(this).data('id');
//            location.href = "/account/dialog/" + id;
//        });
//    });
//}



//function SetDeleteDialogClickable() {
//    $("#messages #linkdelete").each(function () {

//        $(this).on('click', function (e) {
//            var obj = $(this); // first store $(this) in obj
//            var id = $(this).data('id'); // get id of data using this 
//            $.ajax({
//                url: "/Message/DeleteAjax",
//                data: { id: id },
//                //cache: false,
//                //contentType: false,
//                //processData: false,
//                //mimeType: "multipart/form-data",
//                type: "Post",
//                dataType: "Json",
//                success: function (result) {
//                    if (result.Success) {
//                        $(obj).closest("tr").hide('slow', function () { $(obj).closest("tr").remove(); }); // You can remove row like this
//                    }
//                    eval(result.Script);
//                },
//                error: function () {
//                    alert("خطا!");
//                }
//            });
//        });
//    });
//}



function drowMessage(count) {

    if (count != 0) {
        var a = $('#messagess').find('a');
        var div = $('#top-counter');
        if (!div.length) {
            div = $('<div></div>').attr('id', 'top-counter');
            div.text(count);
            a.prepend(div);
        }
        div.text(count);
        div.addClass('messagess-count message-count-icon-pos');

        

    }
    else {
        var li = $('.message-count-icon-pos').remove();
    }
}

function drowMessageInDialog(userName, companionId,companionName,count, lastMessage, newDate, dialogId) {
    var trId = '#message-row-' + dialogId;

    var tr = $(trId);
    tr.addClass('warning-row');
    var tdName = tr.find('#dialog-name');
    var tdText = tr.find('#dialog-text').attr('data-id',dialogId);
    var tdDate = tr.find('#dialog-date').attr('data-id', dialogId);
    var compImg = tr.find('#dialog-block-img').attr('data-id', dialogId);
    var compName = tr.find('#dialog-companion-name').attr('data-id', dialogId);
    //Html.Action("Photo", "Profile", new { UserId'+ companionId +' = message.SenderId })
    compImg.attr('src', 'Html.Action("Photo", "Profile", new { UserId =' + companionId +'})');
    compName.text(companionName);
    tdText.text(lastMessage);
    tdDate.text(newDate);

    var div = tr.find('#dialog-block-counter');
    if (!div.length) {
        div = $('<div></div>').attr('id', 'dialog-block-counter');
        div.addClass('messagess-count message-count-block-pos');
        div.text(count);
        tdName.prepend(div);
    }
    div.text(count);
    

    




}