





function OnClickFluido(UKEmpregado, UkRegistro) {

    //var UkSupervisor = $.trim($(".txtSupervisor").val());
    //var UkRegistro = $.trim($(".txtRegistro").val());

    var altura = $(window).height();
    var comprimento = $(window).width();
    if (altura <= 650 && comprimento <= 832) {

        $('.page-content-area').ace_ajax('startLoading');
        $("#contentDoc").html("");

        $.ajax({
            method: "POST",
            url: "/ChecklistVeicular/BuscarQuestionarioFluidoMD",
            data: { UKEmpregado: UKEmpregado, UKFonteGeradora: UkRegistro },
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
                    $("#contentDoc").html(content);

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
    } else {
        $('.page-content-area').ace_ajax('startLoading');
        $("#contentDoc").html("");

        $.ajax({
            method: "POST",
            url: "/ChecklistVeicular/BuscarQuestionarioFluidos",
            data: { UKEmpregado: UKEmpregado, UKFonteGeradora: UkRegistro },
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
                    $("#contentDoc").html(content);

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
}