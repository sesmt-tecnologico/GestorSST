jQuery(function ($) {

    $('#CPF').mask('999.999.999-99');

    Chosen();

    DatePTBR();

    AplicaDateRangePicker();

    $(".btnPesquisar").off("click").on("click", function () {
        $("#formPesquisaEmpregado").submit();
    });

    $("#Cargo").off("change").on("change", function () {
        
        if ($(this).val() == "") {
            $('#Funcao').empty();

            $('#Funcao').append($('<option>', {
                value: "",
                text: "Selecione antes um cargo..."
            }));

            $("#Funcao").trigger("chosen:updated");
        }
        else {
            $('.page-content-area').ace_ajax('startLoading');

            $.ajax({
                method: "POST",
                url: "/Funcao/BuscarFuncoesPorCargoParaSelect",
                data: { id: $(this).val() },
                error: function (erro) {
                    $('.page-content-area').ace_ajax('stopLoading', true);
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                },
                success: function (content) {
                    $('.page-content-area').ace_ajax('stopLoading', true);

                    if (content.erro != null && content.erro != undefined && content.erro != "") {
                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    }
                    else {
                        $('#Funcao').empty();

                        $('#Funcao').append($('<option>', {
                            value: "",
                            text: "Selecione uma função..."
                        }));

                        $("#Funcao").attr("placeholder", "Selecione uma função...");

                        for (var i = 0; i < content.data.length; i++) {
                            $('#Funcao').append($('<option>', {
                                value: content.data[i].UniqueKey,
                                text: content.data[i].NomeDaFuncao
                            }));
                        }

                        $("#Funcao").trigger("chosen:updated");

                    }
                }
            });

        }

    });

});


function OnBeginPesquisaEmpregado() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnFailurePesquisaEmpregado() {
    $('.page-content-area').ace_ajax('stopLoading', true);
}

function OnSuccessPesquisaEmpregado(content) {
    $('.page-content-area').ace_ajax('stopLoading', true);

    if (content.resultado != null && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
        ExibirMensagemDeErro(content.resultado.Erro);
    }
    else {
        $("#PesquisaEmpregado").hide();
        $(".ResultadoPesquisa").html(content);
        $(".ResultadoPesquisa").show();

        $(".btnVoltar").off("click").on("click", function () {
            $(".ResultadoPesquisa").hide();
            $(".ResultadoPesquisa").html("");
            $("#PesquisaEmpregado").show();
        });

        if ($("#tableResultadoPesquisa").length > 0) {

            AplicaTooltip();
            AplicajQdataTable("tableResultadoPesquisa", [{ "bSortable": false }, null, null, null, null, null], false, 20);

        }
    }
}
