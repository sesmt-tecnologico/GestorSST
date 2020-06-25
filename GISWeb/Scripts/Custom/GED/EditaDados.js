
jQuery(function ($) {

    $('#txtCPF').mask('999.999.999-99');
    $('#txtUKdate').mask('99/99/9999');

});




//setInterval(function () {

//    BuscarTotalDocsInbox();



//}, 3000);

//setInterval(function () {

//    function BuscarTotalDocsInbox() {

//        $.ajax({
//            url: '/Inbox/BuscarTotalDocsInbox',
//            global: false,
//            type: 'POST',
//            async: true,
//            success: function (data) {

//                $(".liNavbarInboxBadgeTotalPessoal").html(data.resultado.Total);

//                //$(".totalDocsPessoal").html(data.resultado.Pessoal + data.resultado.PessoalVeiculo);
//                //$(".totalDocsGrupos").html(data.resultado.Grupos + data.resultado.GruposVeiculo);

//                $("#liNavbarInbox").show();
//            }
//        });

//    }
//}, 3000);




function onClickbtnAlterarValor(UKDoc) {

    

    $.ajax({
        method: "POST",
        url: "/DocumentoAlocacao/Editar",
        data: { UK: UKDoc },
        error: function (erro) {
            $('#modalDataX').show();
            $('#modalFechar').removeClass('disabled');
            $('#modalDataFechar').removeAttr('disabled');
            $('#modalDataProsseguir').hide();
            $('#modalDataCorpo').html('');
            $('#modalDataCorpoLoading').show();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {

            $('#modalDataCorpoLoading').hide();
            $('#modalDataCorpo').html(content);

            $('#modalDataLoading').hide();

            $("#modalDataProsseguir").off("click").on("click", function () {
                var UKDate = $.trim($(".txtUKdate").val());
                var UKDoc = $.trim($(".txtUk").val());
                

                if (UKDoc == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar o registro.");
                }
                else if (UKDate == "") {
                    ExibirMensagemDeAlerta("Favor inserir a Data.");
                }
                else {
                    $("#modalDataLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/DocumentoAlocacao/EditarData",
                        data: { UK: UKDoc, data: UKDate},
                        error: function (erro) {
                            $("#modalDataLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalDataLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            $('#modalDataAnalise').modal('hide');

                            //if (content.resultado.Sucesso != "") {
                            //    $(".resultadoWorkArea").html("");

                            //    if ($("#UKEstabelecimento").val() != "") {
                            //        $("#formPesquisarWorkArea").submit();
                            //    }

                            //    $('#modalAddPerigo').modal('hide');
                            //}




                        }
                    }
                    );
                }


            });

        }
    });

}
