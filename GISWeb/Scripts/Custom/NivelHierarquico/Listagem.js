jQuery(function ($) {

    AplicaTooltip();

    AplicajQdataTable("dynamic-table", [null, null, null, { "bSortable": false }], false, 20);

});

function DeletarNivel(IDNivel, Nome) {

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/NivelHierarquico/Terminar",
            data: { id: IDNivel },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {

                    $(".conteudoAjax").html(content.resultado.Conteudo);
                    if ($("#dynamic-table").length > 0) {
                        AplicajQdataTable("dynamic-table", [null, null, null, { "bSortable": false }], false, 20);
                    }
                }

            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a empresa '" + Nome + "'?", "Exclusão de Empresa", callback, "btn-danger");

}