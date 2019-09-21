
function OnSuccessCadastrarRiscos(data) {
    $('#formEdicaoRiscos').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarRiscos() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoRiscos").css({ opacity: "0.5" });
}