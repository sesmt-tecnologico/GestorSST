jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null,null, { "bSortable": false }], false, 20);

});


function DeletarFuncao(IDFuncao, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Funcao/TerminarComRedirect",
            data: { IDFuncao: IDFuncao },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });
    };



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir esta Função?", "Exclusão de Função", callback, "btn-danger");

    //ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir esta admissao '" + Nome + "'?", "Exclusão de Empresa", callback, "btn-danger");

}

function ListaAtividadePorFuncao(IDFuncao, NomeFuncao)
{

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Atividade/ListaAtividadePorFuncao",
        data: { IDFuncao: IDFuncao, NomeFuncao: NomeFuncao },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Atividades desta Função</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}