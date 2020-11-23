

  


function OnBeginCadastrarAtividade() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroAtividade").css({ opacity: "0.5" });
}


function OnSucessCadastrarAtividade(data) {
    $('#formCadastroAtividade').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}
