jQuery(function ($) {

    Chosen();

    var UKEmpregado = $.trim($(".txtSupervisor").val());
    var UkRegistro = $.trim($(".txtRegistro").val());

    GetDocumento(UKEmpregado, UkRegistro);
   


});




function GetDocumento(UKEmpregado,UkRegistro) {

    //var UkSupervisor = $.trim($(".txtSupervisor").val());
    //var UkRegistro = $.trim($(".txtRegistro").val());

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDoc").html("");

    $.ajax({
        method: "POST",
        url: "/AnaliseDeRisco/BuscarQuestionarioAPR",
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




function OnClickVincularNome() {
    $.ajax({
        method: "POST",
        url: "/AnaliseDeRisco/VincularNome",
        //data: { UKPerigo: pUK_Perigo },
        error: function (erro) {
            $("#modalAddNomeLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddNomeLoading").hide();
            $("#modalAddNomeCorpo").html(content);

            AutoCompleteAdicionarNome();

            $("#modalAddNomeProsseguir").off("click").on("click", function () {
                var UkRegistro = $.trim($(".txtRegistro").val());
                var UkNome = $.trim($(".txtNovoNome").val());
                
                    if (UkNome == "")
                    {
                        ExibirMensagemDeAlerta("Não foi possível identificar nenhum Empregado.");
                    }
                    else if (UkRegistro == "")
                        {

                                ExibirMensagemDeAlerta("Não foi possível identificar nenhum Registro.");

                    }
                             else {
                                    $("#modalAddNomeLoading").show();

                                    $.ajax({
                                        method: "POST",
                                        url: "/AnaliseDeRisco/VincularNomes",
                                        data: { UKNome: UkNome, UkRegistro: UkRegistro },
                                        error: function (erro) {
                                            $("#modalAddNomeLoading").hide();
                                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                                        },
                                        success: function (content) {
                                            $("#modalAddNomeLoading").hide();

                                            TratarResultadoJSON(content.resultado);

                                            if (content.resultado.Sucesso != "") {
                                                $(".resultadoNomes").html("");
                                                $('#modalAddNome').modal('hide');

                                               
                                                // Recarrega a página atual sem usar o cache
                                                document.location.reload(true);


                                                var UkSupervisor = $.trim($(".txtSupervisor").val());

                                                //GetDocumento();
                                
                                            }




                                        }
                                    });
                            }

                



            });
        }
    });
    function AutoCompleteAdicionarNome() {
        var tag_input = $('.txtNovoNome');

        try {
            tag_input.tag
                ({
                    placeholder: 'Campo auto-complete...',
                    source: function (query, process) {
                        if (query.length >= 3) {

                            $.post('/AnaliseDeRisco/BuscarNomeForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
                                var arr = [];

                                var len = partial.Result.length;
                                if (partial.Result.length > 20) {
                                    len = 20;
                                }

                                for (var x = 0; x < len; x++) {
                                    arr.push(partial.Result[x]);
                                }
                                process(arr);
                            });
                        }
                    }
                });
            $('.tags').css('width', '100%');

            $('.txtNovoNome').on('added', function (e, value) {
                $('#modalAddnomeLoading').show();

                $.post('/AnaliseDeRisco/ConfirmarNomeForAutoComplete', { key: value }, function (content) {
                    $('#modalAddNomeLoading').hide();

                    if (!content.Result) {
                        var $tag_obj = $('.txtNovoNome').data('tag');

                        if ($tag_obj.values.length > 0)
                            $.each($tag_obj.values, function (i, v) {
                                if (value == v)
                                    $tag_obj.remove(i);
                            });
                    }
                });
            });
        }
        catch (e) {
            alert(e);
            tag_input.after('<textarea id="' + tag_input.attr('id') + '" name="' + tag_input.attr('name') + '" rows="3">' + tag_input.val() + '</textarea>').remove();
        }
    }
}
