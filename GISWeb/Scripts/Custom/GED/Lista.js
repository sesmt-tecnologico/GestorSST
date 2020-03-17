


$("#btnLiberar").click(function() {
    alert("ok");
});


function onClickAnalise(UKAlocacao, UKREL_DocAloc, status) {

    $.ajax({
        method: "POST",
        url: "/Workflow/WorkfolwAnalise",
        data: { UKAlocacao: UKAlocacao, UKREL_DocAloc: UKREL_DocAloc },
        error: function (erro) {
            $("#modalAddAnaliseLoading").hide();
            $("#modalAddAnaliseCorpoLoading").hide();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $("#modalAddAnaliseLoading").hide();
            $("#modalAddAnaliseCorpoLoading").hide();

            $("#modalAddAnaliseCorpo").html(content);

            $("#modalAddAnaliseProsseguir").off("click").on("click", function () {
                var UKAloc = $.trim($(".txtUkAlocacao").val());
                var UKREL = $.trim($(".txtUkREL").val());
                var Coment = $.trim($(".txtComentario").val());

                if (UKAloc == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar esta Alocação.");
                }
                else if (UKREL == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar Documentos nesta Alocação.");
                }
                else {
                    $("#modalAddAnaliseLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/Workflow/ConfirmaAnalise",
                        data: { UKAlocacao: UKAloc, UKREL_DocAloc: UKREL, Coment: Coment, status:status },
                        error: function (erro) {
                            $("#modalAddAnaliseLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalAddAnaliseLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            $('#modalAddAnalise').modal('hide');

                            //if (content.resultado.Sucesso != "") {
                            //    $(".resultadoWorkArea").html("");

                            //    if ($("#UKEstabelecimento").val() != "") {
                            //        $("#formPesquisarWorkArea").submit();
                            //    }

                            //    $('#modalAddPerigo').modal('hide');
                            //}




                        }
                    }
                    );
                }


            });

        }
    });
}


function onClickPesquisaWorkflow(UKAlocacao, UKREL_DocAloc) {

    $.ajax({
        method: "POST",
        url: "/Workflow/PesquisaWorkflow",
        data: { UKAlocacao: UKAlocacao, UKREL_DocAloc: UKREL_DocAloc },
        error: function (erro) {
            $('#modalAddWorkflowX').show();
            $('#modalAddWorkflowFechar').removeClass('disabled');
            $('#modalAddWorkflowFechar').removeAttr('disabled');
            $('#modalAddWorkflowProsseguir').hide();
            $('#modalAddWorkflowCorpo').html('');
            $('#modalAddWorkflowCorpoLoading').show();

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            
            $('#modalAddWorkflowCorpoLoading').hide();
            $('#modalAddWorkflowCorpo').html(content);
            
            $('#modalAddWorkflowLoading').hide();          
                                  

        }
    });
}






$("#URLLogoMarca").on('click',function () {
    alert("The paragraph was clicked.");
});



$("#URLLogoMarca").attr("data-target", "#modalArquivo");
$("#URLLogoMarca").attr("data-toggle", "modal");
$("#URLLogoMarca").attr("data-backdrop", "static");
$("#URLLogoMarca").attr("data-keyboard", "false");

  $("#URLLogoMarca").click(function () {

    var btnUploadArquivo = $(this);

    $('#modalArquivoX').show();
    $('#modalArquivoFechar').removeClass('disabled');
    $('#modalArquivoFechar').removeAttr('disabled');
    $('#modalArquivoProsseguir').hide();
    $('#modalArquivoCorpo').html('');
    $('#modalArquivoCorpoLoading').show();

    $.ajax({
        method: "GET",
        url: "/Arquivo/Upload",
        data: { ukObjeto: btnUploadArquivo.closest("[data-uniquekey]").data("uniquekey") },
        error: function (erro) {
            $('#modalArquivo').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {

            $('#modalArquivoCorpoLoading').hide();
            $('#modalArquivoCorpo').html(content);

            InitDropZoneSingle(btnUploadArquivo);
            Chosen();
            $.validator.unobtrusive.parse('#formUpload');

        }
    });

 });



function InitDropZoneSingle() {
    try {
        Dropzone.autoDiscover = false;

        var dictDefaultMessage = "";
        dictDefaultMessage += '<span class="bigger-150 bolder">';
        dictDefaultMessage += '  <i class="ace-icon fa fa-caret-right red"></i> Arraste a logo</span> para upload \
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
        //var extensoes = $('#formUpload').data('extensoes');
        //if (extensoes == '') extensoes = null;

        //var uploadMultiplo = $('#formUpload').data('uploadmultiplo');
        //var maxArquivos = 1;
        //if (uploadMultiplo && uploadMultiplo.toUpperCase() == 'TRUE') maxArquivos = 20;
        //#######################################################################################################

        var myDropzone = new Dropzone("#formUpload", {
            paramName: "file",
            uploadMultiple: false, //se habilitar upload múltiplo, pode bugar o SPF
            parallelUploads: 1, //se for mais que 1, pode bugar o SPF
            maxFilesize: 20, // MB
            dictFileTooBig: 'Tamanho máximo permitido ultrapassado.',
            //maxFiles: maxArquivos,
            dictMaxFilesExceeded: 'Limite máximo de número de arquivos permitidos ultrapassado.',
            acceptedFiles: ".png,.jpg,.jpeg,.gif",
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
                $('#modalArquivoX').hide();
                $('#modalArquivoFechar').addClass('disabled');
                $('#modalArquivoFechar').attr('disabled', 'disabled');
            }
        });

        myDropzone.on('canceled', function () {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('success', function (file, content) {
            if (content.sucesso) {
                ExibirMensagemGritter('Sucesso!', content.sucesso, 'gritter-success');

                $("#URLLogoMarca").val(content.arquivo);

                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalArquivo').modal('hide');
                }
            } else if (content.alerta) {
                ExibirMensagemGritter('Alerta!', content.alerta, 'gritter-warning');

                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalArquivo').modal('hide');
                }
            }
            else {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');

                if (content.erro)
                    ExibirMensagemGritter('Oops! Erro inesperado', content.erro, 'gritter-error');
                else
                    ExibirMensagemGritter('Oops! Erro inesperado', 'Ocorreu algum problema não identificado ao fazer o upload do arquivo para o servidor.', 'gritter-error');
            }
        });

        myDropzone.on('error', function () {
            $('#modalArquivoX').show();
            $('#modalArquivoFechar').removeClass('disabled');
            $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
        });

        myDropzone.on('removedfile', function (file) {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
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