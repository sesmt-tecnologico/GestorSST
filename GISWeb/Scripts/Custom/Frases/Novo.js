jQuery(function ($) {

   

});

function OnSuccessCadastrarFrasesSeguranca(data) {
    $('#formCadastroFrases').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarFrasesSeguranca() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroFrases").css({ opacity: "0.5" });
}