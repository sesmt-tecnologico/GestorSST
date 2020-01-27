jQuery(function ($) {


    GetDocumento();


});

function GetDocumento() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDocumento").html("");

    $.ajax({
        method: "POST",
        url: "/Atividade/ListaDocumentoPessoal",
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemDeErro(erro.responseText);
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.erro != null && content.erro != undefined && content.erro != "") {
                ExibirMensagemDeErro(content.erro);
            }
            else {
                $("#contentDocumento").html(content);

                AplicaTooltip();

                $('.dd').nestable();
                $('.dd').nestable('collapseAll');
                $($(".collapseOne button")[1]).click();
                $('.dd-handle a').on('mousedown', function (e) {
                    e.stopPropagation();
                });
            }
        }
    });
}








