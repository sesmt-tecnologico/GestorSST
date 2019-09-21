jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null,null,{ "bSortable": false }], false, 20);

});


function DeletarDiretoria(IDDiretoria, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Diretoria/TerminarComRedirect",
            data: { IDDiretoria: IDDiretoria },
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



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir esta Diretoria?", "Exclusão de Diretoria", callback, "btn-danger");

   
}




function DepartamentoDiretoria(IDDiretoria, Sigla) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Departamento/DepartamentoDiretoria",
        data: { IDDiretoria: IDDiretoria, Sigla: Sigla },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Departamentos desta Diretoria</span>",
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