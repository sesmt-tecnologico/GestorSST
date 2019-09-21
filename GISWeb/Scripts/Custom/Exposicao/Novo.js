jQuery(function ($) {
    

    if ($("#ContinuoID").click(function () {

        if ($(this).is(':checked')) {
        $("#IntermitenteID").prop("disabled", true);
        $('#EventualID').prop("disabled", true);
        }
                    
        else{
        $("#IntermitenteID").prop("disabled", false);
        $('#EventualID').prop("disabled", false);

        }
    }));
    if ($("#IntermitenteID").click(function () {

        if ($(this).is(':checked')) {
            $("#ContinuoID").prop("disabled", true);
            $('#EventualID').prop("disabled", true);

        } else {
            $("#ContinuoID").prop("disabled", false);
            $('#EventualID').prop("disabled", false);

        }
    }));

    if ($("#EventualID").click(function () {

        if ($(this).is(':checked')) {
            $("#ContinuoID").prop("disabled", true);
            $('#IntermitenteID').prop("disabled", true);

        } else {
            $("#ContinuoID").prop("disabled", false);
            $('#IntermitenteID').prop("disabled", false);

        }

    }));
    
});

  

