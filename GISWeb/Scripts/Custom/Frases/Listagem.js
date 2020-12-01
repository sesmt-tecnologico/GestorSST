jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null,null, { "bSortable": false }], false, 20);

    
});


function GetFrases() {
       

              // Recarrega a página atual sem usar o cache
              document.location.reload(true);
         

}






function AtivarFrase(UKFrase) {

    $('.LoadingLayout').show();
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/FrasesSeguranca/Ativar",
        data: { id: UKFrase },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.LoadingLayout').hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);

            GetFrases();
        }
    });

}

function DesativarFrase(UKFrase) {

    $('.LoadingLayout').show();
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/FrasesSeguranca/Desativar",
        data: { id: UKFrase },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.LoadingLayout').hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);

            GetFrases();
        }
    });

}

