
$(document).ready(function () {
    console.log(data_info_remas);
    console.log("dimoa");
    //recorre cada item del td
    $.each($('tr td'), function (k, itemsdf) {
        // se obtienen sus atributos y se parsea
        var data = $(itemsdf).attr('data-coord');

        console.log(data);
        debugger;
    });
});