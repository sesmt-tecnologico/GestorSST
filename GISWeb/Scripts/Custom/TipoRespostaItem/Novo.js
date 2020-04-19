jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

});

function OnBeginCadastrarTipoRespostaItem() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarTipoRespostaItem(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
