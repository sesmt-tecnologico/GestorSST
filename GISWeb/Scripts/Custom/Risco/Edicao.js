
function OnSuccessCadastrarRisco(data) {
    $('#formEdicaoRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarRisco() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoRisco").css({ opacity: "0.5" });
}