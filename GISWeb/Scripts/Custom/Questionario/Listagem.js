jQuery(function ($) {

    GetQuestionarios();

});

function GetQuestionarios() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDepartment").html("");

    $.ajax({
        method: "POST",
        url: "/Questionario/BuscarQuestionarios",
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
                $("#contentQuestionario").html(content);

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

function deleteQuestionario(UKQuestionario, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Questionario/Terminar",
            data: { id: UKQuestionario },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                GetQuestionarios();
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o questionário '" + Nome + "'?", "Exclusão de questionário", callback, "btn-danger");

}

function deletePergunta(UKPergunta, Ordem) {
    var callback = function () {
        $('.LoadingLayout').show();
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Pergunta/Terminar",
            data: { id: UKPergunta },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                GetQuestionarios();
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a pergunta '" + Ordem + "'?", "Exclusão de pergunta", callback, "btn-danger");
}

function AtivarQuestionario(UKQuestionario) {
    
    $('.LoadingLayout').show();
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/Ativar",
        data: { id: UKQuestionario },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.LoadingLayout').hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);

            GetQuestionarios();
        }
    });

}

function DesativarQuestionario(UKQuestionario) {

    $('.LoadingLayout').show();
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/Desativar",
        data: { id: UKQuestionario },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.LoadingLayout').hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);

            GetQuestionarios();
        }
    });

}