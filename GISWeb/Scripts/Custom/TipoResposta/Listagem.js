﻿jQuery(function ($) {

    GetTiposDeResposta();

});

function GetTiposDeResposta() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDepartment").html("");

    $.ajax({
        method: "POST",
        url: "/TipoResposta/BuscarTiposDeResposta",
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
                $("#contentTipoResposta").html(content);

                AplicaTooltip();

                if ($('.dd').length > 0) {
                    $('.dd').nestable();
                    $('.dd').nestable('collapseAll');
                    $($(".collapseOne button")[1]).click();
                    $('.dd-handle a').on('mousedown', function (e) {
                        e.stopPropagation();
                    });
                }

            }
        }
    });

}

function deleteTipoResposta(UKTipoResposta, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/TipoResposta/Terminar",
            data: { id: UKTipoResposta },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                GetTiposDeResposta();
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a resposta '" + Nome + "'?", "Exclusão de resposta (múltipla escolha)", callback, "btn-danger");

}

function deleteItemResposta(UKTipoRespostaItem, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/TipoRespostaItem/Terminar",
            data: { id: UKTipoRespostaItem },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                GetTiposDeResposta();
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o item da resposta '" + Nome + "'?", "Exclusão de item de resposta (múltipla escolha)", callback, "btn-danger");

}