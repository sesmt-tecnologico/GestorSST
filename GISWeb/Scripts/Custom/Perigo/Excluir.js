
function OnBeginExcluirPerigo() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirPerigo").css({ opacity: "0.5" });
}

function OnSuccessExcluirPerigo(data) {
    $('#formExcluirPerigo').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}