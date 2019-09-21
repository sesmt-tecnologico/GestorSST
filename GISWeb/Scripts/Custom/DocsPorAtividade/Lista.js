jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null,null, { "bSortable": false }], false, 20);

});

function ListaDocumentos(id) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/DocsPorAtividade/ListaDocumentos",
        data: { IDAtividade: id },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Lista de Documentos</span>",
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





//function ListaDocumentos(id) {

//    $(".loading.layout").show();

//    $.ajax({
//        method: "POST",
//        url: "/DocsPorAtividade/ListaDocumentos",
//        data: { IDAtividade: id },
//        error: function (erro) {
//            $(".loading.layout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');



//        },

//        sucess: function (content) {
//            $(".loading.layout").show();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Lista de Documentos</span>",
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



