jQuery(function ($) {


    $(".btnNewDepartment").on("click", function () {
        return false;
    });

    $("#ddlComapny").change(function () {
        $("#contentDepartment").html("");

        if ($.trim($(this).val()) == "") {
            $(".btnNewDepartment").attr("disabled", true);
            $(".btnNewDepartment").attr("href", "#");
            $(".btnNewDepartment").on("click", function () {
                return false;
            });
        }
        else {
            $(".btnNewDepartment").attr("disabled", false);
            $(".btnNewDepartment").attr("href", "/Cargoes/Novo?ukCargo=" + $(this).val());
            $(".btnNewDepartment").off("click");

            GetDepartments($(this).val());
        }

    });



    GetDepartments();


});

function GetDepartments() {

    $('.page-content-area').ace_ajax('startLoading');
    $("#contentDepartment").html("");

    $.ajax({
        method: "POST",
        url: "/Cargo/ListaCargo",
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
                $("#contentDepartment").html(content);

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







function DeletarCargo(IDCargo, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Cargo/TerminarComRedirect",
            data: { IDCargo: IDCargo },
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
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });
    };



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir este Cargo?", "Exclusão de Cargo", callback, "btn-danger");


}





function OnClickVincularAtividade(pUK_Funcao) {
    $.ajax({
        method: "POST",
        url: "/Funcao/VincularFuncaoAtividade",
        data: { UK_Funcao: pUK_Funcao },
        error: function (erro) {
            $("#modalAddAtividadeLoading").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $("#modalAddAtividadeLoading").hide();
            $("#modalAddAtividadeCorpo").html(content);

            AutoCompleteAdicionarAtividade();

            $("#modalAddAtividadeProsseguir").off("click").on("click", function () {
                var ukFunc = $.trim($(".txtUKFuncao").val());
                var Ukativ = $.trim($(".txtNovaAtividade").val());

                if (ukFunc == "") {
                    ExibirMensagemDeAlerta("Não foi possível localizar a identificação da Função.");
                }
                else if (Ukativ == "") {
                    ExibirMensagemDeAlerta("Não foi possível identificar nenhuma atividade.");
                }
                else {
                    $("#modalAddAtividadeLoading").show();

                    $.ajax({
                        method: "POST",
                        url: "/Funcao/VincularCargoFuncaoAtividade",
                        data: { UKAtividade: Ukativ, UkFuncao: ukFunc },
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

                                GetDepartments();
                            }




                        }
                    });
                }


            });
        }
    });
}

function AutoCompleteAdicionarAtividade() {
    var tag_input = $('.txtNovaAtividade');

    try {
        tag_input.tag
       ({
            placeholder: 'Campo auto-complete...',
            source: function (query, process) {
                if (query.length >= 3) {

                    $.post('/Atividade/BuscarAtividadeForAutoComplete?key=' + encodeURIComponent(query), function (partial) {
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

        $('.txtNovaAtividade').on('added', function (e, value) {
            $('#modalAddAtividadeLoading').show();

            $.post('/Atividade/ConfirmarAtividadeForAutoComplete', { key: value }, function (content) {
                $('#modalAddAtividadeLoading').hide();

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







