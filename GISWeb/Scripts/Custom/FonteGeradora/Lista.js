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


function OnClickNovoRisco(pUKPerigo) {
    $.ajax({
        method: "POST",
        url: "/Risco/VincularRiscoPerigo",
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
    $("#modalAddTipoControleCorpo").html("");

    $.ajax({
        method: "POST",
        url: "/TipoDeControle/AdicionarTipoDeControle",
        data: {  },
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
                var ddlClassificacao = $("#EClassificacaoDaMedia").val();
                var ddlEficacia = $("#EControle").val();

                var ddlTipoControleTxt = $("#ddlTipoControle option:selected").text();
                var ddlClassificacaoTxt = $("#EClassificacaoDaMedia option:selected").text();
                var ddlEficaciaTxt = $("#EControle option:selected").text();

                if ($("#TableTiposDeControle tr:contains('" + ddlTipoControleTxt + "')").length > 0) {
                    ExibirMensagemDeAlerta("Este tipo de controle já foi adicionado.");
                    return false;
                }

                if (ddlTipoControle == "" || ddlClassificacao == "" || ddlEficacia == "") {
                    ExibirMensagemDeAlerta("Selecione todos os campos antes de prosseguir");
                }
                else {
                    $("#modalAddTipoControle").modal("hide");

                    if ($("#TableTiposDeControle").length == 0) {

                        var sHTML = '<table id="TableTiposDeControle" class="table table-striped table-bordered table-hover">';
                        sHTML += '<thead>';
                        sHTML += '<tr>';
                        sHTML += '<th>Tipo de Controle</th>';
                        sHTML += '<th>Classificação da Medida</th>';
                        sHTML += '<th>Eficácia</th>';
                        sHTML += '<th style="width: 30px;"></hd>';
                        sHTML += '</tr>';
                        sHTML += '</thead>';

                        sHTML += '<tbody>';
                        sHTML += '<tr>';
                        sHTML += '<td data-uk="' + ddlTipoControle + '">' + ddlTipoControleTxt + '</td>';
                        sHTML += '<td data-uk="' + ddlClassificacao + '">' + ddlClassificacaoTxt + '</td>';
                        sHTML += '<td data-uk="' + ddlEficacia + '">' + ddlEficaciaTxt + '</td>';
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

                        AplicajQdataTable("TableTiposDeControle", [null, null, null, { "bSortable": false }], false, 20);
                    }
                    else {

                        var sHTML2 = '<tr>';
                        sHTML2 += '<td data-uk="' + ddlTipoControle + '">' + ddlTipoControleTxt + '</td>';
                        sHTML2 += '<td data-uk="' + ddlClassificacao + '">' + ddlClassificacaoTxt + '</td>';
                        sHTML2 += '<td data-uk="' + ddlEficacia + '">' + ddlEficaciaTxt + '</td>';
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

    var callback = function() {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/FonteGeradoraDeRisco/Terminar",
            data: { UKFonte: pUKFonte },
            error: function(erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function(content) {
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

    var callback = function() {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/FonteGeradoraDeRisco/TerminarRelComPerigo",
            data: { UKFonte: pUKFonte, UKPerigo: pUKPerigo },
            error: function(erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function(content) {
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

    var callback = function() {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/WorkArea/TerminarRelComPerigo",
            data: { UKRisco: pUKRisco, UKPerigo: pUKPerigo },
            error: function(erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function(content) {
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