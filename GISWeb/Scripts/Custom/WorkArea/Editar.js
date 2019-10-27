//jQuery(function ($) {

  

//});

function OnSuccessCadastrarWorkArea(data) {
    $('#formEdicaoWorkArea').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    ExibirMsgGritter(data.resultado);
}

function OnBeginAtualizarWorkArea() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formEdicaoWorkArea").css({ opacity: "0.5" });
}