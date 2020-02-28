//AplicaValidacaoCPF();
jQuery(function ($) {

    $('#txtCPF').mask('999.999.999-99');
    $('#DataNascimento').mask('99/99/9999');

});

function OnSuccessCadastrarEmpregado(data) {
    $('#formCadastroEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarEmpregado() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });
}

