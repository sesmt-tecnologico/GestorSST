

function OnSuccessExcluirTipoDeRisco(data) {
    $('#formExcluirTipoDeRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginExcluirTipoDeRisco() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirTipoDeRisco").css({ opacity: "0.5" });
}