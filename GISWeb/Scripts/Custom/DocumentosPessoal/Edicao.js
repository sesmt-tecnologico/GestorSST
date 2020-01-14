jQuery(function ($) {

    AplicaDatePicker();
    DatePTBR();

});


function OnSuccessAtualizarDocumentos(data) {
    
    $('#formEditarDocumentos').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarDocumentos() {    
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEditarDocumentos").css({ opacity: "0.5" });
}