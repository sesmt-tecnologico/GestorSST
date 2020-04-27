jQuery(function ($) {

    Chosen();

    DatePTBR();

    AplicaDateRangePicker();

    $(".btnPesquisar").click(function () {
        $("#formCadastroUsuario").submit();
    });

    $("#CPF").inputmask("999.999.999-99");

    $("#CPF").keydown(function () {
        try {
            $("#CPF").unmask();
        } catch (e) { }

        $("#CPF").inputmask("999.999.999-99");
    });

});




function OnBeginPesquisarUsuario() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastroUsuario").css({ opacity: "0.5" });
}

function OnSuccessPesquisarUsuario(data) {
    $('#formCadastroUsuario').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    if (data.resultado != null && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "") {
        ExibirMensagemDeErro(data.resultado.Erro);
    }
    else {

        $(".resultadoUsuarios").html(data);

        AplicaTooltip();

        if ($("#tableResultadoPesquisa").length > 0) {
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, null, null, { "bSortable": false }], false, 20);
        }
    }

}




function DeletarUsuario(UKUsuario, NomeUsuario) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Usuario/Terminar",
            data: { IDUsuario: UKUsuario },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $(".btnPesquisar").click();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o usuário '" + NomeUsuario + "'?", "Exclusão do Usuário", callback, "btn-danger");
}
