
function OnBeginExcluirRisco() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirRisco").css({ opacity: "0.5" });
}

function OnSuccessExcluirRisco(data) {
    $('#formExcluirRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}