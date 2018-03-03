$(document).ready(function () {
    SearchOffers();
});

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


$(".game-filter-item").each(function (index) {
    if (index == 0 && window.location.href.split('=')[1] == undefined) {
        $(this).addClass("active");
        

    }
    else if (this.dataset.game == window.location.href.split('=')[1]) {
        $(this).addClass("active");


    }
   
    $(this).on("click", function () {
        // For the boolean value
        $(".game-filter-item").each(function (index) {
            $(this).removeClass("active");
        });
            
            var url = Router.action('offer', 'list', { game: this.dataset.game});
            window.history.pushState("2", "", url);

            if (this.dataset.game == window.location.href.split('=')[1]) {
                $(this).addClass("active");
                return;
            }  
        
            else if (window.location.href.split('=')[1] == "all") {
                $(this).addClass("active");
                return;
            }
        

        // For the mammal value

    });
});

//function SelectGame(game) {
//    if (game != null) {
//        $('#game').val(game);
//    } 
//    $(this).addClass("active");

//    SearchOffers();
//}

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
        "page": 1,
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
        url: '/Offer/OfferList',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ searchInfo: message }),
        dataType: "html",
        beforeSend: function () {
            $('#loader').show();

        },
        success: function (response) {
            var s = $('#sort').val();

            $('#list').html(response);
            $('#loader').hide();
            $('#sort').val(s)
            slider();

            //SelectFilterItem(g, false);
        },
        error: function () {
            alert("fff");
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

function CreateFilters(ranks) {
    var label, hidden, hidden2;
    var select;
    var div;
    var mainDiv = $("#filter-item");
    mainDiv.empty();
    for (var rank in ranks) {
        div = $("<div></div>");
        div.addClass("image-select");
        
        label = $("<label for='Filter.Value'>" + ranks[rank].Name + "</label>")
        select = $("<select class='selsel' id='filterItemValues' name='FilterItemValues'></select>");
        var defaultOption = $('<option>Выберите ранг</option>').val('empty');
        hidden2 = $('<input type="hidden" class="selselsel" id="filters" name="Filters" value="' + ranks[rank].Value + '">');
        select.append(defaultOption);
        for (var item in ranks[rank].FilterItems) {
            hidden = $('<input type="hidden" id="filterItems" name="FilterValues" value="' + ranks[rank].FilterItems[item].Value + '">');
            var url = "../../Content/Images/" + ranks[rank].GameValue + "/Ranks/" + ranks[rank].FilterItems[item].Image;
            var option = $('<option data-url="' + url + '"></option>').val(ranks[rank].FilterItems[item].Value).text(ranks[rank].FilterItems[item].Name);

            select.append(option);
        }
        div.append(select);
        div.append(hidden);
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
                    if (s.options[i].innerText == this.innerText) {
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