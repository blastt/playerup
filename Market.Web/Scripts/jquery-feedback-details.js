function SelectPage(page) {

    $('#page').val(page);
    SearchFeedbacksDetails();
}
function SearchFeedbacksDetails() {
    var message = {
        "page": $('#page').val(),
        "filter": $('#filter').val(),
        "userId": $('#user-id').val()
    };
    $.ajax({
        url: '/Feedback/FeedbackList',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(message),
        dataType: "html",
        beforeSend: function () {
            $('#loader').show();

        },
        success: function (response) {

            $('#feedback-list').html(response);
            $('#loader').hide();

            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
        }
    });
}