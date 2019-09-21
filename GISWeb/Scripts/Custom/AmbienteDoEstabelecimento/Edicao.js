jQuery(function ($) {

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
                            $('#ddlDepartamento').append(
                                $('<option></option>').val(content.resultado[i].IDDepartamento).html(content.resultado[i].Sigla)
                            );
                        }
                    }
                    else {
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
                    }
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

function OnSuccessCadastrarDepartamento(data) {
    $('#formEdicaoDepartamento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    ExibirMsgGritter(data.resultado);
}

function OnBeginAtualizarDepartamento() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formCadastrarDepartamento").css({ opacity: "0.5" });
}