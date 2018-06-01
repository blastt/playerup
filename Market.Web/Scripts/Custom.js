




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
    
    RefreshPageOffers();
});

function SearchButton() {
    $('#page').val(1);
    SearchOffers();
}

function ResetButton() {
    $('#page').val(1);
    ResetOffers();
}

function SelectPage(page) {

    $('#page').val(page);
    var game = $('#game').val();
    //$('html, body').animate({ scrollTop: 0 }, 0);
    
    SearchOffersPage();

    //var route = new Object();
    //route.Page = page;
    //route.Game = game;
    //urlPath = Router.action('Offer', 'Buy', route);
    //window.history.pushState("", "", urlPath);
}
function ChangeGame(game) {
    $('#game').val(game);

    ResetOffers();
}

function RefreshPageOffers() {
    var g = $('#game').val();
    var page = $('#page').val();
    var searchString = $('#searchString').val();
    var filters = [];
    $(".image-select").each(function () {
        filters.push({ "attribute": $(this).find('.selselsel').val(), "value": "empty" })
    });

    var message = {
        "page": page,
        "sort": $('#sort').val(),
        "priceFrom": $('#priceFrom').val(),
        "priceTo": $('#priceTo').val(),
        "isOnline": false,
        "searchInDiscription": false,
        "searchString": searchString,
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
            $('#list').empty();
            //$('#find').show();
            //$('#find').animate({ opacity: '0.7' }, 400);

        },
        success: function (response) {

            //$('#find').hide()

            $('#list').html(response);
            Slider(parseFloat($('#priceFrom').val()), parseFloat($('#priceTo').val()));
            SelectFilterItem(g, true);
            //var urlPath;
            //var routes = new Object();
            //routes.game = g;
            //urlPath = Router.action('Offer', 'Buy', routes);
            //window.history.pushState("", "", urlPath);
            //var currentPath = window.location.href.split('/');
            //var urlPath = Router.action('Offer', 'Buy', { game: g });
            //window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            //$('#find').animate({ opacity: '0.0' }, 400, "", function () {

            //});

            //SelectFilterItem(g, false);
        }
    });
}
function SearchOffersPage() {
    var game = $('#game').val();
    var page = $('#page').val();
    var sort = $('#sort').val();
    var isOnline = $('#isOnline').is(':checked');
    var searchInDiscription = $('#searchInDiscription').is(':checked');
    var searchString = $('#searchString').val();
    var filters = [];
    $(".image-select").each(function () {
        filters.push({ "attribute": $(this).find('.selselsel').val(), "value": $(this).find('.selsel').val() })
    });


    var message = {
        "page": page,
        "sort": sort,
        "isOnline": isOnline,
        "searchInDiscription": searchInDiscription,
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


            // $('#loader').show();
            //$('#loader').animate({ opacity: '0.7' }, 400);


        },
        success: function (response) {
            var s = $('#sort').val();

            $('#offer-block').html(response);

            //urlPath = Router.action('Offer', 'Buy', routes);
            //window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            //$('#loader').animate({ opacity: '0.0' }, 400, "", function () {
            //$('#loader').hide();
            //});
            var urlPath;
            var routes = new Object();
            if (page !== "1") {
                routes.page = page;

            }
            routes.game = game;
            urlPath = Router.action('Offer', 'Buy', routes);
            var href = document.location.href.split('#')[0];
            window.history.pushState("", "", href + "#page=" + page);


            if (typeof s !== "undefined") {
                $('#sort').val(s);
            }

            //SelectFilterItem(g, false);
        },
        error: function (response) {
            alert("error");
        }
    });
}

function SearchOffers() {
    var game = $('#game').val();
    var page = 1;
    var sort = $('#sort').val();
    var isOnline = $('#isOnline').is(':checked');
    var searchInDiscription = $('#searchInDiscription').is(':checked');
    var searchString = $('#searchString').val();
    var priceFrom = $('#priceFrom').val();
    var priceTo = $('#priceTo').val();
    var selsel = $('#selsel').val();
    var filters = [];
    $(".image-select").each(function () {              
        filters.push({ attribute: $(this).find('.selselsel').val(), value: $(this).find('.selsel').val() })
    });
    

    var message = {
        "page": page,
        "sort": sort,
        "isOnline": isOnline,
        "searchInDiscription": searchInDiscription,
        "searchString": searchString,
        "jsonFilters": filters,
        "Filters" : filters,
        "game": game,
        "priceFrom": priceFrom,
        "priceTo": priceTo
    };

    $.ajax({
        url: '/Offer/List',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",
        beforeSend: function () {
            
            
           // $('#loader').show();
            //$('#loader').animate({ opacity: '0.7' }, 400);


        },
        success: function (response) {
            var s = $('#sort').val();
            

            $('#offer-block').html(response);
            var urlPath;
            var routes = new Object();            
            
         
            if (sort !== "bestSeller" && sort !== undefined) {
                routes.sort = sort;
            }
            
            if (isOnline) {
                routes.isOnline = isOnline;
            }
            if (searchInDiscription) {
                routes.searchindiscription = searchInDiscription;
            }
            if (searchString !== "") {

                routes.searchstring = searchString;
            }
            if (priceFrom !== $('#minGamePrice').val().split(',')[0]) {

                routes.pricefrom = priceFrom;
            }

            if (priceTo !== $('#maxGamePrice').val().split(',')[0]) {

                routes.priceto = priceTo;
            }
        
            //routes.attribute = filtersStr.replace(/,\s*$/, "");

            routes.game = game;
            urlPath = Router.action('Offer', 'Buy', routes);

            
            var filtersString = "";
            filters.forEach(function (element) {
                if (element.value !== "empty") {
                    filtersString += "filters=" + element.attribute + "=" + element.value + "&";
                }
                
            });
            
            if (filtersString.length !== 0) {
                urlPath += "?";
                filtersString = filtersString.substring(0, filtersString.length - 1);
                urlPath += filtersString;
            }
            
            window.history.pushState("", "", urlPath);
            //urlPath = Router.action('Offer', 'Buy', routes);
            //window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            //$('#loader').animate({ opacity: '0.0' }, 400, "", function () {
                //$('#loader').hide();
            //});
          
            if (typeof s !== "undefined") {
                $('#sort').val(s);
            }
            
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
        "priceFrom": $('#minGamePrice').val(),
        "priceTo": $('#maxGamePrice').val(),
        "game": g
    };
    $.ajax({
        url: '/Offer/Reset',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",
        beforeSend: function () {
            //$('#list').empty();
            //$('#find').show();
            //$('#find').animate({ opacity: '0.7' }, 400);

        },
        success: function (response) {

            //$('#find').hide()
            $('#searchForm').get(0).reset();
            $('#list').html(response);
            
            Slider(parseFloat($('#priceFrom').val()), parseFloat($('#priceTo').val()));
            SelectFilterItem(g, true);
            var urlPath;
            var routes = new Object();
            routes.game = g;
            urlPath = Router.action('Offer', 'Buy', routes);
            window.history.pushState("", "", urlPath);
            //var currentPath = window.location.href.split('/');
            //var urlPath = Router.action('Offer', 'Buy', { game: g });
            //window.history.pushState({ "html": response.html, "pageTitle": response.pageTitle }, "", urlPath);
            //$('#find').animate({ opacity: '0.0' }, 400, "", function () {
                
            //});


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
        min: parseFloat($("#minGamePrice").val()),
        max: parseFloat($("#maxGamePrice").val()),
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







