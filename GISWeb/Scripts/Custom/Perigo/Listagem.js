jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },  null, { "bSortable": false }], false, 20);

});


function DeletarPerigo(ID, Descricao) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Perigo/Terminar",
            data: { ID: ID },
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
                    $("#linha-" + ID).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Perigo '" + Descricao + "'?", "Exclusão de Perigo", callback, "btn-danger");

}