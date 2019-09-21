

function CriarPlanoDeAção(IDIdentificador) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/PlanoDeAcao/CriarPlanoDeAção",
        data: { IDIdentificador: IDIdentificador },
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




function OnSuccessCadastrarTipoDeRisco(data) {
    $('#formCadastroTipoDeRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarTipoDeRisco() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroTipoDeRisco").css({ opacity: "0.5" });
}