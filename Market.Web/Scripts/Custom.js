



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
    H();
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
    var game = $('#game').val();
    var page = $('#page').val();
    var searchString = $('#searchString').val();
    var filters = [];
    $(".image-select").each(function () {              
        filters.push({ "attribute": $(this).find('.selselsel').val(), "value": $(this).find('.selsel').val() })
    });
    

    var message = {
        "page": page,
        "sort": $('#sort').val(),
        "isOnline": $('#isOnline').is(':checked'),
        "searchInDiscription": $('#searchInDiscription').is(':checked'),
        "searchString": searchString,
        
        "jsonFilters": filters,
        "game": game,
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

            $('#offer-block').html(response);

            
            var urlPath;
            var routes = new Object();
            if (game != "csgo") {
                routes.game = game;
                
            }
            if (page != "1") {
                routes.page = page;
            }

            urlPath = Router.action('Offer', 'Buy', routes);
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
        error: function (response) {
            alert("error");
        }
    });
}

function ResetOffers() {

    var g = $('#game').val();
    var filters = [];
    $(".image-select").each(function () {
        filters.push({ "attribute": $(this).find('.selselsel').val(), "value": "empty" })
    });

    var message = {
        "page": 1,
        "sort": "bestSeller",
        "isOnline": false,
        "searchInDiscription": false,
        "searchString": "",
        "jsonFilters": filters,
        "game": g
    };
    $.ajax({
        url: '/Offer/Reset',
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
            Slider(parseFloat($('#priceFrom').val()), parseFloat($('#priceTo').val()));
            SelectFilterItem(g, true);
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
        }
    });
}

//function getVals() {
//    // Get slider values
//    var parent = this.parentNode;
//    var slides = parent.getElementsByTagName("input");
//    var slide1 = parseFloat(slides[0].value);
//    var slide2 = parseFloat(slides[1].value);
//    // Neither slider will clip the other, so make sure we determine which is larger
//    if (slide1 > slide2) { var tmp = slide2; slide2 = slide1; slide1 = tmp; }

//    var displayElement = parent.getElementsByClassName("rangeValues")[0];
//    $('#priceFrom').val(slide1);
//    $('#priceTo').val(slide2);
    
//}
function Slider(fromPrice, toPrice) {
    
    $("#slider-range").slider({
        range: true,
        min: fromPrice,
        max: toPrice,
        values: [fromPrice, toPrice],
        slide: function (event, ui) {
            $("#priceFrom").val(ui.values[0]);
            $("#priceTo").val(ui.values[1]);
        }
    });
    $("#amount").val("$" + $("#slider-range").slider("values", 0) +
        " - $" + $("#slider-range").slider("values", 1));
};
//function sliderr() {
//    // Initialize Sliders
//    var sliderSections = document.getElementsByClassName("range-slider");
//    for (var x = 0; x < sliderSections.length; x++) {
//        var sliders = sliderSections[x].getElementsByTagName("input");
//        for (var y = 0; y < sliders.length; y++) {
//            if (sliders[y].type === "range") {
//                sliders[y].oninput = getVals;
//                // Manually trigger event first time to display values
//                sliders[y].oninput();
//            }
//        }
//    }
//}




//function SelectGame(game) {
//    if (game != null) {
//        $('#game').val(game);
//    } 
//    $(this).addClass("active");

//    SearchOffers();
//}







