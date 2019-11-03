jQuery(function ($) {

    BuscarListagemDeRiscos();

});

function BuscarListagemDeRiscos() {
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Risco/BuscarRiscos",
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
                AplicajQdataTable("tableRiscos", [null, null, null, { "bSortable": false }], false, 20);
            }
        }
    });
}


function ExcluirRisco(id, Nome) {

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Risco/Terminar",
            data: { id: id },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    BuscarListagemDeRiscos();                    
                }

            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o risco '" + Nome + "'?", "Exclusão de Risco", callback, "btn-danger");

}