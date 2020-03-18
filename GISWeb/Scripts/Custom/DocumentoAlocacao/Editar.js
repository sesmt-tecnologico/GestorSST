


jQuery(function () {
    jQuery('#btnSalvar').on('click', function () {
        var $this = $(this);//o proprio parágrafo

        $this.parents('form').prop('disabled', true);
    });
});

function OnSuccessCadastrarDat(data) {

    $('#numero').removeAttr('style');
    $(".LoadingLayout").hide();
    //$('#btnSalvar').show();    
    //TratarResultadoJSON(data.resultado);
    $('#UKdate').prop("disabled", true);
    $('#btnSalvar').prop("disabled", true);
    alert("Enviado com sucesso"); 
    
    $(function () {
        var numero = $.trim($('#numero').val());
        var td = $.trim($('#form').val());

        if (numero === td) {
            $('#form').css('background-color', 'Grey');
        }
    });
    
    
    
}
function OnBeginCadastrarDat() {

    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#numero").css({ opacity: "0.5" });
    
}


