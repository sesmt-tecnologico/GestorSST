
jQuery(function ($) {

    AplicaTooltip();

    InitDropZoneSingle();

});

function OnClickBuscarArquivos(pUKObjeto) {

    $.ajax({
        method: "POST",
        url: "/FonteGeradoraDeRisco/ListarArquivosAnexados",
        data: { UKObjeto: pUKObjeto },
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
                sHTML += '          <i class="ace-icon fa fa-cloud-upload bigger-170"></i>';
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
                    data: { ukObjeto: btnUploadArquivo.closest("[data-uniquekey]").data("uniquekey") },
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
