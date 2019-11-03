function OnBeginAtualizarPerigo() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoPerigo").css({ opacity: "0.5" });
}

function OnSuccessAtualizarPerigo(data) {
    $('#formEdicaoPerigo').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}