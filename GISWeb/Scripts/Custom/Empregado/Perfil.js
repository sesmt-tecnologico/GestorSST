jQuery(function ($) {

    DatePTBR();

    $("#btnAdmissao").off("click").on("click", function () {

        $.ajax({
            method: "POST",
            url: "/Admissao/Novo",
            data: { id: $("#UKEmp").val() },
            error: function (erro) {
                $("#modalAdmissaoLoading").hide();
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $("#modalAdmissaoLoading").hide();
                if (content.erro != null && content.erro != undefined && content.erro != "") {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    $("#modalAdmissao").modal("hide");
                }
                else {
                    $("#modalAdmissaoCorpo").html(content);

                    AplicaDatePicker();

                    $("#modalAdmissaoProsseguir").off("click").on("click", function () {
                        $("#formCadAdmissao").submit();
                    });
                }
            }
        });
    });

    $('.lnkAtualizarFotoPortfolio').on('click', function () {
        $('#modalAtualizarFotoProsseguir').removeClass('disabled');
        $('#modalAtualizarFotoProsseguir').removeAttr('disabled', 'disabled');
        $('#modalAtualizarFotoProsseguir').hide();
    });

    if ($('#inputUpload').length > 0) {
        $('#inputUpload').ace_file_input({
            no_file: 'Selecione algum arquivo...',
            btn_choose: 'Escolher',
            btn_change: 'Trocar',
            allowExt: ["jpeg", "jpg", "png", "gif", "bmp"],
            allowMime: ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"],
            droppable: false,
            thumbnail: false
        }).on('change', function () {
            EnviaArquivoParaCroppieEmpregado(this);

            $('#modalAtualizarFotoProsseguir').show();
        });

        $('#divCropie').croppie({
            viewport: {
                width: 175,
                height: 175,
            },
            boundary: {
                width: 250,
                height: 250,
            }
        });

        $('#modalAtualizarFotoProsseguir').on('click', function (ev) {
            $('#modalAtualizarFotoProsseguir').hide();
            $('#divInputUpload').hide();

            $('#modalAtualizarFotoLoading').show();

            $('#divFotoPerfil').css({ opacity: "0.5" });

            $('.cr-slider').addClass('disabled');
            $('.cr-slider').attr('disabled', 'disabled');

            $('#divCropie').croppie('result', {
                type: 'canvas',
                size: 'viewport'
            }).then(function (resp) {
                TratarResultadoCroppieEmpregado({
                    src: resp,
                    obid: $('#perfilAplicacao').data('obid-aplicacao')
                });
            });
        });
    }

    CarregarAdmissao();

});

function CarregarAdmissao() {

    //BuscarAdmissoesAtuais

    $.ajax({
        method: "POST",
        url: "/Admissao/BuscarAdmissoesAtuais",
        data: { UKEmpregado: $("#UKEmp").val() },
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {

            if (content.erro != null && content.erro != undefined && content.erro != "") {
                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
            }
            else {
                $("#ConteudoAdmissao").html(content);
            }
        }
    });

}

function EnviaArquivoParaCroppieEmpregado(input) {
    if (input.files && input.files[0]) {
        var ext = input.files[0].name.split('.').pop().toLowerCase();

        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            ExibirMensagemGritter('Oops!', 'Extensão de arquivo inválida.', 'gritter-warning');
        } else {
            var uploadCrop = $('#divCropie')
            var reader = new FileReader();

            reader.onload = function (e) {
                uploadCrop.croppie('bind', {
                    url: e.target.result
                });
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
}

function TratarResultadoCroppieEmpregado(result) {
    if (result.src) {
        $.post('/Empregado/AtualizarFotoEmpregado', { imagemStringBase64: result.src, login: $("#txtCPF").val() }, function (content) {
            if (content.url)
                location.reload();
        });
    }
}

function OnBeginCadastrarAdmissao() {
    $('#modalAdmissaoX').hide();
    $('#modalAdmissaoFechar').addClass('disabled');
    $('#modalAdmissaoFechar').attr('disabled', 'disabled');
    $('#modalAdmissaoProsseguir').addClass('disabled');
    $('#modalAdmissaoProsseguir').attr('disabled', 'disabled');
    $('#modalAdmissaoLoading').show();
}

function OnSuccessCadastrarAdmissao(content) {

    if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
        ExibirMensagemDeErro(content.resultado.Erro);

        $('#modalAdmissaoX').show();
        $('#modalAdmissaoFechar').removeClass('disabled');
        $('#modalAdmissaoFechar').attr('disabled', false);
        $('#modalAdmissaoProsseguir').removeClass('disabled');
        $('#modalAdmissaoProsseguir').attr('disabled', false);
        $('#modalAdmissaoLoading').hide();

    }
    else if (content.resultado.URL != null && content.resultado.URL != undefined && content.resultado.URL != "") {
        window.location.href = content.resultado.URL;
    }

}