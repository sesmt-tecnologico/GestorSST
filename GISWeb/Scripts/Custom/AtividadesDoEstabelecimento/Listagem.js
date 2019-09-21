jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },  null, { "bSortable": false }], false, 20);

});


var scripts = ["/Components/jquery.maskedinput/dist/jquery.maskedinput.js", "/Components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js", "/Components/bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-BR.min.js", "/Components/bootbox.js/bootbox.js"]
$('.page-content-area').ace_ajax('loadScripts', scripts, function () {
    jQuery(function ($) {

        $('#txtCPF').mask('999.999.999-99');
        $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });

        $('.date-picker').datepicker({
            autoclose: true,
            todayhighlight: true,
            language: "pt-BR"
        })
            .next().on(ace.click_event, function () {
                $(this).prev().focus();
            });


    });

});


function Detalhes(IDPlanoDeAcao) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/PlanoDeAcao/Detalhes",
        data: { IDPlanoDeAcao: IDPlanoDeAcao },
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




function BuscarDetalhesEstabelecimentoImagens(IDAtividadesDoEstabelecimento) {
    
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/BuscarDetalhesEstabelecimentoImagens",
        data: { idAtividadesDoEstabelecimento: IDAtividadesDoEstabelecimento },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Riscos da Atividade</span>",
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


function CriarPlanoDeAção(IDAtividadesDoEstabelecimento) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/PlanoDeAcao/CriarPlanoDeAção",
        data: { IDIdentificador: IDAtividadesDoEstabelecimento },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Plano De Ação</span>",
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


function BuscarDetalhesDeMedidasDeControleEstabelecimento(IDTipoDeRisco) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/MedidasDeControle/BuscarDetalhesDeMedidasDeControleEstabelecimento",
        data: { IDTipoDeRisco: IDTipoDeRisco },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Controles</span>",
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




function DeletarEmpresa(IDEmpresa, Nome) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Empresa/Terminar",
            data: { IDEmpresa: IDEmpresa },
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
                    $("#linha-" + IDEmpresa).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a empresa '" + Nome + "'?", "Exclusão de Empresa", callback, "btn-danger");

}

function OnSuccessCadastrarPlanoDeAcao(data) {
    $('#formCadastroPlanoDeAcao').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarPlanoDeAcao() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroPlanoDeAcao").css({ opacity: "0.5" });
}