$(function () {
    // приготовим Id пользователя
        // прокси         
    

    // объявляем callback, который среагирует на событие сервера          
    

    // Запускаем хаб
    //$.connection.hub.start().done(function () {
    //    // расскажем серверу кто подключился
    //    alert("few");
        
        
    //});

    //$.connection.stateHub.client.SetOnline = function (message) {
    //    alert(message);
    //}

    
    $.connection.hub.start().done(function () {
        // расскажем серверу кто подключился
        alert("few");


    });
    $.connection.statusHub.client.clientConected = function (c, c) {
        alert("cc");
    }
});