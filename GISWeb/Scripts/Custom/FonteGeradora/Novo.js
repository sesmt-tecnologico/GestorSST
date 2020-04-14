




function OnSuccessCadastrarFonte(data) {
    $('#formCadastroFonte').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarFonte() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroFonte").css({ opacity: "0.5" });
}