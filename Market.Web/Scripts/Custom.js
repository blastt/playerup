
function ajaxFilters(g, isListView) {

    $.ajax({
        url: "/Offer/GetFiltersJson",
        contentType: "application/json",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({ game: g }),

        success: function (data) {

            var div = $('#rank');
            var divRank = $('<div></div>');
            div.empty();

            if (Object.keys(data).length != 0) {

                divRank.addClass('form-group');


                if (g == 'dota2') {
                    AddDota2RankFields(divRank, data, isListView);
                }
                else if (g == 'csgo') {
                    AddCsgoRankFields(divRank, data, isListView);
                }
                div.append(divRank);

            }
        }
    });
}