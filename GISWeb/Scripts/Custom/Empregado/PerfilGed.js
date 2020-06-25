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
                if (content.erro !== null && content.erro !== undefined && content.erro !== "") {
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



    //    $('.btnAlterarValor').on('click', function (e) {
    //        e.preventDefault();

    //        alert("Clicou");

    //        var elementoClicado = $(this);
    //        var propriedade = elementoClicado.data('propriedade');
    //        var tipoCampo = elementoClicado.data('tipo');

    //        if (tipoCampo == 'exibircombobox' || tipoCampo == 'exibirtextbox') {
    //            $('#' + elementoClicado.data('label')).hide();
    //            $('#' + elementoClicado.data('combobox')).show();

    //            if (propriedade == "Telefone") {

    //                var idTextBox = elementoClicado.data('idtextbox');

    //                $("#" + idTextBox).inputmask("(99) 9 9999-9999");

    //                $("#" + idTextBox).keydown(function () {
    //                    try {
    //                        $("#" + idTextBox).unmask();
    //                    } catch (e) { }

    //                    $("#" + idTextBox).inputmask("(99) 9 9999-9999");
    //                });
    //            }

    //        }
    //        else if (tipoCampo == 'salvarcombobox' || tipoCampo == 'salvartextbox') {
    //            valorAtual = $('#' + elementoClicado.data('referencia')).val();

    //            $('.page-content-area').ace_ajax('startLoading');

    //            $.ajax({
    //                method: "POST",
    //                url: "/Home/AlterarPropriedade",
    //                data: { obid: elementoClicado.closest("[data-ukusuario]").data("ukusuario"), propriedade: propriedade, valor: valorAtual },
    //                success: function (content) {
    //                    $('.page-content-area').ace_ajax('stopLoading', true);

    //                    if (content.erro) {
    //                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
    //                    } else {

    //                        location.reload();

    //                    }
    //                }
    //            });
    //        }
    //    });

    //    $('.btnCancelarAlteracao').on('click', function (e) {
    //        e.preventDefault();

    //        var elementoClicado = $(this);

    //        $('#' + elementoClicado.data('label')).show();
    //        $('#' + elementoClicado.data('combobox')).hide();
    //    });



    //});


    //$('.btnAlterarValor').on('click', function (e) {
    //    e.preventDefault();



    //    var elementoClicado = $(this);
    //    var propriedade = elementoClicado.data('propriedade');
    //    var tipoCampo = elementoClicado.data('tipo');

    //    if (tipoCampo == 'exibircombobox' || tipoCampo == 'exibirtextbox') {
    //        $('#' + elementoClicado.data('label')).hide();
    //        $('#' + elementoClicado.data('combobox')).show();

    //        if (propriedade == "Telefone") {

    //            var idTextBox = elementoClicado.data('idtextbox');

    //            $("#" + idTextBox).inputmask("(99) 9 9999-9999");

    //            $("#" + idTextBox).keydown(function () {
    //                try {
    //                    $("#" + idTextBox).unmask();
    //                } catch (e) { }

    //                $("#" + idTextBox).inputmask("(99) 9 9999-9999");
    //            });
    //        }

    //    }
    //    else if (tipoCampo == 'salvarcombobox' || tipoCampo == 'salvartextbox') {
    //        valorAtual = $('#' + elementoClicado.data('referencia')).val();

    //        $('.page-content-area').ace_ajax('startLoading');

    //        $.ajax({
    //            method: "POST",
    //            url: "/Home/AlterarPropriedade",
    //            data: { obid: elementoClicado.closest("[data-ukusuario]").data("ukusuario"), propriedade: propriedade, valor: valorAtual },
    //            success: function (content) {
    //                $('.page-content-area').ace_ajax('stopLoading', true);

    //                if (content.erro) {
    //                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
    //                } else {

    //                    location.reload();

    //                }
    //            }
    //        });
    //    }


    //});

    function CarregarAdmissao() {

        $.ajax({
            method: "POST",
            url: "/Ged/BuscarAdmissoesAtuaisGed",
            data: { UKEmpregado: $("#UKEmp").val() },
            error: function (erro) {
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {

                if (content.erro !== null && content.erro !== undefined && content.erro !== "") {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    $("#ConteudoAdmissao").html(content);

                    $(".btnNovaAlocacao").off("click").on("click", function (e) {
                        e.preventDefault();
                        OnClickNovaAlocacao($(this));
                    });

                    //BuscarWorkAreaParaPerfilEmpregado(string UKEstabelecimento)
                    //conteundoWorkArea

                    if ($(".txtEstabelecimento").length > 0) {

                        $.ajax({
                            method: "POST",
                            url: "/WorkArea/BuscarWorkAreaParaPerfilEmpregado",
                            data: { UKEstabelecimento: $(".txtEstabelecimento").data("ukestabelecimento") },
                            error: function (erro) {
                                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                            },
                            success: function (content) {
                                if (content.erro !== null && content.erro !== undefined && content.erro !== "") {
                                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                                }
                                else {
                                    $(".gedworkarea").hide();
                                    //$(".conteundoWorkArea").html(content);

                                    //$('.dd').nestable();
                                    //$('.dd').nestable('collapseAll');
                                    //$($(".collapseOne button")[1]).click();
                                    //$('.dd-handle a').on('mousedown', function (e) {
                                    //    e.stopPropagation();
                                    //});
                                }
                            }
                        });

                    }
                }
            }
        });

    }

    function EnviaArquivoParaCroppieEmpregado(input) {
        if (input.files && input.files[0]) {
            var ext = input.files[0].name.split('.').pop().toLowerCase();

            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) === -1) {
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

        if (content.resultado.Erro !== null && content.resultado.Erro !== undefined && content.resultado.Erro !== "") {
            ExibirMensagemDeErro(content.resultado.Erro);

            $('#modalAdmissaoX').show();
            $('#modalAdmissaoFechar').removeClass('disabled');
            $('#modalAdmissaoFechar').attr('disabled', false);
            $('#modalAdmissaoProsseguir').removeClass('disabled');
            $('#modalAdmissaoProsseguir').attr('disabled', false);
            $('#modalAdmissaoLoading').hide();

        }
        else if (content.resultado.URL !== null && content.resultado.URL !== undefined && content.resultado.URL !== "") {
            window.location.href = content.resultado.URL;
        }

    }



    function OnClickNovaAlocacao(origemElemento) {
        $('#modalAlocacaoX').show();
        $('#modalAlocacaoFechar').removeClass('disabled');
        $('#modalAlocacaoFechar').removeAttr('disabled', 'disabled');
        $('#modalAlocacaoProsseguir').removeClass('disabled');
        $('#modalAlocacaoProsseguir').removeAttr('disabled', 'disabled');
        $('#modalAlocacaoProsseguir').hide();
        $('#modalAlocacaoCorpo').html('');
        $('#modalAlocacaoCorpoLoading').show();
        $('#modalAlocacaoLoading').hide();

        var ukAdmissao = origemElemento.closest('[data-ukadmissao]').attr('data-ukadmissao');

        $.ajax({
            method: 'POST',
            url: '/Alocacao/Novo',
            data: { id: ukAdmissao },
            error: function (erro) {
                $('#modalAlocacao').modal('hide');
                ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
            },
            success: function (content) {

                $('#modalAlocacaoCorpoLoading').hide();

                if (content.erro !== undefined && content.erro !== null && content.erro !== "") {

                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-warning');
                    $('#modalAlocacaoCorpo').html('<div class="alert alert-warning"><strong><i class="ace-icon fa fa-meh-o"></i> Oops!</strong> ' + content.erro + '<br /></div>');
                }
                else {
                    $('#modalAlocacaoCorpoLoading').hide();
                    $('#modalAlocacaoCorpo').html(content);

                    Chosen();

                    AplicaTooltip();

                    $('#modalAlocacaoProsseguir').show();

                    $("#ddlContrato").off("change").on("change", function () {
                        if ($(this).val() === "") {
                            $('#ddlDepartamento').empty();

                            $('#ddlDepartamento').append($('<option>', {
                                value: "",
                                text: "Selecione um departamento..."
                            }));

                            $("#ddlDepartamento").trigger("chosen:updated");
                        }
                        else {
                            $('#modalAlocacaoLoading').show();


                            $.ajax({
                                method: "POST",
                                url: "/Departamento/BuscarDepartamentosPorContratoParaSelect",
                                data: { id: $(this).val() },
                                error: function (erro) {
                                    $('#modalAlocacaoLoading').hide();
                                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                                },
                                success: function (content) {
                                    $('#modalAlocacaoLoading').hide();

                                    if (content.erro !== null && content.erro !== undefined && content.erro !== "") {
                                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                                    }
                                    else {
                                        $('#ddlDepartamento').empty();

                                        $('#ddlDepartamento').append($('<option>', {
                                            value: "",
                                            text: "Selecione um departamento..."
                                        }));

                                        $("#ddlDepartamento").attr("placeholder", "Selecione um departamento...");

                                        for (var i = 0; i < content.data.length; i++) {
                                            $('#ddlDepartamento').append($('<option>', {
                                                value: content.data[i].UniqueKey,
                                                text: content.data[i].Sigla
                                            }));
                                        }

                                        $("#ddlDepartamento").trigger("chosen:updated");

                                    }
                                }
                            });

                        }
                    });

                    $("#ddlCargo").off("change").on("change", function () {

                        if ($(this).val() === "") {
                            $('#ddlFuncao').empty();

                            $('#ddlFuncao').append($('<option>', {
                                value: "",
                                text: "Selecione uma função..."
                            }));

                            $("#ddlFuncao").trigger("chosen:updated");
                        }
                        else {
                            $('#modalAlocacaoLoading').show();

                            $.ajax({
                                method: "POST",
                                url: "/Funcao/BuscarFuncoesPorCargoParaSelect",
                                data: { id: $(this).val() },
                                error: function (erro) {
                                    $('#modalAlocacaoLoading').hide();
                                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                                },
                                success: function (content) {
                                    $('#modalAlocacaoLoading').hide();

                                    if (content.erro !== null && content.erro !== undefined && content.erro !== "") {
                                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                                    }
                                    else {
                                        $('#ddlFuncao').empty();

                                        $('#ddlFuncao').append($('<option>', {
                                            value: "",
                                            text: "Selecione uma função..."
                                        }));

                                        $("#ddlFuncao").attr("placeholder", "Selecione uma função...");

                                        for (var i = 0; i < content.data.length; i++) {
                                            $('#ddlFuncao').append($('<option>', {
                                                value: content.data[i].UniqueKey,
                                                text: content.data[i].NomeDaFuncao
                                            }));
                                        }

                                        $("#ddlFuncao").trigger("chosen:updated");

                                    }
                                }
                            });

                        }
                    });




                    var funcSubmit = function (e) {

                        $('#modalAlocacaoX').hide();
                        $('#modalAlocacaoFechar').addClass('disabled');
                        $('#modalAlocacaoFechar').attr('disabled', 'disabled');
                        $('#modalAlocacaoProsseguir').addClass('disabled');
                        $('#modalAlocacaoProsseguir').attr('disabled', 'disabled');

                        $('#formCadastroAlocacao').submit();
                    };


                    $('#modalAlocacaoProsseguir').off('click').on('click', function (event) {
                        funcSubmit(event);
                    });

                    $('#modalAlocacaoFechar').off('click').on('click', function () {
                        $('#modalAlocacaoCorpo').html('');
                    });

                    $('#modalAlocacao').on('hide', function () {
                        $('#modalAlocacaoProsseguir').off('click', funcSubmit);
                    });

                }

            }
        });
    }


    function OnBeginCadastrarAlocacao() {
        $('#modalAlocacaoX').hide();
        $('#modalAlocacaoFechar').addClass('disabled');
        $('#modalAlocacaoFechar').attr('disabled', 'disabled');
        $('#modalAlocacaoProsseguir').addClass('disabled');
        $('#modalAlocacaoProsseguir').attr('disabled', 'disabled');
        $('#modalAlocacaoLoading').show();
    }

    function OnSuccessCadastrarAlocacao(content) {

        if (content.resultado.Erro !== null && content.resultado.Erro !== undefined && content.resultado.Erro !== "") {
            ExibirMensagemDeErro(content.resultado.Erro);

            $('#modalAlocacaoX').show();
            $('#modalAlocacaoFechar').removeClass('disabled');
            $('#modalAlocacaoFechar').attr('disabled', false);
            $('#modalAlocacaoProsseguir').removeClass('disabled');
            $('#modalAlocacaoProsseguir').attr('disabled', false);
            $('#modalAlocacaoLoading').hide();

        }
        else if (content.resultado.Alerta !== null && content.resultado.Alerta !== undefined && content.resultado.Alerta !== "") {
            ExibirMensagemDeAlerta(content.resultado.Alerta);
        }
        else if (content.resultado.URL !== null && content.resultado.URL !== undefined && content.resultado.URL !== "") {
            window.location.href = content.resultado.URL;
        }

    }
});