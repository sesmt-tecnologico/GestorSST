


function OnSuccessAtualizarDocumentos(data) {
    alert("A");
    $('#formEditarDocumentos').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarDocumentos() {
    alert("B");
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEditarDocumentos").css({ opacity: "0.5" });
}