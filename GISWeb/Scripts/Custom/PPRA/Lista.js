jQuery(function ($) {

   
    $(".waPesquisar").change(function () {
        if ($("#Estabelecimento").val() != "") {
            $("#formPesquisaPPRA").submit();
        }
        else {
            $(".resultadoPPRA").html("");
        }
    });


});


function OnBeginPesquisaPPRA() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisaPPRA").css({ opacity: "0.5" });
}

function OnSuccessPesquisaPPRA(data) {
    $('#formPesquisaPPRA').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    $(".resultadoPPRA").html(data);
    AplicaTooltip();

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null,  { "bSortable": false }], false, 20);


}


           