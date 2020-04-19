jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

    $("#TipoResposta").off("change").on("change", function () {
        if ($("#TipoResposta").val() == 3) {
            $("#UKTipoResposta").attr("disabled", false);
        }
        else {
            $("#UKTipoResposta").val("");
            $("#UKTipoResposta").attr("disabled", true);
        }
    });

});

function OnBeginCadastrarPergunta() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarPergunta(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}
