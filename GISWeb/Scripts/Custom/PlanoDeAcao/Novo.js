






function OnSuccessCadastrarPlanoDeAcao(data) {
    $('#formCadastroPlanoDeAcao').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarPlanoDeAcao() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroPlanoDeAcao").css({ opacity: "0.5" });
}