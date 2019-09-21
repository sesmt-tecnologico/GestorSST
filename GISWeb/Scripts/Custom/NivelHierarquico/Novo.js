jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

});


function OnBeginCadastrarNivel() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarNivel(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
