jQuery(function ($) {


    GetDocumento();


});

function GetDocumento() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDoc").html("");

    $.ajax({
        method: "POST",
        url: "/Perigo/ListaPerigo",
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


function OnClickVincularRisco(pUK_Perigo) {
    $.ajax({
        method: "POST",
        url: "/Risco/VincularRiscoPerigo",
        data: { UKPerigo: pUK_Perigo },
        error: function (erro) {
            $("#modalAddAtividadeLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddAtividadeLoading").hide();
            $("#modalAddAtividadeCorpo").html(content);

            AutoCompleteAdicionarAtividade();

            $("#modalAddAtividadeProsseguir").off("click").on("click", function () {
                var UkPer = $.trim($(".txtUKPerigo").val());
                var UkRisc = $.trim($(".txtNovoRisco").val());

                if (UkPer == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação da Função.");
                }
                else if (UkRisc == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhuma atividade.");
                }
                else {
                    $("#modalAddAtividadeLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/Risco/VincularRisco",
                        data: { UKPerigo: UkPer, UKRisco: UkRisc },
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

                                GetDocumento();
                            }




                        }
                    });
                }


            });
        }
    });

    function AutoCompleteAdicionarAtividade() {
        var tag_input = $('.txtNovoRisco');

        try {
            tag_input.tag
                ({
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
                $('#modalAddAtividadeLoading').show();

                $.post('/Risco/ConfirmarRiscoForAutoComplete', { key: value }, function (content) {
                    $('#modalAddAtividadeLoading').hide();

                    if (!content.Result) {
                        var $tag_obj = $('.txtNovoRisco').data('tag');

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






function DeletarPerigo(ID, Descricao) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Perigo/Terminar",
            data: { ID: ID },
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
                    $("#linha-" + ID).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o Perigo '" + Descricao + "'?", "Exclusão de Perigo", callback, "btn-danger");

}