


function OnSuccessCadastrarDocsPorAtividade(data) {

   
    $('#formCadastroDocsPorAtividade').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    
}



function OnBeginCadastrarDocsPorAtividade() {
    
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroDocsPorAtividade").css({ opacity: "0.5" });
}