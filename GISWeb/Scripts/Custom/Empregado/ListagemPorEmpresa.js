
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

//function OnBeginPesquisaEmpregado() {
//    $(".LoadingLayout").show();
//    $('.page-content-area').ace_ajax('startLoading');
//    $("#ResultadoPesquisa").html("");
//    $("#formPesquisaEmpregado").show();
//}

//function OnSuccessPesquisaEmpregado(data) {
//    if (data.resultado != null &&
//        data.resultado != undefined &&
//        data.resultado.Erro != null &&
//        data.resultado.Erro != undefined &&
//        data.resultado.Erro != "") {

//        $('.page-content-area').ace_ajax('stopLoading', true);
//        $(".LoadingLayout").hide();
//        ExibirMensagemDeAlerta(data.resultado.Erro);
//    }
//    else {

//        $(".LoadingLayout").hide();
//        $("#formPesquisaEmpregado").hide();
//        $('.page-content-area').ace_ajax('stopLoading', true);
//        $("#ResultadoPesquisa").html(data);

//        AplicaTooltip();

//        if ($("#divTableResultadoPesquisa").length > 0) {
//            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, null, null, null, null, null, { "bSortable": false }], false, 20);

//            $('.btnDropdownMenu').off("click").on('click', function () {
//                $(this).closest('tr').css({ 'background-color': '#dff0d8' });

//                OnClickBtnDropdownMenu($(this));
//            });
//        }

//        $(".btnVoltarPesquisaBase").off("click").on("click", function () {
//            $("#ResultadoPesquisa").html("");
//            $("#formPesquisaEmpregado").show();
//        });
//    }
//}

//function OnFailurePesquisaEmpregado() {
//    $(".LoadingLayout").hide();
//    $('.page-content-area').ace_ajax('stopLoading', true);
//    $("#formPesquisaEmpregado").show();
//    $("#ResultadoPesquisa").html("");
//}









