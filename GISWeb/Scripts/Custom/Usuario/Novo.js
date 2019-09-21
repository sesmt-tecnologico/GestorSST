AplicaValidacaoCPF();

jQuery(function ($) {

    $("#txtCPF").inputmask("999.999.999-99");
    
    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");
    });

    Chosen();

    $.validator.unobtrusive.parse(document);

    AplicaValidacaoCPF();

    $("#ddlEmpresa").change(function () {

        if ($(this).val() != "") {

            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Aguarde ..."));
            $("#ddlDepartamento").attr("disabled", true);

            $.ajax({
                method: "POST",
                url: "/Departamento/ListarDepartamentosPorEmpresa",
                data: { idEmpresa: $(this).val() },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {

                    if (content.resultado.length > 0) {
                        $("#ddlDepartamento").attr("disabled", false);
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione um departamento"));
                        for (var i = 0; i < content.resultado.length; i++) {
                            alert(content.resultado[i].Sigla);

                            $('#ddlDepartamento').append(
                                $('<option></option>').val(content.resultado[i].IDDepartamento).html(content.resultado[i].Sigla)
                            );
                        }
                    }
                    else {
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
                    }

                    $("#ddlDepartamento").trigger("chosen:updated");

                }
            });
        }
        else {
            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
            $("#ddlDepartamento").attr("disabled", true);
        }
    });

});


function OnSuccessCadastrarUsuario(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarUsuario() {
    $('.page-content-area').ace_ajax('startLoading');
}