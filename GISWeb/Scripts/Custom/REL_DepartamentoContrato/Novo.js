
function OnBeginCadastrarREL_DeapartamentoContrato() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroREL_DeapartamentoContrato").css({ opacity: "0.5" });
}

function OnSuccessCadastrarREL_DeapartamentoContrato(data) {
    $('#formCadastroREL_DeapartamentoContrato').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}