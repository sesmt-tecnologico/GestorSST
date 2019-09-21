






function OnSuccessCadastrarEvento(data) {
    $('#formCadastroEvento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarEvento() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEvento").css({ opacity: "0.5" });
}