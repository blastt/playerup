$(document).ready(function () {
    
    var game = $('#game').val();
    $(':checkbox').change(function () {
        CalculatePrice();
    });
    SelectFilterItem(game, true);
});
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
                    if (s.options[i].innerHTML === this.innerHTML) {
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
            if (elmnt === y[i]) {
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

function CreateFilters(ranks) {
    var label, hidden, hidden2;
    var select;
    var div;
    var mainDiv = $("#filter-item");

    mainDiv.empty();
    for (var rank in ranks) {
        div = $("<div></div>");
        div.addClass("image-select");
        div.addClass("form-group");
        label = $("<label for='Filter.Value'>" + ranks[rank].Name + "</label>")
        select = $("<select class='selsel' id='filterItemValues' name='FilterItemValues'></select>");
        var defaultOption = $('<option>Выберите ранг</option>').val('empty');
        hidden2 = $('<input type="hidden" class="selselsel" id="filters" name="FilterValues" value="' + ranks[rank].Value + '">');
        select.append(defaultOption);
        for (var item in ranks[rank].FilterItems) {

            var url = "../../Content/Images/" + ranks[rank].GameValue + "/Ranks/" + ranks[rank].FilterItems[item].Image;
            var option = $('<option data-url="' + url + '"></option>').val(ranks[rank].FilterItems[item].Value).text(ranks[rank].FilterItems[item].Name);

            select.append(option);
        }
        div.append(select);

        div.append(hidden2);
        mainDiv.append(label).append(div);

    }
    myfunction();
}

function SelectFilterItem(g, isListView) {

    $.ajax({
        url: "/FilterItem/GetFiltersForGameJson",
        contentType: "application/json",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({ game: g }),

        success: function (ranks) {
            CreateFilters(ranks);

        },
        error: function myfunction() {
            alert("hi");
        }
    });
}




function myfunction() {
    var x, i, j, selElmnt, a, b, c, span;
    /*look for any elements with the class "custom-select":*/
    x = document.getElementsByClassName("image-select");
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
            span = document.createElement("IMG");
            var urlImg = selElmnt.options[j].dataset.url;
            span.setAttribute("src", urlImg)
            span.setAttribute("alt", "")

            span.style.width = "50px";
            span.style.marginRight = "15px";
            c = document.createElement("DIV");
            c.appendChild(span);
            c.innerHTML += selElmnt.options[j].innerHTML;
            c.addEventListener("click", function (e) {
                /*when an item is clicked, update the original select box,
                and the selected item:*/
                var i, s, h;
                s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                h = this.parentNode.previousSibling;
                for (i = 0; i < s.length; i++) {
                    if (s.options[i].innerText === this.innerText) {
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
    document.addEventListener("click", closeAllSelect);
}

function closeAllSelect(elmnt) {
    /*a function that will close all select boxes in the document,
    except the current select box:*/
    var x, y, i, arrNo = [];
    x = document.getElementsByClassName("select-items");
    y = document.getElementsByClassName("select-selected");
    for (i = 0; i < y.length; i++) {
        if (elmnt === y[i]) {
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