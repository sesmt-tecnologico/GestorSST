
function OnBeginCadastrarCargo() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroCargo").css({ opacity: "0.5" });
}

function OnSuccessCadastrarCargo(data) {
    $('#formCadastroCargo').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}