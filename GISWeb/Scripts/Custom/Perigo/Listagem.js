jQuery(function ($) {

    BuscarListagemDePerigos();

});

function BuscarListagemDePerigos()
{
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Perigo/BuscarPerigos",
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.resultado != null && content.resultado != undefined) {
                TratarResultadoJSON();
            }
            else {
                $(".conteudoAjax").html(content);

                AplicaTooltip();
                AplicajQdataTable("tablePerigos", [null, null, null, { "bSortable": false }], false, 20);
            }
        }
    });
}

function ExcluirPerigo(id, desc) {

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Perigo/Terminar",
            data: { id: id },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "")
                {
                    BuscarListagemDePerigos();
                }

            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o perigo '" + desc + "'?", "Exclusão de Perigo", callback, "btn-danger");

}