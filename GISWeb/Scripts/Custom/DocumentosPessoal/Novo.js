
jQuery(function ($) {

    alert("a00");

});


function OnSuccessCadastrarDocumento(data) {

    alert("A");
    $('#formCadastroDocumento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
    
}



function OnBeginCadastrarDocumento() {
    alert("B");
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroDocumento").css({ opacity: "0.5" });
}