jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null, null,null,null, { "bSortable": false }], false, 20);


    $(".btnPesquisar").click(function () {
        $("#formPesquisarWorkArea").submit();
    });


});

function OnBeginPesquisarWorkArea() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisarWorkArea").css({ opacity: "0.5" });
}

function OnSuccessPesquisarWorkarea(data) {
    $('#formPesquisarWorkArea').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    //if (data.resultado != null && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "") {
    //    ExibirMensagemDeErro(resultado.Erro);
    //}
    //else {

    $(".resultadoWorkArea").html(data);

    //if ($("#tableResultadoPesquisa").length > 0) {
    //    AplicajQdataTable("tableResultadoPesquisa", [null, { "bSortable": false }], false, 20);
    //}
    //}
}




function BuscarDetalhesEstabelecimentoImagens(IDEstabelecimentoImagens) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/EstabelecimentoImagens/BuscarDetalhesEstabelecimentoImagens",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
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



function OnClickVincularPerigo() {

}

function OnClickNovoRisco(UKWorkArea) {

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
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Estabelecimento/Terminar",
            data: { id: IDEstabelecimento },
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
                    $("#linha-" + IDEstabelecimento).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Estabelecimento '" + NomeEstabelecimento + "'?", "Exclusão de Estabelecimento", callback, "btn-danger");

}