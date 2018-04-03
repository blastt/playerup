// Get the modal
var modal = document.getElementById('modal-dialog');
var modal2 = $('#m-dialog');

// Get the button that opens the modal
var btn = document.getElementById("sendMessage");

// Get the <span> element that closes the modal
var span = document.getElementById("close");

// When the user clicks on the button, open the modal 
btn.onclick = function () {
    alert('few');
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
            modal2.css("display", "block");
            modal2.css("opacity", "1.0");
            modal2.animate({ opacity: '0.0' }, 5000, "", function () {
                modal2.css("display","none");
            });
        },
        error: function () {
            alert("خطا!");
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