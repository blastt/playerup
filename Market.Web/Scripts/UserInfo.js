$(document).ready(function () {
    SearchOffersInfo();
});
function SelectPageOffersInfo(page) {
    $('#page').val(page);
    SearchOffersInfo();
}
function SelectPageFeedbacksInfo(page) {
    $('#page').val(page);
    SearchFeedbacksInfo();
}
$(".info-menu-item").each(function (index) {

    $(this).on("click", function () {
        $(".info-menu-item").each(function (index) {
            $(this).removeClass('active');

        });
        $(this).addClass('active');

    });
});

$(".offers-div").on("click", function () {
    SearchOffersInfo();
});

$(".feedbacks-div").on("click", function () {
    SearchFeedbacksInfo();
});

function SearchOffersInfo() {

    var g = $('#sort-game');
    var message = {
        "game": g.val(),
        "userId": $('#user-id').val(),
        "page": $('#page').val(),
        "searchString": $('#searchString').val()
    };
    $.ajax({
        url: '/Offer/OfferListInfo',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",

        success: function (response) {

            var sort = $('#sort-game').val();
            $('#list-info').html(response);
            if (typeof sort != 'undefined') {

                $('#sort-game').val(sort);
            }


            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
        }
    });


}

function SearchFeedbacksInfo() {

    var g = $('#game').val();
    var message = {
        "userId": $('#user-id').val(),
        "page": $('#page').val(),
        "searchString": $('#searchString').val()

    };
    $.ajax({
        url: '/Feedback/FeedbackListInfo',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",

        success: function (response) {


            $('#list-info').html(response);


            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
        }
    });


}