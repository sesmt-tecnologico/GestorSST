
function OnSuccessCadastrarPerigo(data) {
    $('#formEdicaoPerigo').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarPerigo() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoPerigo").css({ opacity: "0.5" });
}