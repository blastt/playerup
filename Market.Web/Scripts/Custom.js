$(document).ready(function () {
    Slider(parseFloat($('#priceFrom').val()), parseFloat($('#priceTo').val()));
    SelectFilterItem($('#game').val(), true);
    //RefreshPageOffers();

});

function SearchButton() {
    $('#page').val(1);
    SearchOffers();
}

function SortChange() {
    var s = $('#select-sort').val();
    $('#sort').val(s);
    SearchOffers();
}

function ResetButton() {
    var routes = new Object();   
    routes.game = $('#game').val();;
    urlPath = Router.action('Offer', 'Buy', routes);
    window.history.pushState("", "", urlPath);
    location.reload();
    
}

function SelectPage(page) {

    $('#page').val(page);
    var game = $('#game').val();   
    SearchOffersPage();
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
    $(".filters").each(function () {
        
        filters.push($(this).val());
    });
   
    var message = {
        "page": page,
        "sort": $('#sort').val(),
        "priceFrom": $('#priceFrom').val(),
        "priceTo": $('#priceTo').val(),
        "isOnline": false,
        "searchInDiscription": false,
        "searchString": searchString,        
        "game": g
    };
    $.ajax({
        url: '/Offer/Reset',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message, filters: filters}),
        dataType: "html",
        beforeSend: function () {
            $('#list').empty();
        },
        success: function (response) {           
            $('#list').html(response);
            SelectFilterItem($('#game').val(), true);
        }
    });
}
function SearchOffersPage() {
    var game = $('#game').val();
    var page = $('#page').val();
    var sort = $('#sort').val();
    var priceFrom = $('#priceFrom').val();
    var priceTo = $('#priceTo').val();
    var isOnline = $('#isOnline').is(':checked');
    var searchInDiscription = $('#searchInDiscription').is(':checked');
    var searchString = $('#searchString').val();
    var filters = [];
    $(".filters").each(function () {
        filters.push($(this).val());
    });


    var message = {
        "page": page,
        "sort": sort,
        "isOnline": isOnline,
        "searchInDiscription": searchInDiscription,
        "searchString": searchString,
        "game": game,
        "priceFrom": $('#priceFrom').val(),
        "priceTo": $('#priceTo').val()
    };

    $.ajax({
        url: '/Offer/List',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message, filters: filters }),
        dataType: "html",

        success: function (response) {
            var s = $('#sort').val();

            $('#list').html(response);
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
            routes.game = game;

             if (page !== "1") {
                routes.page = page;

            }            
            urlPath = Router.action('Offer', 'Buy', routes);
            window.history.pushState("", "", urlPath);
            if (typeof s !== "undefined") {
                $('#sort').val(s);
            }
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
    
    $(".filters").each(function () {
        filters.push($(this).val());
    });


    var message = {
        "page": page,
        "sort": sort,
        "isOnline": isOnline,
        "searchInDiscription": searchInDiscription,
        "searchString": searchString,
        "jsonFilters": filters,       
        "game": game,
        "priceFrom": priceFrom,
        "priceTo": priceTo
    };

    $.ajax({
        url: '/Offer/List',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message, filters: filters }),
        dataType: "html",
        success: function (response) {
            var s = $('#sort').val();            
            $('#list').html(response);
            var urlPath;
            var routes = new Object();            
                    
            if (sort !== "bestSeller" && s !== undefined) {

                routes.sort = sort;
            }
            
            if (isOnline) {
                routes.isonline = isOnline;
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
        
            routes.game = game;
            urlPath = Router.action('Offer', 'Buy', routes);

            
            var filtersString = "";
            filters.forEach(function (element) {
                if (element.split('=')[1] !== "empty") {
                    filtersString += "filters=" + element + "&";
                }
                
            });
            
            if (filtersString.length !== 0) {
                urlPath += "?";
                filtersString = filtersString.substring(0, filtersString.length - 1);
                urlPath += filtersString;
            }
            
            window.history.pushState("", "", urlPath);

            if (typeof s !== "undefined") {
                $('#sort').val(s);
            }
        }
    });
}

function ResetOffers() {

    var g = $('#game').val();
    var filters = [];
    
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
        data: JSON.stringify({ searchInfo: message, filters: filters }),
        dataType: "html",
        success: function (response) {
            $('#searchForm').get(0).reset();
            $('#list').html(response);            
            Slider(parseFloat($('#priceFrom').val()), parseFloat($('#priceTo').val()));
            SelectFilterItem(g, true);
            var urlPath;
            var routes = new Object();
            routes.game = g;
            urlPath = Router.action('Offer', 'Buy', routes);
            window.history.pushState("", "", urlPath);
        }
    });
}

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