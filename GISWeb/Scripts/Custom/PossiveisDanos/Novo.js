






function OnSuccessCadastrarDano(data) {
    $('#formCadastroDano').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarDano() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroDano").css({ opacity: "0.5" });
}