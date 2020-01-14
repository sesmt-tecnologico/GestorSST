//jQuery(function ($) {

//    $("#ddlEmpresa").change(function () {

//        if ($(this).val() != "") {

//            $('#ddlDepartamento').empty();
//            $('#ddlDepartamento').append($('<option></option>').val("").html("Aguarde ..."));
//            $("#ddlDepartamento").attr("disabled", true);

//            $.ajax({
//                method: "POST",
//                url: "/Departamento/ListarDepartamentosPorEmpresa",
//                data: { idEmpresa: $(this).val() },
//                error: function (erro) {
//                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
//                },
//                success: function (content) {
//                    if (content.resultado.length > 0) {
//                        $("#ddlDepartamento").attr("disabled", false);
//                        $('#ddlDepartamento').empty();
//                        $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione um departamento"));
//                        for (var i = 0; i < content.resultado.length; i++) {
//                            $('#ddlDepartamento').append(
//                                $('<option></option>').val(content.resultado[i].IDDepartamento).html(content.resultado[i].Sigla)
//                            );
//                        }
//                    }
//                    else {
//                        $('#ddlDepartamento').empty();
//                        $('#ddlDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
//                    }
//                }
//            });
//        }
//        else {
//            $('#ddlDepartamento').empty();
//            $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
//            $("#ddlDepartamento").attr("disabled", true);
//        }
//    });

//});

function DeletarAtividade(ID, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Atividade/TerminarComRedirect",
            data: { ID: ID, Descricao: Nome },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso !== null && content.resultado.Sucesso !== "") {
                    $("#linha-" + ID).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Documento '" + Nome + "'?", "Exclusão de Documento", callback, "btn-danger");

}





function OnSuccessExcluirDepartamento(data) {
    $('#formExcluirDepartamento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();
    TratarResultadoJSON(data.resultado);
    ExibirMsgGritter(data.resultado);
}

function OnBeginExcluirDepartamento() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formExcluirDepartamento").css({ opacity: "0.5" });
}