
//jQuery(function ($) {

   // AplicajQdataTable("dynamic-table", [null, null, null, null, null, { "bSortable": false }], false, 20);

//});
function SalvarAtividade(Acao, idAtividade, idAlocacao,IDFuncao)
{
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $.post('/AtividadeFuncaoLiberada/SalvarAtividade',
        {

            Acao: Acao,
            idAtividade: idAtividade,
            idAlocacao: idAlocacao,
            IDFuncao: IDFuncao
        }, function (partial) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            $(".LoadingLayout").hide();
            alert("Item salvo!");
        }       
    );
    
   
}

//function OnSuccessCadastrarAtividadeAlocada(data) {
//    $('#formCadastroAtividadeAlocada').removeAttr('style');
//    $(".LoadingLayout").hide();
//    $('#btnSalvar').show();
//    TratarResultadoJSON(data.resultado);
//}

//function OnBeginCadastrarAtividadeAlocada() {
//    $(".LoadingLayout").show();
//    $('#btnSalvar').hide();
//    $("#formCadastroAtividadeAlocada").css({ opacity: "0.5" });
//}