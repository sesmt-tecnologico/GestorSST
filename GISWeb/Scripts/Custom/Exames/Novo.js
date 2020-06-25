
  

function OnBeginCadastrarExames() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroExames").css({ opacity: "0.5" });
}

function OnSuccessCadastrarExames(data) {
    $('#formCadastroExames').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}