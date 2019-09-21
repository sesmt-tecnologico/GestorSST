
//jQuery(function ($) {

   // AplicajQdataTable("dynamic-table", [null, null, null, null, null, { "bSortable": false }], false, 20);

//});
function SalvarAtividadeEstabelecimento(Acao, idAtividadeEstabelecimento, idAlocacao)
{
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $.post('/AtividadeAlocada/SalvarAtividadeEstabelecimento',
        {

            Acao: Acao,
            idAtividadeEstabelecimento: idAtividadeEstabelecimento,
            idAlocacao: idAlocacao
        }, function (partial) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            $(".LoadingLayout").hide();
            alert("Item salvo!");
        }       
    );
    
   
}

function SalvarAnaliseRisco(Acao, idAtividadeEstabelecimento, idAlocacao,bemEstar,conhecimento)
{
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $.post('/AnaliseRisco/SalvarAnaliseRisco',
        {

            Acao: Acao,
            idAtividadeEstabelecimento: idAtividadeEstabelecimento,
            idAlocacao: idAlocacao,
            BemEstar: bemEstar,
            Conhecimento: conhecimento
        }, function (partial) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            $(".LoadingLayout").hide();
            alert("Item salvo!");
        }
    );


}








function OnSuccessCadastrarAtividadeAlocada(data) {
    $('#formCadastroAtividadeAlocada').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarAtividadeAlocada() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroAtividadeAlocada").css({ opacity: "0.5" });
}