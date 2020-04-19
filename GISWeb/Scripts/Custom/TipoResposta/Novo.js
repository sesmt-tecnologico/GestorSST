jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

});


function OnBeginCadastrarTipoResposta() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarTipoResposta(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
