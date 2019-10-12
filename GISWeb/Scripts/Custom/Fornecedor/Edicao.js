

function OnSuccessCadastrarFornecedor(data) {
    $('#formEdicaoFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarFornecedor() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoFornecedor").css({ opacity: "0.5" });
}