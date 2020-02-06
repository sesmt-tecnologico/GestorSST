



function OnSuccessCadastrarTipoControle(data) {
    $('#formCadastroTipoControle').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarTipoControle() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroTipoControle").css({ opacity: "0.5" });
}