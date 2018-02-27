﻿function SearchOffers() {
    var message = {
        "page": p,
        "sort": sort,
        "online": online,
        "search": searchString,
        "filterItems": filters,
        "game": game,
        "priceFrom": priceFrom,
        "priceTo": priceTo
    };
    $.ajax({
        type: "POST",
        data: { action: "OfferList", data: message },
        dataType: "Json",
        success: function (response) {

        }
    });
}



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

function CustomSelect() {
    var x, i, j, selElmnt, a, b, c;
    /*look for any elements with the class "custom-select":*/
    x = document.getElementsByClassName("custom-select");
    for (i = 0; i < x.length; i++) {
        selElmnt = x[i].getElementsByTagName("select")[0];
        /*for each element, create a new DIV that will act as the selected item:*/
        a = document.createElement("DIV");
        a.setAttribute("class", "select-selected");
        a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
        x[i].appendChild(a);
        /*for each element, create a new DIV that will contain the option list:*/
        b = document.createElement("DIV");
        b.setAttribute("class", "select-items select-hide");
        for (j = 1; j < selElmnt.length; j++) {
            /*for each option in the original select element,
            create a new DIV that will act as an option item:*/
            c = document.createElement("DIV");
            c.innerHTML = selElmnt.options[j].innerHTML;
            c.addEventListener("click", function (e) {
                /*when an item is clicked, update the original select box,
                and the selected item:*/
                var i, s, h;
                s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                h = this.parentNode.previousSibling;
                for (i = 0; i < s.length; i++) {
                    if (s.options[i].innerHTML == this.innerHTML) {
                        s.selectedIndex = i;
                        h.innerHTML = this.innerHTML;
                        break;
                    }
                }
                h.click();
            });
            b.appendChild(c);
        }
        x[i].appendChild(b);
        a.addEventListener("click", function (e) {
            /*when the select box is clicked, close any other select boxes,
            and open/close the current select box:*/
            e.stopPropagation();
            closeAllSelect(this);
            this.nextSibling.classList.toggle("select-hide");
            this.classList.toggle("select-arrow-active");
        });
    }
    function closeAllSelect(elmnt) {
        /*a function that will close all select boxes in the document,
        except the current select box:*/
        var x, y, i, arrNo = [];
        x = document.getElementsByClassName("select-items");
        y = document.getElementsByClassName("select-selected");
        for (i = 0; i < y.length; i++) {
            if (elmnt == y[i]) {
                arrNo.push(i)
            } else {
                y[i].classList.remove("select-arrow-active");
            }
        }
        for (i = 0; i < x.length; i++) {
            if (arrNo.indexOf(i)) {
                x[i].classList.add("select-hide");
            }
        }
    }
    document.addEventListener("click", closeAllSelect);
}