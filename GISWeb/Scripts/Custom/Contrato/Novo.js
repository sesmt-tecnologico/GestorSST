jQuery(function ($) {

    DatePTBR();

    

    var date = new Date();
    date.setDate(date.getDate());

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR',
        maxDate: date
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });



    $('#HoraIncidente').timepicker({
        minuteStep: 1,
        showSeconds: false,
        showMeridian: false,
        disableFocus: true,
        icons: {
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down'
        }
    }).on('focus', function () {
        $('#HoraIncidente').timepicker('showWidget');
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    $("#HoraIncidente").val("");

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