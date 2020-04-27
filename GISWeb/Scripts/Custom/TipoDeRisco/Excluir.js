
function OnBeginExcluirTipoDeRisco() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirTipoDeRisco").css({ opacity: "0.5" });
}

function OnSuccessExcluirTipoDeRisco(data) {
    $('#formExcluirTipoDeRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}