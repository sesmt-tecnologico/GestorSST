jQuery(function ($) {

    Chosen();
   


});


function OnSuccessCadastrarWorkArea(data) {
    $('#formCadastroWorkArea').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarWorkArea() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroWorkArea").css({ opacity: "0.5" });
}