jQuery(function ($) {

    Chosen();
   


});

function OnBeginCadastrarWorkArea() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarWorkArea(data) {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
