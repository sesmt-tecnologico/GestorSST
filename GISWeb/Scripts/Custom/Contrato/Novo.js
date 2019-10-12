jQuery(function ($) {

    DatePTBR();

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR'
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    Chosen();

});

function OnSuccessCadastrarContrato(data) {
    $('#formCadastroContrato').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginCadastrarContrato() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroContrato").css({ opacity: "0.5" });
}