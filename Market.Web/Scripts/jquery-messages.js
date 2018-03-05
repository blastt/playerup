$("#messages .set-viewed-icon").each(function () {
    $(this).on('click', function (e) {
        var obj = $(this);
        var id = $(this).data('id');
        $.ajax({
            url: "/Message/SetViewedAjax",
            data: { id: id },
            //cache: false,
            //contentType: false,
            //processData: false,
            //mimeType: "multipart/form-data",
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result.Success) {
                    $(obj).closest('a').closest('td').closest('tr').removeClass('font-weight-bold'); // You can remove row like this
                    $(obj).remove(); // You can remove row like this

                }
                eval(result.Script);
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
});

$(".clickable").each(function () {
    $(this).on('click', function (e) {
        var id = $(this).data('id');
        location.href = "/message/info?messageId=" + id;
    });
});