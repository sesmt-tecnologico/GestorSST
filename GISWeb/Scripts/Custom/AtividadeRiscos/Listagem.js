jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false },null,null,null,null, { "bSortable": false }], false, 20);

    Chosen();

    $("#btnLocalizarAtividade").off("click").on("click", function () {
        $("#formPesquisarAtividade").submit();
    });

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, null, null, null, null, { "bSortable": false }], false, 20);

    $(".ativPesquisar").change(function () {
        if ($("#UniqueKey").val() != "") {
            $("#formPesquisarAtividade").submit();
        }
        else {
            $(".resultadoAtividade").html("");
        }
    });

    $(".btnPesquisar").click(function () {
        if ($("#UKAtividade").val() != "") {
            $("#formPesquisarAtividade").submit();
        }
        else {
            ExibirMensagemDeAlerta("Selecione uma Atividade antes de prosseguir.");
        }
    });

   // GetDocumentoAtividadeGeradoraRisco();

    

    


});

function OnClickControleDeRisco(pUKAtividade, pPerigo, pRisco) {

    $("#modalAddControleCorpoLoading").show();
    $("#modalAddControleCorpo").html("");

    $.ajax({
        method: "POST",
        url: "/AtividadeGeradoraRisco/CriarControle",
        data: { UKAtividade: pUKAtividade,  UKPerigo: pPerigo, UKRisco: pRisco },
        error: function (erro) {
            $("#modalAddControleCorpoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddControleCorpoLoading").hide();
            $("#modalAddControleCorpo").html(content);

            AplicaTooltip();
        }
    });
}






function OnBeginPesquisarEstabelecimento() {
    $('.page-content-area').ace_ajax('startLoading');
}





function BuscarAtividadeRiscos(UKAtidade) {

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



function DeletarPerigo(IDAtividadeRiscos, Descricao) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/AtividadeRiscos/Terminar",
            data: { IDAtividadeRiscos: IDAtividadeRiscos },
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
                    $("#linha-" + IDAtividadeRiscos).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Perigo Existente '" + Descricao + "'?", "Exclusão do Perigo Existente", callback, "btn-danger");

}

function OnBeginPesquisarAtividade() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
    $("#formPesquisarAtividade").css({ opacity: "0.5" });
}

function OnSuccessPesquisarAtividade(data) {
    $('#formPesquisarAtividade').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    $(".resultadoAtividade").html(data);
    AplicaTooltip();

    $('.dd').nestable();
    $('.dd').nestable('collapseAll');
    $($(".collapseOne button")[1]).click();
    $('.dd-handle a').on('mousedown', function (e) {
        e.stopPropagation();
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

function GetDocumentoAtividadeGeradoraRisco() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDoc").html("");

    $.ajax({
        method: "POST",
        url: "/AtividadeGeradoraRisco/Index",
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






function OnClickVincularPerigo(pUK_Atividade) {
    $.ajax({
        method: "POST",
        url: "/Atividade/VincularPerigoAtividade",
        data: { UKAtiv: pUK_Atividade },
        error: function (erro) {
            $("#modalAddAtividadeLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddAtividadeLoading").hide();
            $("#modalAddAtividadeCorpo").html(content);

            AutoCompleteAdicionarRisco();

            $("#modalAddAtividadeProsseguir").off("click").on("click", function () {
                var Ukativ = $.trim($(".txtUKAtiv").val());
                var UkPerig = $.trim($(".txtNovoPerigo").val());

                if (Ukativ == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a Atividade.");
                }
                else if (UkPerig == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar o Perigo.");
                }
                else {
                    $("#modalAddAtividadeLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/Atividade/VincularPerigo",
                        data: { UKAtividade: Ukativ, UkPerigo: UkPerig },
                        error: function (erro) {
                            $("#modalAddAtividadeLoading").hide();
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            $("#modalAddAtividadeLoading").hide();

                            TratarResultadoJSON(content.resultado);

                            if (content.resultado.Sucesso != "") {
                                $(".resultadoAtividade").html("");
                                $('#modalAddAtividade').modal('hide');

                                GetDocumentoAtividadeGeradoraRisco();
                            }




                        }
                    });
                }


            });
        }
    });

    function AutoCompleteAdicionarRisco() {
        var tag_input = $('.txtNovoPerigo');

        try {
            tag_input.tag
                ({
                    placeholder: 'Campo auto-complete...',
                    source: function (query, process) {
                        if (query.length >= 3) {

                            $.post('/Atividade/BuscarRiscoForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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
                $('#modalAddAtividadeLoading').show();

                $.post('/Atividade/ConfirmarRiscoForAutoComplete', { key: value }, function (content) {
                    $('#modalAddAtividadeLoading').hide();

                    if (!content.Result) {
                        var $tag_obj = $('.txtNovoPerigo').data('tag');

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

function OnClickEditarControleDoRisco(pUKReconhecimento, pUKWorkArea, pFonte, pPerigo, pRisco) {

    $("#modalAddControleCorpoLoading").show();
    $("#modalAddControleCorpo").html("");

    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/EditarControle",
        data: { UKReconhecimento: pUKReconhecimento, UKWorkArea: pUKWorkArea, UKFonte: pFonte, UKPerigo: pPerigo, UKRisco: pRisco },
        error: function (erro) {
            $("#modalAddControleCorpoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $("#modalAddControleCorpoLoading").hide();
            $("#modalAddControleCorpo").html(content);

            AplicaTooltip();

            if ($("#TableTiposDeControle").length > 0) {
                AplicajQdataTable("TableTiposDeControle", [null, null, null, null, { "bSortable": false }], false, 20);
            }

        }
    });

}

function OnClickControleDoRisco(pUKWorkArea, pFonte, pPerigo, pRisco) {

    $("#modalAddControleCorpoLoading").show();
    $("#modalAddControleCorpo").html("");

    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/CriarControle",
        data: { UKWorkArea: pUKWorkArea, UKFonte: pFonte, UKPerigo: pPerigo, UKRisco: pRisco },
        error: function (erro) {
            $("#modalAddControleCorpoLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddControleCorpoLoading").hide();
            $("#modalAddControleCorpo").html(content);

            AplicaTooltip();
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

function OnClickListaReconhecimento(pUKWorkArea, pFonte, pRisco) {
    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/ListaReconhecimento",
        data: { UKWorkArea: pUKWorkArea, UKFonte: pFonte, UKRisco: pRisco },
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

function OnClickNovoTipoControle() {
    $("#modalAddTipoControleCorpoLoading").show();
    $("#modalAddTipoControleLoading").hide();
    $("#modalAddTipoControleCorpo").html("");

    $.ajax({
        method: "POST",
        url: "/TipoDeControle/AdicionarTipoDeControle",
        data: {},
        error: function (erro) {
            $("#modalAddTipoControleCorpoLoading").hide();
            $("#modalAddTipoControleLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddTipoControleCorpoLoading").hide();
            $("#modalAddTipoControleLoading").hide();
            $("#modalAddTipoControleCorpo").html(content);

            $("#modalAddTipoControleProsseguir").off("click").on("click", function () {

                var ddlTipoControle = $("#ddlTipoControle").val();
                var ddlClassificacao = $("#ddlClassificacao").val();
                var ddlEficacia = $("#EControle").val();
                var ddlLink = $("#ddlLink").val();

                var ddlTipoControleTxt = $("#ddlTipoControle option:selected").text();
                var ddlClassificacaoTxt = $("#ddlClassificacao option:selected").text();
                var ddlEficaciaTxt = $("#EControle option:selected").text();
                var ddlLinkTxt = $("#ddlLink option:selected").text();

                if ($("#TableTiposDeControle tr:contains('" + ddlTipoControleTxt + "')").length > 0) {
                    ExibirMensagemDeAlerta("Este tipo de controle já foi adicionado.");
                    return false;
                }

                if (ddlTipoControle == "" || ddlClassificacao == "" || ddlEficacia == "") {
                    ExibirMensagemDeAlerta("Selecione todos os campos antes de prosseguir");
                }
                else {
                    $("#modalAddTipoControle").modal("hide");

                    if (ddlLink == "") {
                        ddlLinkTxt = "";
                    }

                    if ($("#TableTiposDeControle").length == 0) {

                        var sHTML = '<table id="TableTiposDeControle" class="table table-striped table-bordered table-hover">';
                        sHTML += '<thead>';
                        sHTML += '<tr>';
                        sHTML += '<th>Tipo de Controle</th>';
                        sHTML += '<th>Classificação da Medida</th>';
                        sHTML += '<th>Eficácia</th>';
                        sHTML += '<th>Link</th>';
                        sHTML += '<th style="width: 30px;"></th>';
                        sHTML += '</tr>';
                        sHTML += '</thead>';

                        sHTML += '<tbody>';
                        sHTML += '<tr>';
                        sHTML += '<td data-uk="' + ddlTipoControle + '">' + ddlTipoControleTxt + '</td>';
                        sHTML += '<td data-uk="' + ddlClassificacao + '">' + ddlClassificacaoTxt + '</td>';
                        sHTML += '<td data-uk="' + ddlEficacia + '">' + ddlEficaciaTxt + '</td>';

                        if (ddlLinkTxt == "") {
                            sHTML += '<td data-uk="' + ddlLink + '"></td>';
                        }
                        else {
                            sHTML += '<td data-uk="' + ddlLink + '"><a href="#" onclick="ExibirLink(\'' + ddlLink + '\'); return false;">' + ddlLinkTxt + '</a></td>';
                        }


                        sHTML += '<td>';
                        sHTML += '<a href="#" class="CustomTooltip red" title="Excluir Tipo de Controle" onclick="RemoverLinhaTipoDeControle(this);">';
                        sHTML += '<i class="ace-icon fa fa-trash-o bigger-120"></i>';
                        sHTML += '</a>';
                        sHTML += '</td > ';
                        sHTML += '</tr>';
                        sHTML += '</tbody>';
                        sHTML += '</table>';

                        $(".divAlerta").hide();

                        $(".conteudoTipoDeControle").html(sHTML);

                        AplicajQdataTable("TableTiposDeControle", [null, null, null, null, { "bSortable": false }], false, 20);
                    }
                    else {

                        var sHTML2 = '<tr>';
                        sHTML2 += '<td data-uk="' + ddlTipoControle + '">' + ddlTipoControleTxt + '</td>';
                        sHTML2 += '<td data-uk="' + ddlClassificacao + '">' + ddlClassificacaoTxt + '</td>';
                        sHTML2 += '<td data-uk="' + ddlEficacia + '">' + ddlEficaciaTxt + '</td>';

                        if (ddlLinkTxt == "") {
                            sHTML2 += '<td data-uk="' + ddlLink + '"></td>';
                        }
                        else {
                            sHTML2 += '<td data-uk="' + ddlLink + '"><a href="#" onclick="ExibirLink(\'' + ddlLink + '\'); return false;">' + ddlLinkTxt + '</a></td>';
                        }

                        sHTML2 += '<td>';
                        sHTML2 += '<a href="#" class="CustomTooltip red" title="Excluir Tipo de Controle" onclick="RemoverLinhaTipoDeControle(this);">';
                        sHTML2 += '<i class="ace-icon fa fa-trash-o bigger-120"></i>';
                        sHTML2 += '</a>';
                        sHTML2 += '</td > ';
                        sHTML2 += '</tr>';

                        $("#TableTiposDeControle tbody").append(sHTML2);
                    }

                    AplicaTooltip();

                }

            });


        }
    });
}

function RemoverLinhaTipoDeControle(obj) {

    if ($("#TableTiposDeControle tbody>tr").length == 1) {
        $(".conteudoTipoDeControle").html("");
        $(".divAlerta").show();
    }
    else {
        $(obj).parent().parent().remove();
    }
}



function OnClickRemoverFonteGeradora(pUKFonte, pDescFonte) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/FonteGeradoraDeRisco/Terminar",
            data: { UKFonte: pUKFonte },
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

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a fonte geradora '" + pDescFonte + "'?", "Exclusão de Fonte Geradora", callback, "btn-danger");
}

function OnClickRemoverPerigo(pUKPerigo, pUKFonte, pDescPerigo) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/FonteGeradoraDeRisco/TerminarRelComPerigo",
            data: { UKFonte: pUKFonte, UKPerigo: pUKPerigo },
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

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja desvincular o perigo '" + pDescPerigo + "' da fonte geradora?", "Remoção de vínculo", callback, "btn-danger");
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




function OnClickBuscarArquivos(pUKObjeto) {

    $.ajax({
        method: "POST",
        url: "/AtividadeGeradoraRisco/ListarArquivosAnexados",
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


function InitDropZoneSingle(elementoClicado) {
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



function deleteWorkfArea(UK, Nome) {


    var callback = function () {

        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/WorkArea/Terminar",
            data: { id: UK },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

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

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir a work área '" + Nome + "'?", "Exclusão de WorkArea", callback, "btn-danger");

}


function ExcluirReconhecimentoComControles(UKReconhecimento, Perigo, Risco) {

    var callback = function () {

        $('.page-content-area').ace_ajax('startLoading');

        $.ajax({
            method: "POST",
            url: "/ReconhecimentoDoRisco/Terminar",
            data: { id: UKReconhecimento },
            error: function (erro) {
                $('.page-content-area').ace_ajax('stopLoading', true);
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.page-content-area').ace_ajax('stopLoading', true);

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

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o controle de risco realizado para " + Perigo + "/" + Risco + "?", "Exclusão de Controle de Risco", callback, "btn-danger");

}


function OnBeginAtualizarControle() {
    if ($("#TableTiposDeControle").length == 0) {
        ExibirMensagemDeAlerta("Informe pelo menos um tipo de controle para prosseguir com o cadastro.");
        return false;
    }

    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoControle").css({ opacity: "0.5" });

    var idx = 0;
    var arrControles = [];
    $("#TableTiposDeControle tbody>tr").each(function () {

        var sTipoControl = $($(this).children()[0]).data("uk");
        var sClassificacao = $($(this).children()[1]).data("uk");
        var sEficacia = $($(this).children()[2]).data("uk");
        var sLink = $($(this).children()[3]).data("uk");

        var arrControl = [sTipoControl, sClassificacao, sEficacia, sLink];
        arrControles.push(arrControl);

        idx += 1;
    });

    //###########################################################################################################################################

    var doc = {
        UKReconhecimento: $("#UKReconhecimento").val(),
        UKWorkarea: $("#UKWorkarea").val(),
        UKFonteGeradora: $("#UKFonteGeradora").val(),
        UKPerigo: $("#UKPerigo").val(),
        UKRisco: $("#UKRisco").val(),
        Tragetoria: $("#Tragetoria").val(),
        EClasseDoRisco: $("#EClasseDoRisco").val(),
        Controles: arrControles
    };

    var form = $('#formEditarControle');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/AtualizarControleDeRisco",
        data: { __RequestVerificationToken: token, entidade: doc },
        error: function (erro) {
            $('#formEdicaoControle').removeAttr('style');
            $(".LoadingLayout").hide();
            $('#btnSalvar').show();

            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (data) {

            $('#formEdicaoControle').removeAttr('style');
            $(".LoadingLayout").hide();
            $('#btnSalvar').show();

            TratarResultadoJSON(data.resultado);

            if (data.resultado.Sucesso != "") {
                $(".resultadoWorkArea").html("");

                if ($("#UKEstabelecimento").val() != "") {
                    $("#formPesquisarWorkArea").submit();
                }

                $('#modalAddControle').modal('hide');
            }

        }
    });
    //###########################################################################################################################################

    return false;
}

function OnSuccessAtualizarControle() {
    $('#formEdicaoControle').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);

    if (!(data.resultado.Alerta != null && data.resultado.Alerta != undefined && data.resultado.Alerta != "")) {
        $('#modalAddControle').modal('hide');
    }
}


function ExibirLink(UKLink) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Link/BuscarURLLink",
        data: { id: UKLink },
        error: function (erro) {
            $(".LoadingLayout").hide();

            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (data) {

            $(".LoadingLayout").hide();

            var resultado = data.resultado;

            if (resultado.Erro !== null && resultado.Erro !== undefined && resultado.Erro !== "") {
                ExibirMensagemDeErro(resultado.Erro);
            }
            else if (resultado.Alerta !== null && resultado.Alerta !== undefined && resultado.Alerta !== "") {
                ExibirMensagemDeAlerta(resultado.Alerta);
            }
            else if (resultado.Sucesso !== null && resultado.Sucesso !== undefined && resultado.Sucesso !== "") {
                ExibirMensagemDeSucesso(resultado.Sucesso);
            }
            else if (resultado.URL !== null && resultado.URL !== undefined && resultado.URL !== "") {
                window.open(resultado.URL, '_blank');
            }

        }
    });

}