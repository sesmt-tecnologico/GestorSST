
function OnBeginAtualizarLink() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessAtualizarLink(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    TratarResultadoJSON(data.resultado);
}
