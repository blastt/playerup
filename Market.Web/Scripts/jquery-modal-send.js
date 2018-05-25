// Get the modal

var modal = document.getElementById('modal-dialog');
var modal2 = $('#m-dialog');

var sendBtn = document.getElementById("contact");

sendBtn.onclick = function () {
    modal.style.display = "block";
}
// Get the <span> element that closes the modal
var span = document.getElementById("close");

var btn = document.getElementById("sendMessage");

var messageInput = $('#messageBody');
btn.onclick = function () {
    if (messageInput.val().trim() !== '') {
        SendMessage();
    }

}


// When the user clicks on the button, open the modal 
function SendMessage() {

    var message = {
        "OfferHeader": $('#offerHeader').val(),
        "MessageBody": $('#messageBody').val(),
        "ReceiverId": $('#receiverId').val(),
        "Subject": $('#subject').val(),
    };
    $.ajax({
        url: "/Message/New",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ model: message }),
        dataType: "Json",

        success: function (result) {
            
            if (result.success) {
                modal2.css("display", "block");
                modal2.css("opacity", "1.0");
                modal2.animate({ opacity: '0.0' }, 5000, "", function () {
                    modal2.css("display", "none");
                });
            }
            else {
                alert(result.responseText);
            }
        },
        error: function (response) {
            alert("Для отправки сообщений нужно зарегистрироваться");
        }
    });
    modal.style.display = "none";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {

    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target === modal) {
        modal.style.display = "none";
    }
}