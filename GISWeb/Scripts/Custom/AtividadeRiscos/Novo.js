






function OnSuccessCadastrarPerigo(data) {
    $('#formCadastroPerigo').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarPerigo() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroPerigo").css({ opacity: "0.5" });
}