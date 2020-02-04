

function OnBeginPesquisarWorkArea() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisarWorkArea").css({ opacity: "0.5" });
}

function OnSuccessPesquisarWorkarea(data) {
    $('#formPesquisarWorkArea').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    $(".resultadoWorkArea").html(data);
}








function OnClickControleDoRisco(pEstabelecimento, pUKWorkArea, pRisco) {
    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDorisco/CriarControle",
        data: { UKEstabelecimento: pEstabelecimento, UKWorkArea: pUKWorkArea, UKRisco: pRisco  },
        error: function (erro) {
            $("#modalAddControleLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddControleLoading").hide();
            $("#modalAddControleCorpo").html(content);           

            $("#modalAddControleProsseguir").off("click").on("click", function () {
                var ukEst = $.trim($(".txtUKEstabelecimento").val());
                var ukWA = $.trim($(".txtUKWorkArea").val());
                var ukRisc = $.trim($(".txtUKRisco").val());

                if (ukEst == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a o Estabelecimento.");
                }

                if (ukWA == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação da work area.");
                }
                else if (ukRisc == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar Risco.");
                }
                else {
                    $("#modalAddControleLoading").show();
                    
                    $.ajax({
                        method: "POST",
                        url: "/ReconhecimentoDorisco/CadastrarControleDeRisco",
                        data: { UKEstabelecimento: ukEst, UKWorkArea: ukWA, UKRisco: ukRisc  },
                        error: function (erro) {
                            $("#modalAddControleLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalAddControleLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            if (content.resultado.Sucesso != "") {
                                $(".resultadoWorkArea").html("");

                                if ($("#UKEstabelecimento").val() != "") {
                                    $("#formPesquisarWorkArea").submit();
                                }

                                $('#modalAddControle').modal('hide');
                            }




                        }
                    }
                    );
                }
                

            });
        }
    });
}
