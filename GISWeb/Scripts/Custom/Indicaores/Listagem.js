﻿jQuery(function ($) {

    Chosen();

    DatePTBR();

    AplicaDateRangePicker();

    if ($("#UKEmpresa").val() != "") {

        $.ajax({
            method: "POST",
            url: "/Questionario/ListarQuestionariosPorEmpresa",
            data: { UKEmpresa: $("#UKEmpresa").val() },
            error: function (erro) {

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {

                if (content.erro != null && content.erro != undefined && content.erro != "") {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    $('#UKQuestionario').empty();

                    $('#UKQuestionario').append($('<option>', {
                        value: "",
                        text: "Selecione um questionário..."
                    }));

                    $("#UKQuestionario").attr("placeholder", "Selecione um questionário...");

                    for (var i = 0; i < content.data.length; i++) {
                        $('#UKQuestionario').append($('<option>', {
                            value: content.data[i].UniqueKey,
                            text: content.data[i].Nome
                        }));
                    }

                    $("#UKQuestionario").trigger("chosen:updated");

                }
            }
        });



        $.ajax({
            method: "POST",
            url: "/Empregado/ListarEmpregadosPorEmpresa",
            data: { UKEmpresa: $("#UKEmpresa").val() },
            error: function (erro) {

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {

                if (content.erro != null && content.erro != undefined && content.erro != "") {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    $('#UKEmpregado').empty();

                    $('#UKEmpregado').append($('<option>', {
                        value: "",
                        text: "Selecione um empregado..."
                    }));

                    $("#UKEmpregado").attr("placeholder", "Selecione um empregado...");

                    for (var i = 0; i < content.data.length; i++) {
                        $('#UKEmpregado').append($('<option>', {
                            value: content.data[i].UniqueKey,
                            text: content.data[i].Nome
                        }));
                    }

                    $("#UKEmpregado").trigger("chosen:updated");

                }
            }
        });
    }

    $("#UKEmpresa").off("change").on("change", function () {


        if ($(this).val() == "") {
            $('#UKQuestionario').empty();

            $('#UKQuestionario').append($('<option>', {
                value: "",
                text: "Selecione antes uma empresa..."
            }));

            $("#UKQuestionario").trigger("chosen:updated");
        }
        else {

            $.ajax({
                method: "POST",
                url: "/Questionario/ListarQuestionariosPorEmpresa",
                data: { UKEmpresa: $(this).val() },
                error: function(erro) {
                    
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                },
                success: function(content) {
                    
                    if (content.erro != null && content.erro != undefined && content.erro != "") {
                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    }
                    else {
                        $('#UKQuestionario').empty();

                        $('#UKQuestionario').append($('<option>', {
                            value: "",
                            text: "Selecione um questionário..."
                        }));

                        $("#UKQuestionario").attr("placeholder", "Selecione um questionário...");

                        for (var i = 0; i < content.data.length; i++) {
                            $('#UKQuestionario').append($('<option>', {
                                value: content.data[i].UniqueKey,
                                text: content.data[i].Nome
                            }));
                        }

                        $("#UKQuestionario").trigger("chosen:updated");

                    }
                }
            });

            
            $.ajax({
                method: "POST",
                url: "/Empregado/ListarEmpregadosPorEmpresa",
                data: { UKEmpresa: $(this).val() },
                error: function(erro) {

                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                },
                success: function(content) {

                    if (content.erro != null && content.erro != undefined && content.erro != "") {
                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    }
                    else {
                        $('#UKEmpregado').empty();

                        $('#UKEmpregado').append($('<option>', {
                            value: "",
                            text: "Selecione um empregado..."
                        }));

                        $("#UKEmpregado").attr("placeholder", "Selecione um empregado...");

                        for (var i = 0; i < content.data.length; i++) {
                            $('#UKEmpregado').append($('<option>', {
                                value: content.data[i].UniqueKey,
                                text: content.data[i].Nome
                            }));
                        }

                        $("#UKEmpregado").trigger("chosen:updated");

                    }
                }
            });

        }


    });

    $("#btnLocalizarRespostas").off("click").on("click", function () {
        $("#formPesquisar").submit();
    });

});


function OnBeginPesquisarResposta() {
    $('.page-content-area').ace_ajax('startLoading');
    //$(".LoadingLayout").show();
    //$('#btnSalvar').hide();
    //$("#formPesquisar").css({ opacity: "0.5" });
}

function OnSuccessPesquisarResposta(data) {

    $('.page-content-area').ace_ajax('stopLoading', true);

    if (data.resultado != undefined && data.resultado.Erro != null && data.resultado.Erro != undefined && data.resultado.Erro != "")
    {
        ExibirMensagemDeErro(data.resultado.Erro);
    }
    else {
        $(".resultadoGrafico").html(data);

        AplicaTooltip();
        
        if ($(".dd").length > 0) {
            $('.dd').nestable();
            $('.dd').nestable('collapseAll');
            $($(".collapseOne button")[1]).click();
            $('.dd-handle a').on('mousedown', function (e) {
                e.stopPropagation();
            });
        }
    }
   
    //$("#formPesquisar").removeAttr('style');
    //$(".LoadingLayout").hide();
    //$('#btnSalvar').show();
    //TratarResultadoJSON(data.resultado);

}
