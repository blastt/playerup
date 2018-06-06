

$(document).ready(function () {
    var messageInput = $('#messageBody');
    messageInput.click(function () {
        SetMessagesViewed();
    });
    messageInput.keypress(function (e) {
        if (e.which == 13) {
            event.returnValue = false;
            if (messageInput.text().trim() !== '') {

                CreateNewMessage();
                SetMessagesViewed();
            }            
            return false;
                
            
        }
        
    });
});


function SetMessagesViewed() {
    $.ajax({
        url: '/Message/SetMessagesViewed',
        type: "POST",
        data: { dialogId: $('#dialogId').val() },
        dataType: "json",
        success: function (response) {

        }
    });

}

window.onload = function () {
    messageScrollToButton();
}
function messageScrollToButton() {
    var objDiv = $('#messages-col');
    objDiv.scrollTop(objDiv[0].scrollHeight);
}

//function inboxScrollToButton() {
//    $(window).scrollTop(320);
//}

var btn = document.getElementById("send");
        var messageInput = $('#messageBody');
        btn.onclick = function () {
            if (messageInput.text().trim() !== '') {
                CreateNewMessage();
            }

        }

        function CreateNewMessage() {
            var message = {
                "MessageBody": $('#messageBody').text(),
                "ReceiverId": $('#receiverId').val()
            };
            messageInput.text("");
            $.ajax({
                url: "/Message/New",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ model: message }),
                dataType: "Json",

                success: function (result) {
                    messageInput.text("");
                   
                },
                error: function () {
                    messageInput.text("");
                }
            });
            modal.style.display = "none";
        }