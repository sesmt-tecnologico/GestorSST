jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [null, null, null, { "bSortable": false }], false, 20);

});

function DeletarPossiveisDanos(IDPossiveisDanos, DescricaoDanos) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/PossiveisDanos/Terminar",
            data: { IDPossiveisDanos: IDPossiveisDanos },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Possivel Dano '" + DescricaoDanos + "'?", "Exclusão do possivel Dano", callback, "btn-danger");

}