jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null, null,null,null, { "bSortable": false }], false, 20);

    $(".waPesquisar").change(function () {
        if ($("#UKEstabelecimento").val() != "") {
            $("#formPesquisarWorkArea").submit();
        }
    });

    $(".btnPesquisar").click(function () {
        if ($("#UKEstabelecimento").val() != "") {
            $("#formPesquisarWorkArea").submit();
        }
        else {
            ExibirMensagemDeAlerta("Selecione um estabelecimento antes de prosseguir.");
        }
    });

   

   
});




function OnBeginPesquisarWorkArea() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisarWorkArea").css({ opacity: "0.5" });
}

function OnSuccessPesquisarWorkarea(data) {
    $('#formPesquisarWorkArea').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    $(".resultadoWorkArea").html(data);
}

function PesquisarEstabelecimento(ID) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/EstabelecimentoImagens/PesquisarEstabelecimento",
        data: { idEstabelecimento: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Estabelecimento</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}

function DeletarEstabelecimento(IDEstabelecimento, NomeEstabelecimento) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Estabelecimento/Terminar",
            data: { id: IDEstabelecimento },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDEstabelecimento).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Estabelecimento '" + NomeEstabelecimento + "'?", "Exclusão de Estabelecimento", callback, "btn-danger");

}




function OnClickVincularPerigo(pUKWorkArea) {
    $.ajax({
        method: "POST",
        url: "/WorkArea/VincularPerigo",
        data: { UKWorkArea: pUKWorkArea },
        error: function (erro) {
            $("#modalAddPerigoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddPerigoLoading").hide();
            $("#modalAddPerigoCorpo").html(content);

            AutoCompleteAdicionarPerigo();

            $("#modalAddPerigoProsseguir").off("click").on("click", function () {
                var ukWA = $.trim($(".txtUKWorkArea").val());
                var perigos = $.trim($(".txtNovoPerigo").val());

                if (ukWA == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação da work area.");
                }
                else if (perigos == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhum perigo selecionado.");
                }
                else {
                    $("#modalAddPerigoLoading").show();
                    
                    $.ajax({
                        method: "POST",
                        url: "/WorkArea/VincularPerigoAWorkArea",
                        data: { UKWorkArea: ukWA, Perigos: perigos },
                        error: function (erro) {
                            $("#modalAddPerigoLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalAddPerigoLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            if (content.resultado.Sucesso != "") {
                                $(".resultadoWorkArea").html("");

                                if ($("#UKEstabelecimento").val() != "") {
                                    $("#formPesquisarWorkArea").submit();
                                }

                                $('#modalAddPerigo').modal('hide');
                            }




                        }
                    }
                    );
                }
                

            });
        }
    });
}

function AutoCompleteAdicionarPerigo() {
    var tag_input = $('.txtNovoPerigo');

    try {
        tag_input.tag({
            placeholder: 'Campo auto-complete...',
            source: function (query, process) {
                if (query.length >= 3) {

                    $.post('/Perigo/BuscarPerigoForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

        $('.txtNovoPerigo').on('added', function (e, value) {
            $('#modalAddPerigoLoading').show();

            $.post('/Perigo/ConfirmarPerigoForAutoComplete', { key: value }, function (content) {
                $('#modalAddPerigoLoading').hide();

                if (!content.Result) {
                    var $tag_obj = $('.txtNovoPerigo').data('tag');

                    if ($tag_obj.values.length > 0)
                        $.each($tag_obj.values, function (i, v) {
                            if (value == v)
                                $tag_obj.remove(i);
                        });
                }
            });
        })
    }
    catch (e) {
        alert(e);
        tag_input.after('<textarea id="' + tag_input.attr('id') + '" name="' + tag_input.attr('name') + '" rows="3">' + tag_input.val() + '</textarea>').remove();
    }
}

function OnClickRemoverPerigo(pUKPerigo, pUKWorkArea, pDescPerigo) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });
        
        $.ajax({
            method: "POST",
            url: "/WorkArea/TerminarRelComWorkArea",
            data: { UKWorkArea: pUKWorkArea, UKPerigo: pUKPerigo },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $(".resultadoWorkArea").html("");

                    if ($("#UKEstabelecimento").val() != "") {
                        $("#formPesquisarWorkArea").submit();
                    }
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja desvincular o perigo '" + pDescPerigo + "' da work área?", "Remoção de vínculo", callback, "btn-danger");
}




function OnClickNovoRisco(pUKPerigo) {
    $.ajax({
        method: "POST",
        url: "/WorkArea/VincularRisco",
        data: { UKPerigo: pUKPerigo },
        error: function (erro) {
            $("#modalAddRiscoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddRiscoLoading").hide();
            $("#modalAddRiscoCorpo").html(content);

            AutoCompleteAdicionarRisco();

            $("#modalAddRiscoProsseguir").off("click").on("click", function () {
                var ukPerigo = $.trim($(".txtUKPerigo").val());
                var riscos = $.trim($(".txtNovoRisco").val());

                if (ukPerigo == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação do perigo.");
                }
                else if (riscos == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhum risco selecionado.");
                }
                else {
                    $("#modalAddRiscoLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/WorkArea/VincularRiscoAoPerigo",
                        data: { UKPerigo: ukPerigo, Riscos: riscos },
                        error: function (erro) {
                            $("#modalAddRiscoLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalAddRiscoLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            if (content.resultado.Sucesso != "") {
                                $(".resultadoWorkArea").html("");

                                if ($("#UKEstabelecimento").val() != "") {
                                    $("#formPesquisarWorkArea").submit();
                                }

                                $('#modalAddRisco').modal('hide');
                            }




                        }
                    });
                }


            });
        }
    });
}


function AutoCompleteAdicionarRisco() {
    var tag_input = $('.txtNovoRisco');

    try {
        tag_input.tag({
            placeholder: 'Campo auto-complete...',
            source: function (query, process) {
                if (query.length >= 3) {

                    $.post('/Risco/BuscarRiscoForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

        $('.txtNovoRisco').on('added', function (e, value) {
            $('#modalAddRiscoLoading').show();

            $.post('/Risco/ConfirmarRiscoForAutoComplete', { key: value }, function (content) {
                $('#modalAddRiscoLoading').hide();

                if (!content.Result) {
                    var $tag_obj = $('.txtNovoRisco').data('tag');

                    if ($tag_obj.values.length > 0)
                        $.each($tag_obj.values, function (i, v) {
                            if (value == v)
                                $tag_obj.remove(i);
                        });
                }
            });
        })
    }
    catch (e) {
        alert(e);
        tag_input.after('<textarea id="' + tag_input.attr('id') + '" name="' + tag_input.attr('name') + '" rows="3">' + tag_input.val() + '</textarea>').remove();
    }
}

function OnClickRemoverRisco(pUKRisco, pUKPerigo, pNomeRisco) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/WorkArea/TerminarRelComPerigo",
            data: { UKRisco: pUKRisco, UKPerigo: pUKPerigo },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $(".resultadoWorkArea").html("");

                    if ($("#UKEstabelecimento").val() != "") {
                        $("#formPesquisarWorkArea").submit();
                    }
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja desvincular o risco '" + pNomeRisco + "' do perigo?", "Remoção de vínculo", callback, "btn-danger");
}

function OnClickControleDoRisco(pUKWorkArea, pRisco) {
    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDorisco/CriarControle",
        data: { UKWorkArea: pUKWorkArea, UKRisco: pRisco },
        error: function (erro) {
            $("#modalAddControleLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddControleLoading").hide();
            $("#modalAddControleCorpo").html(content);

            AutoCompleteAdicionarControle();

            $("#modalAddControleProsseguir").off("click").on("click", function () {
                var ukControle = $.trim($(".txtNovoControle").val());
                var ukWA = $.trim($(".txtUKWorkArea").val());
                var ukRisc = $.trim($(".txtUKRisco").val());


                if (ukControle == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a o Controle.");
                }

                if (ukWA == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação da workarea.");
                }
                else if (ukRisc == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar Risco.");
                }
                //else {
                //            $("#modalAddControleLoading").show();

                //                    $.ajax({
                //                        method: "POST",
                //                        url: "/ReconhecimentoDoRisco/CadastrarControleDeRisco",
                //                        data: { UKPerigo: ukPerigo, Riscos: riscos },
                //                        error: function (erro) {
                //                            $("#modalAddRiscoLoading").hide();
                //                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                //                        },
                //                        success: function (content) {
                //                            $("#modalAddRiscoLoading").hide();

                //                            TratarResultadoJSON(content.resultado);

                //                            if (content.resultado.Sucesso != "") {
                //                                $(".resultadoWorkArea").html("");

                //                                if ($("#UKEstabelecimento").val() != "") {
                //                                    $("#formPesquisarWorkArea").submit();
                //                                }

                //                                $('#modalAddRisco').modal('hide');
                //                            }


                //                        }
                //                     });
                //    }





            });
        }
    });
}




function AutoCompleteAdicionarControle() {
    var tag_input = $('.txtNovoControle');
    
    try {
        tag_input.tag({
            placeholder: 'Campo auto-complete...',
            source: function (query, process) {
                if (query.length >= 3) {

                    $.post('/ReconhecimentodoRisco/BuscarControlesForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

        $('.txtNovoControle').on('added', function (e, value) {
            $('#modalAddControleLoading').show();

            $.post('/ReconhecimentoDorisco/ConfirmarControleForAutoComplete', { key: value }, function (content) {
                $('#modalAddControleLoading').hide();

                if (!content.Result) {
                    var $tag_obj = $('.txtNovoControle').data('tag');

                    if ($tag_obj.values.length > 0)
                        $.each($tag_obj.values, function (i, v) {
                            if (value == v)
                                $tag_obj.remove(i);
                        });
                }
            });
        })
    }
    catch (e) {
        alert(e);
        tag_input.after('<textarea id="' + tag_input.attr('id') + '" name="' + tag_input.attr('name') + '" rows="3">' + tag_input.val() + '</textarea>').remove();
    }
}

