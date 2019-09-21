jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null, { "bSortable": false }], false, 20);

});

function DeletarContrato(IDContrato, NumeroContrato) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Contrato/Terminar",
            data: { IDContrato: IDContrato },
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
                    $("#linha-" + IDContrato).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Contrato '" + NumeroContrato + "'?", "Exclusão do Contrato", callback, "btn-danger");
}