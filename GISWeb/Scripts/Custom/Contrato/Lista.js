jQuery(function ($) {

    Chosen();

    DatePTBR();

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR'
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    $(".btnPesquisar").click(function () {
        $("#formCadastroContrato").submit();
    });

});


function OnBeginPesquisarContrato() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroContrato").css({ opacity: "0.5" });
}

function OnSuccessPesquisarContrato(data) {
    $('#formCadastroContrato').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    if (data.resultado != null && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "") {
        ExibirMensagemDeErro(data.resultado.Erro);
    }
    else {
        
        $(".resultadoContratos").html(data);

        if ($("#tableResultadoPesquisa").length > 0) {
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, null, null, null, { "bSortable": false }], false, 20);
        }
    }

}

function BuscarDetalhesContrato(UK) {
    alert(UK);
}
