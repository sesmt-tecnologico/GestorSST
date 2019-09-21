




function OnSuccessCadastrarEquipe(data) {
    $('#formCadastroEquipe').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarEquipe() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroEquipe").css({ opacity: "0.5" });
}