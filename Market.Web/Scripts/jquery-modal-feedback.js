// Get the modal

var modal = document.getElementById('modal-dialog');
var modal2 = $('#m-dialog');

var giveBtn = document.getElementById("givefeed");

// Get the <span> element that closes the modal


// When the user clicks on the button, open the modal 
giveBtn.onclick = function () {
    modal.style.display = "block";
}

// Get the button that opens the modal
var btn = document.getElementById("leaveFeedback");

// Get the <span> element that closes the modal
var span = document.getElementById("close");

var messageInput = $('#comment');
btn.onclick = function () {

    if (messageInput.val().trim() !== '') {
        alert("");
        GiveFeedback();
    }

}


// When the user clicks on the button, open the modal 
function GiveFeedback() {

    var message = {
        "Grade": $('input[name=Grade]:checked').val(),
        "Comment": $('#comment').val(),
        "OrderId": $('#orderId').val(),
        "ReceiverId": $('#receiverId').val(),
    };
    $.ajax({
        url: "/Feedback/GiveFeedback",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ model: message }),
        dataType: "Json",

        success: function (result) {
            modal2.css("display", "block");
            modal2.css("opacity", "1.0");
            modal2.animate({ opacity: '0.0' }, 5000, "", function () {
                modal2.css("display", "none");
            });
        },
        error: function () {
            alert("Вы не ввели текст");
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