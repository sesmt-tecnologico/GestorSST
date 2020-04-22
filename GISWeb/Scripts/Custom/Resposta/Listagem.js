jQuery(function ($) {

    Chosen();

    DatePTBR();

    AplicaDateRangePicker();

    $("#UKEmpresa").off("change").on("change", function () {
        //alert($(this).val());
    });

    $("#btnLocalizarRespostas").off("click").on("click", function () {
        $("#formPesquisaResposta").submit();
    });

});

function OnBeginPesquisarResposta() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessPesquisarResposta(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    if (data.resultado != null && data.resultado != undefined && data.resultado.Erro != null && data.resultado.Erro == undefined && data.resultado.Erro != "") {
        ExibirMensagemDeErro(data.resultado.Erro);
    }
    else {
        $(".resultadoRespostas").html(data);

        if ($(".dd").length > 0) {
            $('.dd').nestable();
            $('.dd').nestable('collapseAll');
            $($(".collapseOne button")[1]).click();
            $('.dd-handle a').on('mousedown', function (e) {
                e.stopPropagation();
            });
        }
    }

}