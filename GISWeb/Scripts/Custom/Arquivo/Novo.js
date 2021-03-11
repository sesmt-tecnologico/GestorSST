
       
       




function OnBeginCadastrarCollection() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroCollection").css({ opacity: "0.5" });
}


function OnSuccessCadastrarCollection(data) {
    $('#formCadastroCollection').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}
