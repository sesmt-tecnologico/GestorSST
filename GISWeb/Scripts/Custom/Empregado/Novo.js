AplicaValidacaoCPF();

jQuery(function ($) {
    $('#txtCPF').mask('999.999.999-99');
    $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });
    AplicaDatePicker(false);

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