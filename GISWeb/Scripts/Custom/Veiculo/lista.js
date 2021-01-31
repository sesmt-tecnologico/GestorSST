jQuery(function ($) {

    Chosen();

    var UKEmpregado = $.trim($(".txtSupervisor").val());
    var UkRegistro = $.trim($(".txtRegistro").val());



    GetDocumento(UKEmpregado, UkRegistro);

    //getCadeado(UKFonteGeradora);


});






}