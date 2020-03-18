
jQuery(function ($) {

    AplicaDatePicker();
    DatePTBR();

});


function OnSuccessCadastrarDoc(data) {
   
    $('#formCadastroDoc').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    
}



function OnBeginCadastrarDoc() {
    
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroDoc").css({ opacity: "0.5" });
}