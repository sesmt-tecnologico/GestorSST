



function OnSuccessCadastrarClassificacaoMedida(data) {
    $('#formCadastroClassificacaoMedida').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarClassificacaoMedida() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroClassificacaoMedida").css({ opacity: "0.5" });
}