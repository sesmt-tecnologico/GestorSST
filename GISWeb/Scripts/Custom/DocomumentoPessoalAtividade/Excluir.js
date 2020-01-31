

function DeletarDocumento(IDDocumento,Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/DocumentosPessoal/TerminarComRedirect",
            data: { IDDocumentosEmpregado: IDDocumento, NomeDocumento: Nome },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso !== null && content.resultado.Sucesso !== "") {
                    $("#linha-" + IDDocumentosEmpregado).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Documento '" + Nome + "'?", "Exclusão de Documento", callback, "btn-danger");

}