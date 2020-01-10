jQuery(function ($) {


    $(".btnNewDepartment").on("click", function () {
        return false;
    });

    $("#ddlComapny").change(function () {
        $("#contentDepartment").html("");

        if ($.trim($(this).val()) == "") {
            $(".btnNewDepartment").attr("disabled", true);
            $(".btnNewDepartment").attr("href", "#");
            $(".btnNewDepartment").on("click", function () {
                return false;
            });
        }
        else {
            $(".btnNewDepartment").attr("disabled", false);
            $(".btnNewDepartment").attr("href", "/Cargoes/Novo?ukCargo=" + $(this).val());
            $(".btnNewDepartment").off("click");

            GetDepartments($(this).val());
        }

    });



    GetDepartments();


});

function GetDepartments() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDepartment").html("");

    $.ajax({
        method: "POST",
        url: "/Cargoes/ListaCargo",
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







function DeletarCargo(IDCargo, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Cargo/TerminarComRedirect",
            data: { IDCargo: IDCargo },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });
    };



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Cargo?", "Exclusão de Cargo", callback, "btn-danger");


}








