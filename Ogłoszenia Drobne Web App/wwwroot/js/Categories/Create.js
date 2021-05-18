
    $.get("/Categories/GetCategoriesWithParent", function () {
        alert("success");
    })
        .done(function (data) {
            alert("second success");
            console.log(data)
        })
        .fail(function () {
            alert("error");
        })
        .always(function () {
            alert("finished");
        });
