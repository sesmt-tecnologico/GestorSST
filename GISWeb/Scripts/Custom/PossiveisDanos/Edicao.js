
function OnSuccessCadastrarDano(data) {
    $('#formEdicaoDano').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarDano() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoDano").css({ opacity: "0.5" });
}