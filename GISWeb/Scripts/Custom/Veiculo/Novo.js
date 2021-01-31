jQuery(function ($) {


   

});


function OnBeginCadastrarAtividade() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroMovimentacaoVeicular").css({ opacity: "0.5" });
}


function OnSuccessCadastrarMovimentacaoVeicular(data) {
    $('#formCadastroMovimentacaoVeicular').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}
