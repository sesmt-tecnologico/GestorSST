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

function OnBeginAtualizarContrato() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEdicaoContrato").css({ opacity: "0.5" });
}

function OnSuccessAtualizarContrato(data) {
    $('#formEdicaoContrato').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}