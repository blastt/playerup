



//Ajax paging
//$(".page-item").each(function (index) {
//    alert("ffq");
//    $(this).on("click", function () {
//        // For the boolean value
//        alert('loh');
//    });
//});



//------
$(document).ready(function () {
    ResetOffers();
});

function SelectPage(page) {

    $('#page').val(page);
    
    $('html, body').animate({ scrollTop: 0 }, 0);
    SearchOffers();
}
function ChangeGame(game) {
    $('#game').val(game);

    ResetOffers();
}

function SearchOffers() {

    var g = $('#game').val();
    var filterItemValues = [];
    $(".selsel").each(function () {
        filterItemValues.push($(this).val());
    });
    var filterValues = [];
    $(".selselsel").each(function () {
        filterValues.push($(this).val());
    });
    var message = {
        "page": $('#page').val(),
        "sort": $('#sort').val(),
        "isOnline": $('#isOnline').is(':checked'),
        "searchInDiscription": $('#searchInDiscription').is(':checked'),
        "searchString": $('#searchString').val(),
        "filterItemValues": filterItemValues,
        "filterValues": filterItemValues,
        "game": g,
        "priceFrom": $('#priceFrom').val(),
        "priceTo": $('#priceTo').val()
    };
    $.ajax({
        url: '/Offer/List',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",
        beforeSend: function () {
            $('#loader').show();
            $('#loader').animate({ opacity: '0.7' }, 400);



        },
        success: function (response) {
            var s = $('#sort').val();

            $('#list').html(response);
            $(".game-filter-item").each(function (index) {
                if (this.dataset.game === g) {
                    $(this).addClass("active");
                    return;
                }

            });
            var urlPath = Router.action('Offer', 'Buy', { game: g });
            window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            $('#loader').animate({ opacity: '0.0' }, 400, "", function () {
                $('#loader').hide();
            });





            if (typeof s !== "undefined") {
                $('#sort').val(s);
            }


            slider();

            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
        }
    });
}

function ResetOffers() {

    var g = $('#game').val();
    var filterItemValues = [];
    $(".selsel").each(function () {
        filterItemValues.push($(this).val());
    });
    var filterValues = [];
    $(".selselsel").each(function () {
        filterValues.push($(this).val());
    });
    var message = {
        "page": 1,
        "sort": "bestSeller",
        "isOnline": false,
        "searchInDiscription": false,
        "searchString": "",
        "filterItemValues": "all",
        "filterValues": "all",
        "game": g
    };
    $.ajax({
        url: '/Offer/List',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",
        beforeSend: function () {
            $('#loader').show();
            $('#loader').animate({ opacity: '0.7' }, 400);

        },
        success: function (response) {

            $('#list').html(response);
            $(".game-filter-item").each(function (index) {

                if (this.dataset.game === g) {

                    $(this).addClass("active");
                    return;
                }

            });
            var currentPath = window.location.href.split('/');
            var urlPath = Router.action('Offer', 'Buy', { game: g });
            window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            $('#loader').animate({ opacity: '0.0' }, 400, "", function () {
                $('#loader').hide();
            });

            slider();

            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
        }
    });
}

function getVals() {
    // Get slider values
    var parent = this.parentNode;
    var slides = parent.getElementsByTagName("input");
    var slide1 = parseFloat(slides[0].value);
    var slide2 = parseFloat(slides[1].value);
    // Neither slider will clip the other, so make sure we determine which is larger
    if (slide1 > slide2) { var tmp = slide2; slide2 = slide1; slide1 = tmp; }

    var displayElement = parent.getElementsByClassName("rangeValues")[0];
    $('#priceFrom').val(slide1);
    $('#priceTo').val(slide2);
    
}

function slider() {
    // Initialize Sliders
    var sliderSections = document.getElementsByClassName("range-slider");
    for (var x = 0; x < sliderSections.length; x++) {
        var sliders = sliderSections[x].getElementsByTagName("input");
        for (var y = 0; y < sliders.length; y++) {
            if (sliders[y].type === "range") {
                sliders[y].oninput = getVals;
                // Manually trigger event first time to display values
                sliders[y].oninput();
            }
        }
    }
}




//function SelectGame(game) {
//    if (game != null) {
//        $('#game').val(game);
//    } 
//    $(this).addClass("active");

//    SearchOffers();
//}





