

$(".clickable").each(function () {
    $(this).on('click', function (e) {
        var id = $(this).data('id');
        location.href = "/message/info?messageId=" + id;
    });
});



$("#messages #linkdelete").each(function () {

    $(this).on('click', function (e) {
        var obj = $(this); // first store $(this) in obj
        var id = $(this).data('id'); // get id of data using this 
        $.ajax({
            url: "/Message/DeleteAjax",
            data: { id: id },
            //cache: false,
            //contentType: false,
            //processData: false,
            //mimeType: "multipart/form-data",
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result.Success) {
                    $(obj).closest("tr").hide('slow', function () { $(obj).closest("tr").remove(); }); // You can remove row like this
                }
                eval(result.Script);
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
});