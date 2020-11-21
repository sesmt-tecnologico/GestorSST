jQuery(function ($) {

    AplicaTooltip();


    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    //var altura = $(window).height();
    //var comprimento = $(window).width();  

    //alert(altura);
    //alert(comprimento);

    //BuscarQuestionario();
    //BuscarQuestionarioMD();

    
    

    $(".oAtividade").change(function () {

        var altura = $(window).height();
        var comprimento = $(window).width();

        if (altura <= 650 && comprimento <= 832) {

            if ($("#UniqueKey").val() != "") {
                var UkSupervisor = $.trim($(".txtSupervisor").val());
                var UKAtividade = $.trim($("#UniqueKey").val());
                var oRegistro = $.trim($("#txtRegistro").val());
                //alert(UkSupervisor);

                $('.page-content-area').ace_ajax('startLoading');

                $.ajax({
                    method: "POST",
                    url: "/AnaliseDeRisco/BuscarQuestionarioPorSupervisorMD",
                    data: { UKEmpregado: UkSupervisor, UKFonteGeradora: UKAtividade, oRegistro: oRegistro },
                    error: function (erro) {
                        $('.page-content-area').ace_ajax('stopLoading', true);

                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                    },
                    success: function (content) {
                        $('.page-content-area').ace_ajax('stopLoading', true);

                        if (content.resultado != null && content.resultado != undefined && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                            ExibirMensagemDeErro(content.resultado.Erro);
                        }
                        else {
                            $(".conteudoQuestionario").html(content);
                            AplicaTooltip();

                            if ($(".dd").length > 0) {
                                $('.dd').nestable();
                                $('.dd').nestable('collapseAll');
                                $($(".collapseOne button")[1]).click();
                                $('.dd-handle a').on('mousedown', function (e) {
                                    e.stopPropagation();
                                });
                            }
                        }

                    }
                });


            }
        } else {

            if ($("#UniqueKey").val() != "") {

                var UkSupervisor1 = $.trim($(".txtSupervisor").val());
                var UKAtividade1 = $.trim($("#UniqueKey").val());
                var oRegistro1 = $.trim($("#txtRegistro").val());
                //alert(UkSupervisor);

                $('.page-content-area').ace_ajax('startLoading');

                $.ajax({
                    method: "POST",
                    url: "/AnaliseDeRisco/BuscarQuestionarioPorSupervisor",
                    data: { UKEmpregado: UkSupervisor1, UKFonteGeradora: UKAtividade1, oRegistro: oRegistro1 },
                    error: function (erro) {
                        $('.page-content-area').ace_ajax('stopLoading', true);

                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                    },
                    success: function (content) {
                        $('.page-content-area').ace_ajax('stopLoading', true);

                        if (content.resultado != null && content.resultado != undefined && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                            ExibirMensagemDeErro(content.resultado.Erro);
                        }
                        else {
                            $(".conteudoQuestionario").html(content);
                            AplicaTooltip();

                            if ($(".dd").length > 0) {
                                $('.dd').nestable();
                                $('.dd').nestable('collapseAll');
                                $($(".collapseOne button")[1]).click();
                                $('.dd-handle a').on('mousedown', function (e) {
                                    e.stopPropagation();
                                });
                            }
                        }

                    }
                });


            }

        }


    });
    
        


});




function OnClickVerAula(pUKObjeto) {

    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/VerAula",
        data: { UKObjeto: pUKObjeto },
        error: function (erro) {
            $("#modalVerAulaLoading").hide();
            $("#modalVerAulaCorpoLoading").hide();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $("#modalVerAulaLoading").hide();
            $("#modalVerAulaX").hide();
            $("#modalVerAulaCorpoLoading").hide();

            $("#modalVerAulaCorpo").html(content);

            AplicaTooltip();


            var vrm = "https://player.vimeo.com/video/";
            var vrm2 = $("#link").val();
            var resultado = vrm2.substring(18);
            var result = vrm + resultado;

            var sHTML = '<iframe src= ' + result.trim() + ' width="560" height="380" frameborder="0" allow="autoplay; fullscreen" allowfullscreen  data-backdrop="static" data-keyboard="false">';

            sHTML += '</iframe>';

            $("#video").html(sHTML);

            $("#modalVerAulaFechar").off("click").on("click", function () {



                var ukWA = "";

                $.ajax({
                    method: "POST",
                    url: "/ReconhecimentoDoRisco/VerAula",
                    data: { UKObjeto: ukWA },
                    error: function (erro) {
                        $("#modalVerAulaLoading").hide();
                        $("#modalVerAulaCorpoLoading").hide();

                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                    },
                    success: function (content) {
                        $("#modalVerAulaLoading").hide();
                        $("#modalVerAulaCorpoLoading").hide();

                        $("#modalVerAulaCorpo").html(content);

                        AplicaTooltip();

                        $('#modalVerAula').modal('hide');
                    }
                });
            });

        }

    });
}





function OnClickVerAR(UKAtividade) {

    $.ajax({
        method: "POST",
        url: "/AnaliseDeRisco/ListarAR",
        data: { ukAtividade: UKAtividade },
        error: function (erro) {
            $("#modalAddArquivosLoading").hide();
            $("#modalAddArquivosCorpoLoading").hide();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $("#modalAddArquivosLoading").hide();
            $("#modalAddArquivosCorpoLoading").hide();

            $("#modalAddArquivosCorpo").html(content);

            AplicaTooltip();

            //if ($("#tableArquivos").length > 0) {
            //    AplicajQdataTable("tableArquivos", [null, null, null], false, 20);

            //    AplicaTooltip();
            //}

        }
    });


}








function OnClickBuscarArquivos(pUKObjeto, Regis) {

    var oRegis = $(".txtRegistro").val();
    

    $.ajax({
        method: "POST",
        url: "/FonteGeradoraDeRisco/ListarArquivosAnexados",
        data: { UKObjeto: pUKObjeto, Registro: Regis },
        error: function (erro) {
            $("#modalArquivosLoading").hide();
            $("#modalArquivosCorpoLoading").hide();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalArquivosLoading").hide();
            $("#modalArquivosCorpoLoading").hide();

            $("#modalArquivosCorpo").html(content);

            AplicaTooltip();

            if ($("#tableArquivos").length > 0) {
                AplicajQdataTable("tableArquivos", [null, null, null, { "bSortable": false }], false, 20);

                var sHTML = '<a href="#" style="float: left; margin-top: 6px;" class="CustomTooltip lnkUploadArquivo" title = "Anexar Novo Arquivo" data-target="#modalNovoArquivo" data-toggle="modal" data-backdrop="static" data-keyboard="false" data-uniquekey="' + pUKObjeto + '">';
                sHTML += '          <i class="ace-icon fa fa-camera bigger-170"></i>';
                sHTML += '</a>';

                $($($("#tableArquivos_wrapper").children()[0]).children()[0]).html(sHTML);

                AplicaTooltip();
            }

            $('.lnkUploadArquivo').off("click").on('click', function (e) {
                e.preventDefault();

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
                    url: "/Arquivo/Upload",
                    data: { ukObjeto: btnUploadArquivo.closest("[data-uniquekey]").data("uniquekey"), Regis: oRegis },
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

                    ExibirMensagemDeSucesso("Arquivo anexado com sucesso.");
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

function BuscarQuestionarioPorSupervisor(UKEmpregado, Fonte) {


    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/AnaliseDeRisco/BuscarQuestionarioPorSupervisor",
        data: { UKEmpregado: UKEmpregado, UKFonteGeradora: Fonte },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.resultado != null && content.resultado != undefined && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                ExibirMensagemDeErro(content.resultado.Erro);
            }
            else {
                $(".conteudoQuestionario").html(content);
                AplicaTooltip();

                if ($(".dd").length > 0) {
                    $('.dd').nestable();
                    $('.dd').nestable('collapseAll');
                    $($(".collapseOne button")[1]).click();
                    $('.dd-handle a').on('mousedown', function (e) {
                        e.stopPropagation();
                    });
                }
            }

        }
    });

}

function BuscarQuestionarioMD() {
    var altura = $(window).height();
    var comprimento = $(window).width();

    if (altura <= 650 && comprimento <= 832) {

        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/Questionario/BuscarQuestionarioPorEmpregadoMD",
            data: { UKEmpregado: $("#txtUKEmpregado").val(), UKFonteGeradora: $("#txtUKFonteGeradora").val() },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

                if (content.resultado != null && content.resultado != undefined && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    ExibirMensagemDeErro(content.resultado.Erro);
                }
                else {
                    $(".conteudoQuestionarioMD").html(content);
                    AplicaTooltip();

                    if ($(".dd").length > 0) {
                        $('.dd').nestable();
                        $('.dd').nestable('collapseAll');
                        $($(".collapseOne button")[1]).click();
                        $('.dd-handle a').on('mousedown', function (e) {
                            e.stopPropagation();
                        });
                    }
                }

            }
        });
    }
}

function GravarQuestionarioAPR(pUKQuestionario, pUKEmpresa) {

    var pUKEmpregado = $("#txtUKEmpregado").val();
    var pUKFonteGeradora = $("#txtUKFonteGeradora").val();
    var Registro = $("#txtRegistro").val();

    var arrPerguntas = [];
    $(".perguntaAPR").each(function () {
        var ukpergunta = $(this).data("uk");
        var tipo = $(this).data("tipo");

        var arrResposta = [];
        arrResposta.push(ukpergunta);


        if (tipo == "Selecao_Unica") {

            $('input:radio[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }

        else if (tipo == "Multipla_Selecao") {
            $('input:checkbox[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }
        else {
            arrResposta.push("*");
            arrResposta.push($(".txtPerguntaAPR-" + ukpergunta).val());
        }


        arrPerguntas.push(arrResposta);




    });

    var obj = {
        UKFonteGeradora: pUKFonteGeradora,
        UKEmpregado: pUKEmpregado,
        UKQuestionario: pUKQuestionario,
        UKEmpresa: pUKEmpresa,
        PerguntasRespondidas: arrPerguntas,
        Registro: Registro

    };

    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/GravarRespostaQuestionarioAnalise",
        data: { entidade: obj },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);
        }
    });

}

function ExisteSubPergunta(pUKPergunta, pUKTipoRespostaItem) {
    $(".conteudoSubPergunta." + pUKPergunta + "." + pUKTipoRespostaItem).html("");
    $(".conteudoSubPergunta." + pUKPergunta).html("");
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/BuscarPerguntasVinculadasView",
        data: { UKPergunta: pUKPergunta, UKTipoRespostaItem: pUKTipoRespostaItem },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.resultado != null && content.resultado != undefined && content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                ExibirMensagemDeErro(content.resultado.Erro);
            }
            else {
                $(".conteudoSubPergunta." + pUKPergunta + "." + pUKTipoRespostaItem).html(content);
                AplicaTooltip();
            }

        }
    });

}

function GravarQuestionario(pUKQuestionario, pUKEmpresa) {

    var pUKEmpregado = $("#txtUKEmpregado").val();
    var pUKFonteGeradora = $("#txtUKFonteGeradora").val();
    var pRegistro = $("#txtRegistro").val();

    var arrPerguntas = [];
    $(".pergunta").each(function () {
        var ukpergunta = $(this).data("uk");
        var tipo = $(this).data("tipo");

        var arrResposta = [];
        arrResposta.push(ukpergunta);


        if (tipo == "Selecao_Unica") {

            $('input:radio[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }

        else if (tipo == "Multipla_Selecao") {
            $('input:checkbox[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }
        else {
            arrResposta.push("*");
            arrResposta.push($(".txtPergunta-" + ukpergunta).val());
        }


        arrPerguntas.push(arrResposta);




    });

    var obj = {
        UKFonteGeradora: pUKFonteGeradora,
        UKEmpregado: pUKEmpregado,
        UKQuestionario: pUKQuestionario,
        UKEmpresa: pUKEmpresa,        
        PerguntasRespondidas: arrPerguntas,
        Registro: pRegistro
    };

    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/GravarRespostaQuestionarioAnalise",
        data: { entidade: obj },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);
        }
    });

}

function GravarQuestionarioMD(pUKQuestionario, pUKEmpresa) {

    var pUKEmpregado = $("#txtUKEmpregado").val();
    var pUKFonteGeradora = $("#txtUKFonteGeradora").val();

    var arrPerguntas = [];
    $(".perguntaMD").each(function () {
        var ukpergunta = $(this).data("uk");
        var tipo = $(this).data("tipo");

        var arrResposta = [];
        arrResposta.push(ukpergunta);


        if (tipo == "Selecao_Unica") {

            $('input:radio[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }

        else if (tipo == "Multipla_Selecao") {
            $('input:checkbox[name="' + ukpergunta + '"]').each(function () {
                if ($(this).is(':checked')) {
                    var UKTipoResp = $(this).data("uk");
                    arrResposta.push(UKTipoResp);

                    var resposta = $.trim($(this).next().text());
                    arrResposta.push(resposta);
                }
            });
        }
        else {
            arrResposta.push("*");
            arrResposta.push($(".txtPerguntaMD-" + ukpergunta).val());
        }


        arrPerguntas.push(arrResposta);




    });

    var obj = {
        UKFonteGeradora: pUKFonteGeradora,
        UKEmpregado: pUKEmpregado,
        UKQuestionario: pUKQuestionario,
        UKEmpresa: pUKEmpresa,
        PerguntasRespondidas: arrPerguntas
    };

    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Questionario/GravarRespostaQuestionarioAnalise",
        data: { entidade: obj },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content.resultado);
        }
    });

}
