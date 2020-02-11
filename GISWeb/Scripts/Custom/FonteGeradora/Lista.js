jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null, null, null, null, { "bSortable": false }], false, 20);

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
    AplicaTooltip();

    $('.dd').nestable();
    $('.dd').nestable('collapseAll');
    $($(".collapseOne button")[1]).click();
    $('.dd-handle a').on('mousedown', function (e) {
        e.stopPropagation();
    });
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

function OnClickNovoPerigo(pUKFonteGeradora) {
    $.ajax({
        method: "POST",
        url: "/FonteGeradoraDeRisco/VincularPerigo",
        data: { UKFonteGeradora: pUKFonteGeradora },
        error: function (erro) {
            $("#modalAddPerigoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddPerigoLoading").hide();
            $("#modalAddPerigoCorpo").html(content);

            AutoCompleteAdicionarPerigo();

            $("#modalAddPerigoProsseguir").off("click").on("click", function () {
                var UKfonte = $.trim($(".txtUKFonteGeradora").val());
                var UKPerigo = $.trim($(".txtNovoPerigo").val());

                if (UKPerigo == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação do perigo.");
                }
                else if (UKfonte == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhuma fonte de riscos.");
                }
                else {
                    $("#modalAddPerigoLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/FonteGeradoraDeRisco/VincularPerigoAFonte",
                        data: { UKPerigo: UKPerigo, UKFonte: UKfonte },
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
                    });
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

                    $.post('/FonteGeradoraDeRisco/BuscarPerigoForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

            $.post('/FonteGeradoraDeRisco/ConfirmarPerigoForAutoComplete', { key: value }, function (content) {
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

function OnClickControleDoRisco(pUKWorkArea, pRisco,pFonte) {
    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/CriarControle",
        data: { UKWorkArea: pUKWorkArea, UKRisco: pRisco,UKFonte: pFonte },
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

                    $.post('/ReconhecimentoDoRisco/BuscarControlesForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

            $.post('/ReconhecimentoDoRisco/ConfirmarControleForAutoComplete', { key: value }, function (content) {
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

function OnClickListaReconhecimento(pUKWorkArea, pFonte, pRisco ) {
    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/ListaReconhecimento",
        data: { UKWorkArea: pUKWorkArea, UKFonte: pFonte, UKRisco: pRisco  },
        error: function (erro) {
            $("#modalAddListaReconhecimentoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddListaReconhecimentoLoading").hide();
            $("#modalAddListaReconhecimentoCorpo").html(content);

            AutoCompleteAdicionarControle();

            $("#modalAddListaReconhecimentoProsseguir").off("click").on("click", function () {
                var ukControle = $.trim($(".txtNovoControle").val());
                var ukWA = $.trim($(".txtUKWorkArea").val());
                var ukRisc = $.trim($(".txtUKRisco").val());


               


            });
        }
    });
}
