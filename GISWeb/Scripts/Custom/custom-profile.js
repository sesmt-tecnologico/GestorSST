jQuery(function ($) {

    if ($('#imagemPerfil').length !== 0) {

        var $overflow = '';

        var colorbox_params = {
            rel: 'colorbox',
            close: '&times;',
            maxWidth: '100%',
            maxHeight: '100%',
            onOpen: function () {
                $overflow = document.body.style.overflow;
                document.body.style.overflow = 'hidden';
            },
            onClosed: function () {
                document.body.style.overflow = $overflow;
            },
            onComplete: function () {
                $.colorbox.resize();
            }
        };

        $('.ace-thumbnails [data-rel="colorbox"]').colorbox(colorbox_params);

        $(document).one('ajaxloadstart.page', function (e) {
            $('#colorbox, #cboxOverlay').remove();
        });

        $('.lnkAtualizarFoto').on('click', function () {
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
                EnviaArquivoParaCroppie(this);

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
                    TratarResultadoCroppie({
                        src: resp
                    });
                });
            });
        }
    }

    $("#Celular").mask("(99) 9 9999 - 9999");

    $('#DesabilitarEnvioEmail').on('change', function () {
        var flag = false;

        if ($(this).is(':checked')) {
            flag = true;
        }

        $('#perfilLoading').show();
        BloquearDiv("preferenciasUsuario");

        $.post('/Account/DesabilitarEnvioDeEmail', { flag: flag }, function (content) {
            $('#perfilLoading').hide();
            DesbloquearDiv("preferenciasUsuario");

            if (content.erro) {
                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
            }
            else {
                ExibirMensagemGritter('Sucesso', content.sucesso, 'gritter-success');
            }
        });
    });

    $('#Celular').on('change', function () {
        $('#perfilLoading').show();
        BloquearDiv("preferenciasUsuario");

        $.post('/Account/AlterarNumeroCelular', { novoNumero: $(this).val() }, function (content) {
            $('#perfilLoading').hide();
            DesbloquearDiv("preferenciasUsuario");

            if (content.erro) {
                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
            }
            else {
                ExibirMensagemGritter('Sucesso', content.sucesso, 'gritter-success');
            }
        });
    });


});

function EnviaArquivoParaCroppie(input) {
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

function TratarResultadoCroppie(result) {
    if (result.src) {
        $.post('/Account/AtualizarFoto', { imagemStringBase64: result.src }, function (content) {
            if (content.url)
                location.reload()
        });
    }
}