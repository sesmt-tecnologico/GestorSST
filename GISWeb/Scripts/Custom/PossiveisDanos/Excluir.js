

function OnSuccessExcluirDano(data) {
    $('#formExcluirDano').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginExcluirDano() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirDano").css({ opacity: "0.5" });
}