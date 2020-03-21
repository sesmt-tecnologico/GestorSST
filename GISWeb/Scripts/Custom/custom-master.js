jQuery(function ($) {

    $("#modalAlterarSenhaProsseguir").off("click").on("click", function () {
        $("#formAlterarSenha").submit();
    });

});

function OnSuccessAlterarSenha(content) {
    TratarResultadoJSON(content.resultado);

    if (content.resultado.Sucesso != null && content.resultado.Sucesso != undefined && content.resultado.Sucesso != "") {
        $("#modalAlterarSenha").modal("hide");
    }

}