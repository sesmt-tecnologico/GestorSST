
function OnBeginAtualizarWorkArea() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessAtualizarWorkArea(data) {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}