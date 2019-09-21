jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },  null, { "bSortable": false }], false, 20);

});

//function BuscarDetalhesEmpresa(IDEmpresa) {
    
//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/Empresa/BuscarEmpresaPorID",
//        data: { idEmpresa: IDEmpresa },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();
            
//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Detalhes da Empresa</span>",
//                    backdrop: true,
//                    locale: "br",
//                    buttons: {},
//                    onEscape: true
//                });
//            }
//            else {
//                TratarResultadoJSON(content.resultado);
//            }

//        }
//    });

//}

function DeletarEventoPerigoso(IDEventoPerigoso, Descricao) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/EventoPerigoso/Terminar",
            data: { IDEventoPerigoso: IDEventoPerigoso },
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
                    $("#linha-" + IDEventoPerigoso).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Evento '" + Descricao + "'?", "Exclusão de Evento Perigoso", callback, "btn-danger");

}