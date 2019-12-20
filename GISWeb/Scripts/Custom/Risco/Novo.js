






function OnSuccessCadastrarRisco(data) {
    $('#formCadastroRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarRisco() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroRisco").css({ opacity: "0.5" });
}