$(document).ready(function () {
    
    var game = $('#game').val();
    $(':checkbox').change(function () {
        CalculatePrice();
    });
    SelectFilterItem(game, true);
});


function CalculatePrice() {
    var sellerPay = $('#sellerPay').is(':checked');
    var price = $('#ListingPrice').val();
    var floatPrice = parseFloat(price)
    var guaranteePrice = 0;
    var newPrice = 0;
    if (!isNaN(floatPrice)) {
        if (sellerPay) {
            if (floatPrice < 3000) {
                guaranteePrice = 300;

            }
            else if (floatPrice < 15000) {
                guaranteePrice = floatPrice * 0.1;
            }
            else {
                guaranteePrice = 1500;
            }
            newPrice = floatPrice - guaranteePrice;
        }
        else {
            newPrice = floatPrice;
        }
        guaranteePrice = Math.round(guaranteePrice);
        newPrice = Math.round(newPrice);
        $('#guaranteePrice').val(guaranteePrice).text(guaranteePrice);
        $('#EstimatedPayout').val(newPrice).text(newPrice);
    }

}