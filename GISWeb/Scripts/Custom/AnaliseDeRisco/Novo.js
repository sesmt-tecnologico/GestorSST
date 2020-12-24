jQuery(function ($) {

    Chosen();

    var UKEmpregado = $.trim($(".txtSupervisor").val());
    var UkRegistro = $.trim($(".txtRegistro").val());

    

    GetDocumento(UKEmpregado, UkRegistro);

    //getCadeado(UKFonteGeradora);
   
   
});


function getCadeado(UKFonteGeradora) {

    var UkRegistro = $.trim($(".txtRegistro").val());

    $.ajax({
        method: "POST",
        url: "/AnaliseDeRisco/Index",
        data: {UKFonteGeradora: UkRegistro },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            ExibirMensagemDeErro(erro.responseText);
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            alert("Pegou cadedo!");                      

               
            
        }
    });


}




function GetDocumento(UKEmpregado,UkRegistro) {

    //var UkSupervisor = $.trim($(".txtSupervisor").val());
    //var UkRegistro = $.trim($(".txtRegistro").val());

    var altura = $(window).height();
    var comprimento = $(window).width();  
    if (altura <= 650 && comprimento <= 832) {

        $('.page-content-area').ace_ajax('startLoading');
        $("#contentDoc").html("");

        $.ajax({
            method: "POST",
            url: "/AnaliseDeRisco/BuscarQuestionarioAPR_MD",
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

                if (UkNome == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhum Empregado.");
                }
                else if (UkRegistro == "") {

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
}



    function OnClickVincularAtividade() {
        $.ajax({
            method: "POST",
            url: "/AnaliseDeRisco/VincularAtividade",
            //data: { UKPerigo: pUK_Perigo },
            error: function (erro) {
                $("#modalAddAtividadeLoading").hide();
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $("#modalAddAtividadeLoading").hide();
                $("#modalAddAtividadeCorpo").html(content);

                AutoCompleteAdicionarAtividade();

                $("#modalAddAtividadeProsseguir").off("click").on("click", function () {
                    var UkRegistro = $.trim($("#txtRegistro").val());
                    var UkAtividade = $.trim($(".txtNovaAtividade").val());

                    if (UkAtividade == "") {
                        ExibirMensagemDeAlerta("Não foi possível identificar nenhuma Atividade.");
                    }
                    else if (UkRegistro == "") {

                        ExibirMensagemDeAlerta("Não foi possível identificar nenhum Registro.");

                    }  
                    else {
                           $("#modalAddAtividadeLoading").show();

                                var altura = $(window).height();
                                var comprimento = $(window).width();

                                if (altura <= 650 && comprimento <= 832) {

                                    if ($("#UniqueKey").val() != "") {
                                        var UkSupervisor = $.trim($(".txtSupervisor").val());
                                        var UKAtividade = $.trim($(".txtNovaAtividade").val());
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

                                        $("#modalAddAtividadeLoading").hide();
                                    }
                                } else {


                                    if ($("#UniqueKey").val() != "") {

                                        var UkSupervisor1 = $.trim($(".txtSupervisor").val());
                                        var UKAtividade1 = $.trim($(".txtNovaAtividade").val());
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

                                        $("#modalAddAtividadeLoading").hide();

                                    }

                                 }


                           
                            }

                    
                    $('#modalAddAtividade').modal('hide');
                    // Recarrega a página atual sem usar o cache
                    //document.location.reload(true);

                    $(function () {
                        $("#mensagem").dialog();
                    });
                });          

                
            }
        });






    }





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

function AutoCompleteAdicionarAtividade() {
    var tag_input = $('.txtNovaAtividade');
   
    
    try {
        tag_input.tag
            ({
               
                placeholder: 'Campo auto-complete...',
                source: function (query, process) {
                    
                    var UkAtividade = null;

                    if (query.length >= 3 && UkAtividade == null) {

                       

                        $.post('/AnaliseDeRisco/BuscarAtividadeForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
                            var arr = [];
                           
                            UkAtividade = $.trim($(".txtNovaAtividade").val());



                            if (UkAtividade == "") {

                                var len = partial.Result.length;
                                //if (partial.Result.length > 20) {
                                //    len = 200;
                                //}

                                for (var x = 0; x < len; x++) {
                                    arr.push(partial.Result[x]);
                                }
                                process(arr);


                            }

                            else {
                                alert("Permitido uma ativiade!");

                                if (!content.Result) {
                                    var $tag_obj = $('.txtNovaAtividade').data('tag');

                                    //if ($tag_obj.values.length > 0)


                                    //    $.each($tag_obj.values, function (i, v) {
                                    //        if (value == v)
                                    //            $tag_obj.remove(i);
                                    //    });
                                }
                            }


                        });

                        
                    }
                }
            });
        $('.tags').css('width', '100%');

        $('.txtNovaAtividade').on('added', function (e, value) {
            $('#modalAddAtividadeLoading').show();

            var UkAtividade = $.trim($(".txtNovaAtividade").val());

            $.post('/AnaliseDeRisco/ConfirmarAtividadeForAutoComplete', { key: value }, function (content) {
                $('#modalAddAtividadeLoading').hide();

                var UkAtividade = $.trim($(".txtNovaAtividade").val());

                $("input").prop('disabled', true);
                

                    if (!content.Result) {
                        var $tag_obj = $('.txtNovaAtividade').data('tag');

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