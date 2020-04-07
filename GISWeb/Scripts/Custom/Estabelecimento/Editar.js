function OnBeginAtualizarEstabelecimento() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarEstabelecimento(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    
    TratarResultadoJSON(data.resultado);
}