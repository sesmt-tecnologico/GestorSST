
function OnBeginCadastrarFuncao() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroFuncao").css({ opacity: "0.5" });
}

function OnSuccessCadastrarFuncao(data) {
    $('#formCadastroFuncao').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}