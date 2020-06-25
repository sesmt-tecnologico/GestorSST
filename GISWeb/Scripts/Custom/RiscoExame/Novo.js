
  

function OnBeginCadastrarExRiscos() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroExRiscos").css({ opacity: "0.5" });
}

function OnSuccessCadastrarExRiscos(data) {
    $('#formCadastroExRiscos').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}