
function OnBeginCadastrarFornecedor() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroFornecedor").css({ opacity: "0.5" });
}

function OnSuccessCadastrarFornecedor(data) {
    $('#formCadastroFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}