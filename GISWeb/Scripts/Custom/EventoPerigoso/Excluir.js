﻿
function OnBeginExcluirEvento() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirEvento").css({ opacity: "0.5" });
}

function OnSuccessExcluirEvento(data) {
    $('#formExcluirEvento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
}