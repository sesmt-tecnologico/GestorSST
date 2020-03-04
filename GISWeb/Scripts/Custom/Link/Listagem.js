jQuery(function ($)
{
    BuscarListagemDeLinks();
});

function BuscarListagemDeLinks() {

    $('.page-content-area').ace_ajax('startLoading');
    $(".conteudoAjax").html("");

    $.ajax({
        method: "POST",
        url: "/Link/ListarLinks",
        data: { },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.resultado != null) {
                TratarResultadoJSON(content.resultado);
            }
            else {
                $(".conteudoAjax").html(content);

                AplicaTooltip();

                AplicajQdataTable("dynamic-table", [null, null, { "bSortable": false }], false, 20);
            }

        }
    });

}

function DeletarLink(UKLink, NomeLink) {

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Link/Terminar",
            data: { UK: UKLink },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    BuscarListagemDeLinks();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o link '" + NomeLink + "'?", "Exclusão de Link", callback, "btn-danger");


}