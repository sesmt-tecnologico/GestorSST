jQuery(function ($) {

    $('#txtCNPJ').mask('99.999.999/9999-99');

});

function OnSuccessCadastrarEmpresa(data) {
    $('#formCadastroEmpresa').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarEmpresa() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEmpresa").css({ opacity: "0.5" });
}