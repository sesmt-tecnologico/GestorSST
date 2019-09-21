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

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDPossiveisDanos).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Possivel Dano '" + DescricaoDanos + "'?", "Exclusão do possivel Dano", callback, "btn-danger");

}