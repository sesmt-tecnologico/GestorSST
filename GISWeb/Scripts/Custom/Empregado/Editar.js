
function OnBeginAtualizarEmpregado() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEdicaoEmpregado").css({ opacity: "0.5" });
}

function OnSuccessCadastrarEmpregado(data) {
    $('#formEdicaoEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
}