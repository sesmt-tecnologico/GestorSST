jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

});

function OnSuccessAtualizarNivel(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarNivel() {
    $('.page-content-area').ace_ajax('startLoading');
}