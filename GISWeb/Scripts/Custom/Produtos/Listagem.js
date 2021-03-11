jQuery(function ($) {

    var status = $("#txtstatus").val();  

    $('#txtstatus').change(function () {

        if ($(this).is(':checked')) {

            $('#txtQuantMinima').prop("disabled", false);
        } else {
            $('#txtQuantMinima').prop("disabled", true);
        }

        
    });



});
