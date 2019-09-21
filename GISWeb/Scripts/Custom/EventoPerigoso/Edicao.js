
function OnSuccessCadastrarEvento(data) {
    $('#formEdicaoEvento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarEvento() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEvento").css({ opacity: "0.5" });
}