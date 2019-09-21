




function BuscarDetalhesDeMedidasDeControleTipoRisco(IDTipoDeRisco) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AnaliseRisco/BuscarDetalhesDeMedidasDeControleTipoRisco",
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
                    title: "<span class='bigger-110'>Controles deste Risco</span>",
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








function OnSuccessCadastrarAnaliseRisco(data) {

    $('#formCadastroAnaliseRisco').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarAnaliseRisco() {


    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroAnaliseRisco").css({ opacity: "0.5" });

    

}


