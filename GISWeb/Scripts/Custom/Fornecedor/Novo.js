




function OnSuccessCadastrarFornecedor(data) {
    $('#formCadastroFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarFornecedor() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroFornecedor").css({ opacity: "0.5" });
}