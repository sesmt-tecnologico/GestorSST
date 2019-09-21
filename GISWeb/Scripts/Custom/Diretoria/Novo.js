




function OnSuccessCadastrarDiretoria(data) {
    $('#formCadastroDiretoria').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarDiretoria() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroDiretoria").css({ opacity: "0.5" });
}