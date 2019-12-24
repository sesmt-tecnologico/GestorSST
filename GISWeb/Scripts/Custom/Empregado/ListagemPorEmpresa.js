

function OnBeginPesquisaEmpregado() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisaEmpregado").css({ opacity: "0.5" });
}

function OnSuccessPesquisaEmpregado(data) {
    $('#formPesquisaEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    if (data.resultado != null && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "") {
        ExibirMensagemDeErro(resultado.Erro);
    }
    else {

        $(".ResultadoPesquisa").html(data);

        if ($("#tableResultadoPesquisa").length > 0) {
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, { "bSortable": false }], false, 20);
        }
    }

}