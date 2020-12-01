function OnBeginAtualizarFrases() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoFrases").css({ opacity: "0.5" });
}

function OnSuccessCadastrarFrases(data) {
    $('#formEdicaoFrases').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}