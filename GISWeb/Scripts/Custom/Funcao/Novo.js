




function OnSuccessCadastrarFuncao(data) {
    $('#formCadastroFuncao').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarFuncao() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroFuncao").css({ opacity: "0.5" });
}