

function OnSuccessCadastrarControle(data) {
    $('#formCadastroControle').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);

    if (!(data.resultado.Alerta != null && data.resultado.Alerta != undefined && data.resultado.Alerta != "")) {
        $('#modalAddControle').modal('hide');
    }

}

function OnBeginCadastrarControle() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroControle").css({ opacity: "0.5" });


}