




function OnSucessCadastrarEquipe(data) {
    $('#FormCadastroEquipe').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarEquipe() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#FormCadastroEquipe").css({ opacity: "0.5" });
}