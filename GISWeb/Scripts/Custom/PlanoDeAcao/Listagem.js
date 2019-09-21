jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null,null, null,null,null, { "bSortable": false }], false, 20);

});


function EncerrarPlano(IDplano,IDidentificador) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/PlanoDeAcao/TerminarComRedirect",
            data: { IDplano: IDplano, IDidentificador: IDidentificador },
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
                    $("#linha-" + IDAlocacao).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja encerrar este Plano de Ação?", "Encerrar Plano de Ação", callback, "btn-danger");
};

//function Detalhes(IDplano) {

//    var callback = function () {
//        $('.LoadingLayout').show();
//        $('#dynamic-table').css({ opacity: "0.5" });

//        $.ajax({
//            method: "POST",
//            url: "/PlanoDeAcao/Detalhes",
//            data: { IDplano: IDplano },
//            error: function (erro) {
//                $(".LoadingLayout").hide();
//                $("#dynamic-table").css({ opacity: '' });
//                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//            },
//            success: function (content) {
//                $('.LoadingLayout').hide();
//                $("#dynamic-table").css({ opacity: '' });

//                TratarResultadoJSON(content.resultado);

//                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
//                    $("#linha-" + IDAlocacao).remove();
//                }
//            }
//        });
//    };

    
//};

function Detalhes(IDplano) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/PlanoDeAcao/Detalhes",
        data: { IDPlanoDeAcao: IDplano },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Plano de Ação</span>",
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


