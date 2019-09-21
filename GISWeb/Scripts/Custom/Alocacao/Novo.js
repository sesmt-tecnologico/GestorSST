



function OnSuccessCadastrarAlocacao(data) {
    $('#formCadastroAlocacao').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarAlocacao() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroAlocacao").css({ opacity: "0.5" });
}