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

    if (data.resultado != null && data.resultado.Erro != null &&
        data.resultado.Erro != undefined && data.resultado.Erro != "") {

        ExibirMensagemDeErro(resultado.Erro);
    }
    else {
        $(".resultadoWorkArea").html(data);
        AplicaTooltip();
    }
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