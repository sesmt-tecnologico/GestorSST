jQuery(function ($) {

    Chosen();

    $("#btnLocalizarEstabelecimentos").off("click").on("click", function () {
        $("#formPesquisarEstabelecimento").submit();
    });

});

function OnBeginPesquisarEstabelecimento() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessPesquisarEstabelecimento(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    if (data.resultado != null && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "") {
      ExibirMensagemDeErro(data.resultado.Erro);
    }
    else {

        $(".resultadoEstabelecimento").html(data);

        if ($("#tableResultadoPesquisa").length > 0) {
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null,null,{ "bSortable": false }], false, 20);
        }
    }

}







function BuscarDetalhesEstabelecimentoImagens(IDEstabelecimentoImagens) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/EstabelecimentoImagens/BuscarDetalhesEstabelecimentoImagens",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Estabelecimento</span>",
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


function PesquisarEstabelecimento(ID) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/EstabelecimentoImagens/PesquisarEstabelecimento",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Estabelecimento</span>",
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

function BuscarDetalhesDosRiscos(IDEstabelecimentoImagens) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/BuscarDetalhesDosRiscos",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Ambiente</span>",
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

function BuscarDetalhesDeMedidasDeControle(IDEstabelecimentoImagens) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/MedidasDeControle/BuscarDetalhesDeMedidasDeControle",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Ambiente</span>",
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




function BuscarDetalhesEmpresa(IDEmpresa) {
    
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Estabelecimento/BuscarEstabelecimentoPorID",
        data: { idEstabelecimento: IDEstabelecimento },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes da Empresa</span>",
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

function DeletarEstabelecimento(IDEstabelecimento, NomeEstabelecimento) {
    
    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Estabelecimento/Terminar",
            data: { id: IDEstabelecimento },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $(".resultadoEstabelecimento").html("");
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Estabelecimento '" + NomeEstabelecimento + "'?", "Exclusão de Estabelecimento", callback, "btn-danger");

}