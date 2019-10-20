jQuery(function ($) {

    GetDepartments();

});

function GetDepartments() {

    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Departamento/BuscarDepartamentosTodasEmpresa",
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemDeErro(erro.responseText);
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.erro != null && content.erro != undefined && content.erro != "") {
                ExibirMensagemDeErro(content.erro);
            }
            else {
                $("#contentDepartment").html(content);

                AplicaTooltip();

                $('.dd').nestable();
                $('.dd').nestable('collapseAll');
                $($(".collapseOne button")[1]).click();
                $('.dd-handle a').on('mousedown', function (e) {
                    e.stopPropagation();
                });
            }
        }
    });
}

function deleteDepartment(UniqueKey, ShortName) {

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Departamento/Terminar",
            data: { id: UniqueKey },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemDeErro(erro.responseText);
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    ExibirMensagemDeErro(content.resultado.Erro);
                }
                else if (content.resultado.Sucesso != null && content.resultado.Sucesso != undefined && content.resultado.Sucesso != "") {
                    ExibirMensagemDeSucesso(content.resultado.Sucesso);

                    $('.page-content-area').trigger("GetDepartments");
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o departamento '" + ShortName + "'?", "Exclusão de Departamento", callback, "btn-danger");

}