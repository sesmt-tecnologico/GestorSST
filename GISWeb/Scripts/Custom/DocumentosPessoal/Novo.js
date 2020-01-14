
jQuery(function ($) {

    AplicaDatePicker();
    DatePTBR();

});


function OnSuccessCadastrarDocumento(data) {
   
    $('#formCadastroDocumento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    
}



function OnBeginCadastrarDocumento() {
    
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroDocumento").css({ opacity: "0.5" });
}