jQuery(function ($) {
    AplicaDatePicker(false);
});

var scripts = ["/Components/jquery.maskedinput/dist/jquery.maskedinput.js", "/Components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js", "/Components/bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-BR.min.js"]
$('.page-content-area').ace_ajax('loadScripts', scripts, function () {

    jQuery(function ($) {
        $('.date-picker').datepicker({
            autoclose: true,
            todayhighlight: true,
            language: "pt-BR",
            format: 'dd/mm/yyyy'
        });

        $(".date-picker").datepicker('setDate', new Date());
    });


});


function BuscarItens() {

    $('.page-content-area').ace_ajax('startLoading');

    $("#conteudoAjax").html("");

    $.ajax({
        method: "POST",
        url: "/CargoFuncAtiv/Itens",
        data: {},
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {

            $("#conteudoAjax").html(content);

            AplicaTooltip();

            $('.dd').nestable();
            $('.dd').nestable('collapseAll');

            $('.dd-handle a').on('mousedown', function (e) {
                e.stopPropagation();
            });

            $('.page-content-area').ace_ajax('stopLoading', true);
        }
    });

}

function btnNovoCargo() {

    $('#modalNovoCargoX').show();

    $('#modalNovoCargoCorpo').html('');
    $('.page-content-area').ace_ajax('startLoading');

    AplicaTooltip();

    $.ajax({
        method: "POST",
        url: "/CargoFuncAtiv/NovoCargo",
        data: {},
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            var divErro = "" +
                "<div class=\"alert alert-danger\">" +
                "<strong>" +
                "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                "Oops! " +
                "</strong>" +

                "<span>" + erro.message + "</span>" +
                "<br />" +
                "</div>";

            $('#modalNovoCargoCorpo').html(divErro);
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            $('#modalNovoCargoCorpo').html(content);

            $('.multiselect').multiselect({
                enableFiltering: true,
                enableHTML: true,
                buttonClass: 'btn btn-white btn-primary',
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> &nbsp;<b class="fa fa-caret-down"></b></button>',
                    ul: '<ul class="multiselect-container dropdown-menu"></ul>',
                    filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
                    filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default btn-white btn-grey multiselect-clear-filter" type="button"><i class="fa fa-times-circle red2"></i></button></span>',
                    li: '<li><a tabindex="0"><label></label></a></li>',
                    divider: '<li class="multiselect-item divider"></li>',
                    liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
                }
            });

            $('.multiselect').css("width", "100%");
            $($('.multiselect').parent()).css("width", "100%");

            $("#ddlDep").change(function () {
                var valor = $(this).val();
                if (valor == null || valor == "") {
                    $("#divExceto").hide();
                }
                else {
                    var bAchou = false;
                    for (var i = 0; i < valor.length; i++) {
                        if ($("#ddlDep option[value='" + valor[i] + "']").text() == "DPR") {
                            bAchou = true;
                        }
                    }
                    if (bAchou) {
                        $("#divExceto").show();
                    }
                    else {
                        $("#divExceto").hide();
                    }
                }
            });

            Chosen();

            $("#modalNovoCargoProsseguir").click(function () {

                var ddlDep = $("#ddlDep").val();

                var ddlCargo = $("#ddlCargos").val();
                var txtCargo = $("#Nome").val();

                if (ddlDep == null) {
                    ExibirMensagemDeAlerta("Selecione um departamento que ficará vinculado ao cargo a ser criado.");
                }
                else if (ddlCargo == undefined && txtCargo == "") {
                    ExibirMensagemDeAlerta("Informe o nome do cargo a ser criado.");
                }
                else if (ddlCargo != undefined && ddlCargo != "" && txtCargo != "") {
                    ExibirMensagemDeAlerta("Não é permitido selecionar um cargo e informar um manualmente.");
                }
                else if (ddlCargo != undefined && ddlCargo == "" && txtCargo == "") {
                    ExibirMensagemDeAlerta("Selecione um cargo da listagem ou informe o nome de um cargo manualmente.");
                }
                else if (ddlCargo != undefined && ddlCargo == "" && txtCargo == "") {
                    ExibirMensagemDeAlerta("Selecione um cargo da listagem ou informe o nome de um cargo manualmente.");
                }
                else {

                    if (ddlDep > 1) {
                        alert("mais que 1");
                        var sValue = "";
                        for (var i = 0; i < ddlDep.length; i++) {
                            sValue += ddlDep[i] + ",";
                        }
                        $("#Departamentos").val(sValue);
                    }
                    else {
                        $("#Departamentos").val(ddlDep);
                    }

                    $("#formCadastroCargo").submit();
                }

            });

        }
    });
}

function OnBeginCadastrarCargo() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessCadastrarCargo(response) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(response.resultado);
}







function CadastrarFuncao(pUKCargo) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Função:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtFuncaoNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Função!</span>",
        message: sHTML,
        buttons:
        {
            "success":
            {
                "label": "Cancelar",
                "className": "btn-sm btn-danger btnReprovar",
                "callback": function () {
                }
            },
            "danger":
            {
                "label": "Salvar",
                "className": "btn-sm btn-success btnAprovar",
                "callback": function () {
                    var pFuncao = $.trim($("#txtFuncaoNome").val());
                    if (pFuncao == "") {
                        ExibirMensagemDeAlerta("Informe uma função para prossegui com a criação.");
                        return false;
                    }
                    else {
                        $('.page-content-area').ace_ajax('startLoading');
                        $.ajax({
                            method: "POST",
                            url: "/CargoFuncAtiv/CadastrarFuncao",
                            data: { UKCargo: pUKCargo, FuncaoNome: pFuncao },
                            error: function (erro) {
                                $('.page-content-area').ace_ajax('stopLoading', true);
                                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                            },
                            success: function (content) {
                                TratarResultadoJSON(content.resultado);
                            }
                        });
                    }

                }
            }
        }
    });
}

function CadastrarAtividade(pUKFuncao) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Atividade:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtAtividadeNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Atividade!</span>",
        message: sHTML,
        buttons:
        {
            "success":
            {
                "label": "Cancelar",
                "className": "btn-sm btn-danger btnReprovar",
                "callback": function () {
                }
            },
            "danger":
            {
                "label": "Salvar",
                "className": "btn-sm btn-success btnAprovar",
                "callback": function () {
                    var pAtividade = $.trim($("#txtAtividadeNome").val());
                    if (pAtividade == "") {
                        ExibirMensagemDeAlerta("Informa uma atividade para prossegui com a criação.");
                        return false;
                    }
                    else {
                        $('.page-content-area').ace_ajax('startLoading');

                        $.ajax({
                            method: "POST",
                            url: "/CargoFuncAtiv/CadastrarAtividade",
                            data: { UKFuncao: pUKFuncao, AtividadeNome: pAtividade },
                            error: function (erro) {
                                $('.page-content-area').ace_ajax('stopLoading', true);
                                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                            },
                            success: function (content) {
                                TratarResultadoJSON(content.resultado);
                            }
                        });
                    }
                }
            }
        }
    });
}

function AlterarCargo(pUKCargo, pCargo) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Cargo:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCargoNome' value='" + pCargo + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Cargo!</span>",
        message: sHTML,
        buttons:
        {
            "success":
            {
                "label": "Cancelar",
                "className": "btn-sm btn-danger btnReprovar",
                "callback": function () {
                }
            },
            "danger":
            {
                "label": "Salvar",
                "className": "btn-sm btn-success btnAprovar",
                "callback": function () {
                    var pCargo = $("#txtCargoNome").val();
                    $.ajax({
                        method: "POST",
                        url: "/CargoFuncAtiv/AlterarCargo",
                        data: { UKCargo: pUKCargo, CargoNome: pCargo },
                        error: function (erro) {
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                        },
                        success: function (content) {
                            TratarResultadoJSON(content.resultado);
                        }
                    });
                }
            }
        }
    });
}

function AlterarFuncao(pUKFuncao, pFuncao) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Função:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtFuncaoNome' value='" + pFuncao + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Função!</span>",
        message: sHTML,
        buttons:
        {
            "success":
            {
                "label": "Cancelar",
                "className": "btn-sm btn-danger btnReprovar",
                "callback": function () {
                }
            },
            "danger":
            {
                "label": "Salvar",
                "className": "btn-sm btn-success btnAprovar",
                "callback": function () {
                    var pFuncao = $("#txtFuncaoNome").val();
                    $.ajax({
                        method: "POST",
                        url: "/CargoFuncAtiv/AlterarFuncao",
                        data: { UKFuncao: pUKFuncao, FuncaoNome: pFuncao },
                        error: function (erro) {
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                        },
                        success: function (content) {
                            TratarResultadoJSON(content.resultado);

                            BuscarItens();

                        }
                    });
                }
            }
        }
    });
}

function AlterarAtividade(pUKAtividade, pAtividade) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Atividade:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtAtividadeNome' value='" + pAtividade + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Atividade!</span>",
        message: sHTML,
        buttons:
        {
            "success":
            {
                "label": "Cancelar",
                "className": "btn-sm btn-danger btnReprovar",
                "callback": function () {
                }
            },
            "danger":
            {
                "label": "Salvar",
                "className": "btn-sm btn-success btnAprovar",
                "callback": function () {
                    var pAtividade = $("#txtAtividadeNome").val();
                    $.ajax({
                        method: "POST",
                        url: "/CargoFuncAtiv/AlterarAtividade",
                        data: { UKAtividade: pUKAtividade, AtividadeNome: pAtividade },
                        error: function (erro) {
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                        },
                        success: function (content) {
                            TratarResultadoJSON(content.resultado);
                        }
                    });
                }
            }
        }
    });
}

function DeletarCargo(IDCargo, CargoNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir o cargo '" + CargoNome + "'?",
        title: "Confirmação para excluir.",
        buttons: {
            confirm: {
                label: "Sim",
                className: "btn-success btn-sm",
            },
            cancel: {
                label: "Não",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            if (result) {
                $('.page-content-area').ace_ajax('startLoading');

                $.ajax({
                    method: "POST",
                    url: "/CargoFuncAtiv/DeletarCargo",
                    data: { UKCargo: IDCargo },
                    error: function (erro) {
                        $('.page-content-area').ace_ajax('stopLoading', true);
                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                    },
                    success: function (content) {
                        $('.page-content-area').ace_ajax('stopLoading', true);
                        TratarResultadoJSON(content.resultado);
                    }
                });
            }
        }
    });
}

function DeletarFuncao(IDFuncao, FuncaoNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir a Função '" + FuncaoNome + "'?",
        title: "Confirmação para excluir.",
        buttons: {
            confirm: {
                label: "Sim",
                className: "btn-success btn-sm",
            },
            cancel: {
                label: "Não",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    method: "POST",
                    url: "/CargoFuncAtiv/DeletarFuncao",
                    data: { IDFuncao: IDFuncao },
                    error: function (erro) {
                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                    },
                    success: function (content) {
                        TratarResultadoJSON(content.resultado);
                    }
                });
            }
        }
    });
}

function DeletarAtividade(IDAtividade, UKAtividade, AtividadeNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir a Atividade '" + AtividadeNome + "'?",
        title: "Confirmação para excluir.",
        buttons: {
            confirm: {
                label: "Sim",
                className: "btn-success btn-sm",
            },
            cancel: {
                label: "Não",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            $.ajax({
                method: "POST",
                url: "/CargoFuncAtiv/DeletarAtividade",
                data: { IDAtividade: IDAtividade, UKAtividade: UKAtividade },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    TratarResultadoJSON(content.resultado);
                }
            });
        }
    });
}

function ListarTiposDocumentosEmpregado(UniqueKeyObj, pName) {
    $('.page-content-area').ace_ajax('startLoading');

    $("#modalTiposDeDocumentosTituloName").html(pName);

    $.ajax({
        method: "POST",
        url: "/TipoDeDocumento/BuscarItensPorCategoria",
        data: { UKObj: UniqueKeyObj },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                ExibirMensagemDeErro(content.resultado.Erro);
            }
            else {
                $("#modalTiposDeDocumentosCorpo").html(content.resultado.Conteudo);

                AplicaTooltip();

                if ($("#tblDocsDataTable").length > 0) {
                    AplicajQdataTable("tblDocsDataTable", [{ "bSortable": false }, null], false, 10);
                }

                $('#modalTiposDeDocumentos').modal('show');
            }
        }
    });
}

function SalvarVinculoComTipoDocumento(obj, UKObj, UKTipo) {
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/TipoDeDocumento/SalvarVinculoComTipoDocumento",
        data: { Operacao: obj.checked, UKObj: UKObj, UKTipoDocumento: UKTipo },
        error: function (erro) {
            $('.page-content-area').ace_ajax('stopLoading', true);
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('.page-content-area').ace_ajax('stopLoading', true);

            TratarResultadoJSON(content);
        }
    });
}









function OnSuccessCadastrarAtividades(data) {
    $('#formCadastroAtividades').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarAtividades() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroAtividades").css({ opacity: "0.5" });
}