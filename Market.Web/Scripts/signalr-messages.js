﻿function drowMessage(count) {

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

function drowMessageInDialog(userName, companionId, companionName, count, lastMessage, newDate, dialogId) {
    
    var trId = '#message-row-' + dialogId;

    var tr = $(trId);
    tr.addClass('warning-row');
    var tdName = tr.find('#dialog-name');
    var tdText = tr.find('#dialog-text').attr('data-id', dialogId);
    var tdDate = tr.find('#dialog-date').attr('data-id', dialogId);
    var compImg = tr.find('#dialog-block-img').attr('data-id', dialogId);
    var compName = tr.find('#dialog-companion-name').attr('data-id', dialogId);
    //Html.Action("Photo", "Profile", new { UserId'+ companionId +' = message.SenderId })
    compImg.attr('src', 'Html.Action("Photo", "Profile", new { UserId =' + companionId + '})');
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

$(function () {

    var messageHub = $.connection.messageHub;

    messageHub.client.updateMessage = function (counter, name) {

        drowMessage(counter);

    };

    messageHub.client.updateMessageInDialog = function (userName, companionId, companionName, counter, lastMessage, date, dialogId) {
        drowMessageInDialog(userName, companionId, companionName, counter, lastMessage, date, dialogId);
        $(".clickable").each(function () {
            $(this).on('click', function (e) {
                var id = $(this).data('id');
                location.href = "/dialog/details?dialogId=" + id;
            });
        });
        //SetDeleteDialogClickable();
    };

    messageHub.client.addMessage = function (receiverName, senderName, messageBody, date, senderImage) {

        var messagesContainer = $('#messages-col');
        var messageBlock = $('#message-block-element').last().clone();

        messageBlock.find('#message-block-img').attr('src', senderImage);
        messageBlock.find('#sender-name').text(senderName);
        messageBlock.find('#created-date').text(date);
        messageBlock.find('#message-body').text(messageBody);
        messageBlock.removeAttr('style');
        messagesContainer.append(messageBlock);

        $('#messageBody').val('');
        var objDiv = $('#messages-col');
        objDiv.scrollTop(objDiv[0].scrollHeight);


    };

    messageHub.client.addDialog = function (companionId, companionName, dialogId) {

        var dialogsTable = $('#dialogs-table');
        var dialogBlock = $('#dialog-block').clone();
        dialogBlock.attr('id', 'message-row-' + dialogId);

        dialogBlock.removeAttr('style');

        dialogsTable.append(dialogBlock);

    };
    $.connection.hub.start();


});