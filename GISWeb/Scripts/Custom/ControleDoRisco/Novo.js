



function OnSuccessCadastrarControle(data) {
    $('#formCadastroControle').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();   
    TratarResultadoJSON(data.resultado);
    $('#modalAddControle').modal('hide');
}

function OnBeginCadastrarControle() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroControle").css({ opacity: "0.5" });
    

}