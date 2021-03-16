jQuery(function ($) {

    $(".btnMov").click(function () {
        //var veiculo = $("#idVeiculo").val();
        //var frota = $("#idFrota").val();

        $.ajax({
            method: "POST",
            url: "/MovimentacaoVeicular/Novo",
            //data: { veiculo: veiculo, frota: frota },
            error: function (erro) {                
                ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
            },
            success: function (content) {
               
                Chosen();

               
            }
        });
            
    });

   

});


function OnBeginCadastrarPesquisa() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroMovimentacaoPesquisa").css({ opacity: "0.5" });
}


function OnSuccessCadastrarMovimentacaoPesquisa(data) {
    $('#formCadastroMovimentacaoPesquisa').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}








function OnBeginCadastrarAtividade() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroMovimentacaoVeicular").css({ opacity: "0.5" });
}


function OnSuccessCadastrarMovimentacaoVeicular(data) {
    $('#formCadastroMovimentacaoVeicular').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}


function OnBeginAtualizarKM() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEdicaoKM").css({ opacity: "0.5" });
}


function OnSuccessAtualizarKM(data) {
    $('#formEdicaoKM').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}
