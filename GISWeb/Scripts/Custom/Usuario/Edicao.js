
function OnSuccessAtualizarPerfil(data) {
    $('#formEdicaoPerfil').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarPerfil() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoPerfil").css({ opacity: "0.5" });
}