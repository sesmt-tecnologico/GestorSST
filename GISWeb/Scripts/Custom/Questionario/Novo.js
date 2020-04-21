jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

    AplicaTooltip();

});

function OnBeginCadastrarQuestionario() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarQuestionario(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
