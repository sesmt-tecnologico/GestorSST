jQuery(function ($) {

    $("Empresa").off("change").on("change", function () {
        alert($(this).val());
    });

});

//ListarQuestionariosPorEmpresa(string UKEmpresa)