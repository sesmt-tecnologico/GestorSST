jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null,null,null,null, { "bSortable": false }], false, 20);

});

function BuscarDetalhesEmpresa(IDTipoDeRisco) {
    
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadeRiscos/BuscarDetalhesDaAtividadeRisco",
        data: { idAtividadeRisco: IDTipoDeRisco },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes da Atividade</span>",
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

function DeletarPerigo(IDAtividadeRiscos, Descricao) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/AtividadeRiscos/Terminar",
            data: { IDAtividadeRiscos: IDAtividadeRiscos },
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
                    $("#linha-" + IDAtividadeRiscos).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Perigo Existente '" + Descricao + "'?", "Exclusão do Perigo Existente", callback, "btn-danger");

}



