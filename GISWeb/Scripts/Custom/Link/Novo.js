
function OnBeginCadastrarLink() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarLink(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    TratarResultadoJSON(data.resultado);
}