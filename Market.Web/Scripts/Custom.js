$(document).ready(function () {
    SearchOffers();
});



//Ajax paging
//$(".page-item").each(function (index) {
//    alert("ffq");
//    $(this).on("click", function () {
//        // For the boolean value
//        alert('loh');
//    });
//});



//------


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





