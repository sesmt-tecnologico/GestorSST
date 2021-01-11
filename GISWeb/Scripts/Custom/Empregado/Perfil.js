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

    

});



function CarregarAdmissao() {

    $("#modalPerfilLoading").show();

    $.ajax({
        method: "POST",
        url: "/Admissao/BuscarAdmissoesAtuais",
        data: { UKEmpregado: $("#UKEmp").val() },
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {

            $("#modalPerfilLoading").hide();

            if (content.erro != null && content.erro != undefined && content.erro != "") {
                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
            }
            else {
                $("#ConteudoAdmissao").html(content);

                $(".btnNovaAlocacao").off("click").on("click", function (e) {
                    e.preventDefault();
                    OnClickNovaAlocacao($(this));
                });

                $(".btnDesalocar").off("click").on("click", function (e) {
                    e.preventDefault();
                    OnClickDesalocar($(this));
                });

                $(".btnDemitir").off("click").on("click", function (e) {
                    e.preventDefault();
                    OnClickDemitir($(this));
                });

                if ($(".txtEstabelecimento").length > 0) {
                    
                    $.ajax({
                        method: "POST",
                        url: "/WorkArea/BuscarWorkAreaParaPerfilEmpregado",
                        data: { UKEstabelecimento: $(".txtEstabelecimento").data("ukestabelecimento"), UKEmpregado: $("#UKEmp").val() },
                        error: function (erro) {
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            if (content.erro != null && content.erro != undefined && content.erro != "") {
                                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                            }
                            else {
                                $(".conteundoWorkArea").html(content);

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
            $('#modalAlocacaoLoading').hide();
            $('#modalAlocacao').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {

            $('#modalAlocacaoCorpoLoading').hide();
            $('#modalAlocacaoLoading').hide();

            if (content.erro != undefined && content.erro != null && content.erro != "") {
                
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
                    if ($(this).val() == "") {
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

                                if (content.erro != null && content.erro != undefined && content.erro != "") {
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

                    if ($(this).val() == "") {
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

                                if (content.erro != null && content.erro != undefined && content.erro != "") {
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

    $('#modalAlocacaoLoading').hide();

    if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
        ExibirMensagemDeErro(content.resultado.Erro);

        $('#modalAlocacaoX').show();
        $('#modalAlocacaoFechar').removeClass('disabled');
        $('#modalAlocacaoFechar').attr('disabled', false);
        $('#modalAlocacaoProsseguir').removeClass('disabled');
        $('#modalAlocacaoProsseguir').attr('disabled', false);
        $('#modalAlocacaoLoading').hide();

    }
    else if (content.resultado.Alerta != null && content.resultado.Alerta != undefined && content.resultado.Alerta != "") {
        ExibirMensagemDeAlerta(content.resultado.Alerta);

        $('#modalAlocacaoX').show();
        $('#modalAlocacaoFechar').removeClass('disabled');
        $('#modalAlocacaoFechar').attr('disabled', false);
        $('#modalAlocacaoProsseguir').removeClass('disabled');
        $('#modalAlocacaoProsseguir').attr('disabled', false);
        $('#modalAlocacaoLoading').hide();
    }
    else if (content.resultado.URL != null && content.resultado.URL != undefined && content.resultado.URL != "") {
        window.location.href = content.resultado.URL;
    }

}



function OnClickDesalocar(origemElemento) {

    var cargofuncao = $(origemElemento).data("cargafuncao");
    var ukAlocacao = $(origemElemento).data("ukalocacao");

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Alocacao/Desalocar",
            data: { id: ukAlocacao },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });


        $.ajax({
            method: "POST",
            url: "/Documentoalocacao/DesalocarDocs",
            data: { id: ukAlocacao },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });


    };

    



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja desalocar o empregado do cargo/função '" + cargofuncao + "'?", "Desalocar", callback, "btn-danger");

}

function OnClickDemitir(origemElemento) {

    var empresa = $(origemElemento).data("empresa");
    var ukAdmissao = $(origemElemento).data("ukadmissao");

    var callback = function () {
        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Admissao/Demitir",
            data: { id: ukAdmissao },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                TratarResultadoJSON(content.resultado);
            }
        });
    };



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja demitir o empregado da empresa '" + empresa + "'?", "Confirmação de Demissão", callback, "btn-danger");

}



function OnClickIndexar(Nome) {


    //var oRegis = $(".txtRegistro").val();




    var btnUploadArquivo = $(this);

    $('#modalNovoArquivoX').show();

    $('#modalNovoArquivoFechar').removeClass('disabled');
    $('#modalNovoArquivoFechar').removeAttr('disabled');
    $('#modalNovoArquivoFechar').on('click', function (e) {
        e.preventDefault();
        $('#modalNovoArquivo').modal('hide');
    });

    $('#modalNovoArquivoProsseguir').hide();

    $('#modalNovoArquivoCorpo').html('');
    $('#modalNovoArquivoCorpoLoading').show();

    $.ajax({
        method: "GET",
        url: "/Arquivo/IndexFaceUpload",
        data: { ukObjeto: btnUploadArquivo.closest("[data-uniquekey]").data("uniquekey"), Nome: Nome },
        error: function (erro) {
            $('#modalNovoArquivo').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('#modalNovoArquivoCorpoLoading').hide();
            $('#modalNovoArquivoCorpo').html(content);

            InitDropZoneSingle(btnUploadArquivo);

            Chosen();

            $.validator.unobtrusive.parse('#formUpload');
        }
    });


    $(".btnExcluirArquivo").off("click").on("click", function (e) {

        var UKArquivo = $(this).data("ukarquivo");
        var callback = function () {
            $("#modalArquivosCorpoLoading").show();

            $.ajax({
                method: "POST",
                url: "/Arquivo/Excluir",
                data: { ukArquivo: UKArquivo },
                error: function (erro) {

                    $("#modalArquivosCorpoLoading").hide();

                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                },
                success: function (content) {
                    $("#modalArquivosCorpoLoading").hide();

                    if (content.erro) {
                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    }
                    else {
                        ExibirMensagemDeSucesso("Arquivo excluído com sucesso.");
                        $("#modalArquivos").modal("hide");
                    }

                }
            });
        };

        ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este arquivo?", "Exclusão de Arquivo", callback, "btn-danger");

    });


}
function InitDropZoneSingle() {
    try {
        Dropzone.autoDiscover = false;

        var dictDefaultMessage = "";
        dictDefaultMessage += '<span class="bigger-150 bolder">';
        dictDefaultMessage += '  <i class="ace-icon fa fa-caret-right red"></i> Arraste arquivos</span> para upload \
				                <span class="smaller-80 grey">(ou clique)</span> <br /> \
				                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-3x"></i>';

        var previewTemplate = "";
        previewTemplate += "<div class=\"dz-preview dz-file-preview\">\n ";
        previewTemplate += "    <div class=\"dz-details\">\n ";
        previewTemplate += "        <div class=\"dz-filename\">";
        previewTemplate += "            <span data-dz-name></span>";
        previewTemplate += "        </div>\n    ";
        previewTemplate += "        <div class=\"dz-size\" data-dz-size></div>\n  ";
        previewTemplate += "        <img data-dz-thumbnail />\n  ";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"progress progress-small progress-striped active\">";
        previewTemplate += "        <div class=\"progress-bar progress-bar-success\" data-dz-uploadprogress></div>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-success-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-message\">";
        previewTemplate += "        <span data-dz-errormessage></span>";
        previewTemplate += "    </div>\n";
        previewTemplate += "</div>";

        //#######################################################################################################
        //Recupera do form montado os respectivos valores retornados do servidor e armazenados na web como 'data'
        var extensoes = $('#formUpload').data('extensoes');
        if (extensoes == '')
            extensoes = null;

        var uploadMultiplo = $('#formUpload').data('uploadmultiplo');
        /*var maxArquivos = 1;
        if (uploadMultiplo && uploadMultiplo.toUpperCase() == 'TRUE')
            maxArquivos = 200;*/

        var maxArquivos = 200;

        var tamanhoMaximo = $('#formUpload').data('tamanhomaximo');
        //#######################################################################################################

        var myDropzone = new Dropzone("#formUpload", {
            paramName: "file",
            uploadMultiple: false, //se habilitar upload múltiplo, pode bugar o SPF
            parallelUploads: 1, //se for mais que 1, pode bugar o SPF
            maxFilesize: tamanhoMaximo, // MB
            dictFileTooBig: 'Tamanho máximo permitido ultrapassado.',
            maxFiles: maxArquivos,
            dictMaxFilesExceeded: 'Limite máximo de número de arquivos permitidos ultrapassado.',
            acceptedFiles: extensoes,
            dictInvalidFileType: 'Extensão de arquivo inválida para este tipo de anexo.',
            addRemoveLinks: true,
            dictCancelUpload: 'Cancelar',
            dictCancelUploadConfirmation: 'Tem certeza que deseja cancelar?',
            dictRemoveFile: 'Remover',
            dictDefaultMessage: dictDefaultMessage,
            dictResponseError: 'Erro ao fazer o upload do arquivo.',
            dictFallbackMessage: 'Este browser não suporta a funcionalidade de arrastar e soltar arquivos para fazer upload.',
            previewTemplate: previewTemplate,
        });

        myDropzone.on('sending', function (file) {
            if (!$('#formUpload').valid()) {
                myDropzone.removeFile(file);
            } else {
                $('#modalNovoArquivoX').hide();
                $('#modalNovoArquivoFechar').addClass('disabled');
                $('#modalNovoArquivoFechar').attr('disabled', 'disabled');
            }
        });

        myDropzone.on('canceled', function () {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalNovoArquivoX').show();
                $('#modalNovoArquivoFechar').removeClass('disabled');
                $('#modalNovoArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('success', function (file, content) {
            if (content.sucesso || content.alerta) {
                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalNovoArquivo').modal('hide');

                    ExibirMensagemDeSucesso("Imagem Indexada com exito!");
                    $("#modalArquivos").modal("hide");
                }
            }
            else {
                $('#modalNovoArquivoX').show();
                $('#modalNovoArquivoFechar').removeClass('disabled');
                $('#modalNovoArquivoFechar').removeAttr('disabled', 'disabled');
                $(".dz-success-mark").hide();
                $(".dz-error-mark").show();

                if (content.erro) {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    ExibirMensagemGritter('Oops!', 'Ocorreu algum problema não identificado ao fazer o upload do arquivo para o servidor.', 'gritter-error');
                }

            }
        });

        myDropzone.on('error', function () {
            $('#modalNovoArquivoX').show();
            $('#modalNovoArquivoFechar').removeClass('disabled');
            $('#modalNovoArquivoFechar').removeAttr('disabled', 'disabled');
            //ExibirMensagemGritter('Oops!', 'Empregado não reconhecido!', 'gritter-error');
        });

        myDropzone.on('removedfile', function (file) {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalNovoArquivoX').show();
                $('#modalNovoArquivoFechar').removeClass('disabled');
                $('#modalNovoArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('maxfilesexceeded', function () {
            ExibirMensagemGritter('Alerta', 'Só é permitida a inclusão de 1 arquivo para cada tipo de anexo.', 'gritter-warning');
        });

        $(document).one('ajaxloadstart.page', function (e) {
            try {
                myDropzone.destroy();
            } catch (e) { }
        });

    } catch (e) {
        ExibirMensagemGritter('Alerta', 'Este browser não é compatível com o componente Dropzone.js. Sugerimos a utilização do Google Chrome ou Internet Explorer 10 (ou versão superior).', 'gritter-warning');
    }
}