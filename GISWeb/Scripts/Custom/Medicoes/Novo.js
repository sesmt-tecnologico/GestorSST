
  

function OnBeginCadastrarMedicoes() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroMedicoes").css({ opacity: "0.5" });
}

function OnSuccessCadastrarMedicoes(data) {
    $('#formCadastroMedicoes').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}