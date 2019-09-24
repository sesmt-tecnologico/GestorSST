function OnSuccessCadastrarPerfil(data) {
    $('#formCadastroPerfil').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarPerfil() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroPerfil").css({ opacity: "0.5" });
}