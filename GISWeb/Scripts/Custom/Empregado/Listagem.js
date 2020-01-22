jQuery(function ($) {

    $('#CPF').mask('999.999.999-99');

    Chosen();

    $(".btnPesquisar").off("click").on("click", function () {
        $("#formPesquisaEmpregado").submit();
    });




});


function OnBeginPesquisaEmpregado() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnFailurePesquisaEmpregado() {
    $('.page-content-area').ace_ajax('stopLoading', true);
}

function OnSuccessPesquisaEmpregado(content) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    if (content.resultado != null && content.resultado.erro != null && content.resultado.erro != undefined && content.resultado.erro != "") {
        ExibirMensagemDeErro(content.resultado.erro);
    }
    else {
        $("#PesquisaEmpregado").hide();
        $(".ResultadoPesquisa").html(content);
        $(".ResultadoPesquisa").show();

        $(".btnVoltar").off("click").on("click", function () {
            $(".ResultadoPesquisa").hide();
            $(".ResultadoPesquisa").html("");
            $("#PesquisaEmpregado").show();
        });

        if ($("#tableResultadoPesquisa").length > 0) {

            AplicaTooltip();
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, null, null], false, 20);

        }
    }
}


//function BuscarDetalhesEstabelecimentoImagens(IDEstabelecimentoImagens) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/EstabelecimentoImagens/BuscarDetalhesEstabelecimentoImagens",
//        data: { idEstabelecimento: IDEstabelecimentoImagens },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Detalhes do Estabelecimento</span>",
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

//function BuscarDetalhesDosRiscos(IDEstabelecimentoImagens) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/AtividadesDoEstabelecimento/BuscarDetalhesDosRiscos",
//        data: { idEstabelecimento: IDEstabelecimentoImagens },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Detalhes do Ambiente</span>",
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

//function BuscarDetalhesDaAtividadeRisco(IDAtividadeaRiscos) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/TipoDeRisco/BuscarDetalhesDaAtividadeRisco",
//        data: { idTipoDeRisco: IDAtividadeaRiscos },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Riscos da Função</span>",
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

//function BuscarDetalhesDeMedidasDeControleEstabelecimento(IDAtividadesDoEstabelecimento) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/MedidasDeControle/BuscarDetalhesDeMedidasDeControleEstabelecimento",
//        data: { IDAtividadesDoEstabelecimento: IDAtividadesDoEstabelecimento },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Controles</span>",
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





//function BuscarDetalhesDeMedidasDeControle(IDEstabelecimentoImagens) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/MedidasDeControle/BuscarDetalhesDeMedidasDeControle",
//        data: { idEstabelecimento: IDEstabelecimentoImagens },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Detalhes do Ambiente</span>",
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

//function BuscarDetalhesEmpregado(idEmpregado) {

//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/Empregado/BuscarDetalhesEmpregado",
//        data: { idEmpregado: idEmpregado },
//        error: function (erro) {
//            $(".LoadingLayout").hide();
//            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//        },
//        success: function (content) {
//            $(".LoadingLayout").hide();

//            if (content.data != null) {
//                bootbox.dialog({
//                    message: content.data,
//                    title: "<span class='bigger-110'>Detalhes do Empregado</span>",
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

    


//    function BuscarDetalhesDeMedidasDeControle(IDMedidasDeControleRiscoFuncao) {

//        $(".LoadingLayout").show();

//        $.ajax({
//            method: "POST",
//            url: "/Empregado/BuscarDetalhesDeMedidasDeControle",
//            data: { IDMedidasDeControleRiscoFuncao: IDMedidasDeControleRiscoFuncao },
//            error: function (erro) {
//                $(".LoadingLayout").hide();
//                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//            },
//            success: function (content) {
//                $(".LoadingLayout").hide();

//                if (content.data != null) {
//                    bootbox.dialog({
//                        message: content.data,
//                        title: "<span class='bigger-110'>Medidas de Controle dos Riscos da Função</span>",
//                        backdrop: true,
//                        locale: "br",
//                        buttons: {},
//                        onEscape: true
//                    });
//                }
//                else {
//                    TratarResultadoJSON(content.resultado);
//                }

//            }
//        });


//    }


//function BuscarDetalhesEmpresa(IDEmpresa) {
    
//    $(".LoadingLayout").show();

//    $.ajax({
//        method: "POST",
//        url: "/Estabelecimento/BuscarEstabelecimentoPorID",
//        data: { idEstabelecimento: IDEstabelecimento },
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

//function DeletarEmpresa(IDEmpresa, Nome) {
    
//    var callback = function () {
//        $('.LoadingLayout').show();
//        $('#dynamic-table').css({ opacity: "0.5" });

//        $.ajax({
//            method: "POST",
//            url: "/Empresa/Terminar",
//            data: { IDEmpresa: IDEmpresa },
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
//                    $("#linha-" + IDEmpresa).remove();
//                }
//            }
//        });
//    };

//    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a empresa '" + Nome + "'?", "Exclusão de Empresa", callback, "btn-danger");

//}