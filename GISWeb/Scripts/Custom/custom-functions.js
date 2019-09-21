
function ExibirMensagemGritter(titulo, corpo, gritterClasse) {
    var t = titulo;
    if (t == "") t = 'Alerta!';

    $.gritter.add({
        title: t,
        text: corpo,
        sticky: false,
        before_open: function () {
            $('#gritter-notice-wrapper').css('z-index', '11002');
            if ($('.gritter-item-wrapper').length >= 5) {
                return false;
            }
        },
        class_name: gritterClasse
    });
}

function ExibirMensagemDeErro(erro) {
    ExibirMensagemGritter('Oops! Erro inesperado', erro, 'gritter-error');
}

function ExibirMensagemDeAlerta(msg) {
    ExibirMensagemGritter('Alerta!', msg, 'gritter-warning');
}

function ExibirMensagemDeSucesso(msg) {
    ExibirMensagemGritter('Sucesso!', msg, 'gritter-success');
}

function TratarResultadoJSON(resultado) {

    if (resultado.Erro != null && resultado.Erro != undefined && resultado.Erro != "") {
        ExibirMensagemDeErro(resultado.Erro);
    }
    else if (resultado.Alerta != null && resultado.Alerta != undefined && resultado.Alerta != "") {
        ExibirMensagemDeAlerta(resultado.Alerta);
    }
    else if (resultado.Sucesso != null && resultado.Sucesso != undefined && resultado.Sucesso != "") {
        ExibirMensagemDeSucesso(resultado.Sucesso);
    }
    else if (resultado.URL != null && resultado.URL != undefined && resultado.URL != "") {
        window.location.href = resultado.URL;
    }
    else if (resultado.Reload != null && resultado.Reload != undefined && resultado.Reload) {
        window.location.reload();
    }
}

function ExibirMensagemDeConfirmacaoSimples(mensagem, titulo, callback, classeBotao) {
    if (!classeBotao || classeBotao == '') {
        classeBotao = "btn-success";
    }

    bootbox.confirm({
        backdrop: true,
        message: mensagem + ' Deseja prosseguir?',
        title: titulo,
        buttons: {
            confirm: {
                label: "Prosseguir",
                className: classeBotao + " btn-sm",
            },
            cancel: {
                label: "Cancelar",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            if (result)
                callback();
        }
    });
}

function ExibirMensagemSimples(mensagem, titulo) {
    bootbox.dialog({
        message: mensagem,
        title: titulo,
        backdrop: true,
        locale: "br",
        buttons: {
            "ok": {
                "label": "<i class='ace-icon fa fa-check'></i> Ok",
                "className": "btn-sm btn-success",
            }
        },
        onEscape: true
    });
}

function RecuperarQueryStringUrl(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}

function AplicaTooltip() {
    $(".CustomTooltip").tooltip({
        show: null,
        position: {
            my: "left top",
            at: "left bottom"
        },
        open: function (event, ui) {
            ui.tooltip.animate({ top: ui.tooltip.position().top + 10 }, "fast");
        }
    });
}

function AplicaDatePicker(bloquearDataPassada) {
    if (bloquearDataPassada) {
        var date = new Date();
        date.setDate(date.getDate());

        $('.date-picker').datepicker({
            autoclose: true,
            todayHighlight: true,
            language: 'pt-BR',
            startDate: date
        });
    } else {
        $('.date-picker').datepicker({
            autoclose: true,
            todayHighlight: true,
            language: 'pt-BR'
        });
    }

    $('.date-picker-mes-e-ano').datepicker({
        format: "MM/yyyy",
        startView: "months",
        minViewMode: "months",
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR'
    });
}

function AplicaDateRangePicker() {

    $('.date-range-picker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        locale: {
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
            fromLabel: 'De',
            toLabel: 'Até',
            weekLabel: 'S',
            customRangeLabel: 'Intervalo personalizado',
            daysOfWeek: "Do_Sg_Te_Qa_Qi_Sx_Sa".split("_"),
            monthNames: "janeiro_fevereiro_março_abril_maio_junho_julho_agosto_setembro_outubro_novembro_dezembro".split("_"),
            format: 'DD/MM/YYYY'
        }
    })
        .prev().on(ace.click_event, function () {
            $(this).next().focus();
        });
}

function AplicajQdataTable(nomeControle, aoColumns, bLengthChange, iDisplayLength) {
    return $('#' + nomeControle).dataTable({
        "bAutoWidth": false,
        "oLanguage": {
            "sEmptyTable": "<span class=\"small\" style=\">Nenhum registro encontrado.</span>",
            "sInfo": "<small>Mostrando de _START_ até _END_ de _TOTAL_ registros</small>",
            "sInfoEmpty": "<span class=\"small\">Mostrando 0 até 0 de 0 registros</span>",
            "sInfoFiltered": "<small>(Filtrados de _MAX_ registros)</small>",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ <span class=\"small\">resultados por página</span>",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "<small>Nenhum registro encontrado</small>",
            "sSearch": "<span class=\"small\"><i class=\"ace-icon fa fa-search bigger-110\"> </i> </span> ",
            "oPaginate": {
                "sNext": "<span style=\"font-weight: bold\">>></span>",
                "sPrevious": "<span style=\"font-weight: bold\"><<</span>",
                "sFirst": "<span style=\"font-weight: bold\">|<</span>",
                "sLast": "<span style=\"font-weight: bold\">>|</span>"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },
        "aoColumns": aoColumns,
        "aaSorting": [],
        "bLengthChange": bLengthChange,
        "iDisplayLength": iDisplayLength
    });
}

function AplicajQtableTools(nomeControle, dataTable) {
    TableTools.classes.container = "btn-group btn-overlap";

    var tableTools_obj = new $.fn.dataTable.TableTools(dataTable, {
        //colocar "sRowSelector": "td:not(:last-child)" se quiser que selecione o checkbox ao clicar em qualquer parte da linha
        "sRowSelector": "td:first-child",
        "sRowSelect": "multi",
        "fnRowSelected": function (row) {
            $(".btnAprRej").show();

            try {
                $(row).find('input[type=checkbox]').get(0).checked = true
            }
            catch (e) { }
        },
        "fnRowDeselected": function (row) {
            var blnAchou = false;

            $('#' + nomeControle + ' > tbody > tr > td input[type=checkbox]').each(function () {
                if (this.checked && this.id != $(row).find('input[type=checkbox]').get(0).id) {
                    blnAchou = true;
                }
            });

            if (!blnAchou) {
                $(".btnAprRej").hide();
            }

            try {
                $(row).find('input[type=checkbox]').get(0).checked = false
            }
            catch (e) { }
        },

        "sSelectedClass": "success"
    });

    //select/deselect all rows according to table header checkbox
    $('#' + nomeControle + ' > thead > tr > th input[type=checkbox]').on('click', function () {
        var th_checked = this.checked;//checkbox inside "TH" table header

        $(this).closest('table').find('tbody > tr').each(function () {
            var row = this;

            if (th_checked)
                tableTools_obj.fnSelect(row);
            else
                tableTools_obj.fnDeselect(row);
        });
    });

    $('#' + nomeControle + ' > tbody > tr > td input[type=checkbox]').each(function () {
        if (this.checked) {
            var row = $(this).closest('tr').get(0);
            tableTools_obj.fnSelect(row);
        }
    });

    //select/deselect a row when the checkbox is checked/unchecked
    $('#' + nomeControle + '').on('click', 'td input[type=checkbox]', function () {
        var row = $(this).closest('tr').get(0);

        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            if (!this.checked)
                tableTools_obj.fnSelect(row);
            else
                tableTools_obj.fnDeselect($(this).closest('tr').get(0));
        }

        if (!this.checked)
            tableTools_obj.fnSelect(row);
        else
            tableTools_obj.fnDeselect($(this).closest('tr').get(0));
    });
}

function AplicaMultiSelect() {
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
}

function AplicaValidacaoCPF() {
    var CustomValidation = {};
    CustomValidation.Init = function () {
        //CustomValidationCPF
        $.validator.addMethod('customvalidationcpf', function (value, element, params) {
            var cpf = value.replace(/[^0-9]/gi, ''); //Remove tudo que não for número
            if (value.length == 0)
                return true; //vazio
            if (cpf.length != 11 || cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999")
                return false;
            add = 0;
            for (i = 0; i < 9; i++)
                add += parseInt(cpf.charAt(i)) * (10 - i);
            rev = 11 - (add % 11);
            if (rev == 10 || rev == 11)
                rev = 0;
            if (rev != parseInt(cpf.charAt(9)))
                return false;
            add = 0;
            for (i = 0; i < 10; i++)
                add += parseInt(cpf.charAt(i)) * (11 - i);
            rev = 11 - (add % 11);
            if (rev == 10 || rev == 11)
                rev = 0;
            if (rev != parseInt(cpf.charAt(10)))
                return false;
            return true; //cpf válido
        }, '');
        $.validator.unobtrusive.adapters.add('customvalidationcpf', {}, function (options) {
            options.rules['customvalidationcpf'] = true;
            options.messages['customvalidationcpf'] = options.message;
        });
    }
    //executa -- importante que isso seja feito antes do document.ready
    CustomValidation.Init();
}

function Chosen() {
    if (!ace.vars['touch']) {

        $('.chosen-select').chosen({ allow_single_deselect: true });
        //resize the chosen on window resize

        $(window)
            .off('resize.chosen')
            .on('resize.chosen', function () {
                $('.chosen-select').each(function () {
                    var $this = $(this);
                    $this.next().css({ 'width': $this.parent().width() });
                })
            }).trigger('resize.chosen');
        //resize chosen on sidebar collapse/expand
        $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
            if (event_name != 'sidebar_collapsed') return;
            $('.chosen-select').each(function () {
                var $this = $(this);
                $this.next().css({ 'width': $this.parent().width() });
            })
        });


        $('#chosen-multiple-style .btn').on('click', function (e) {
            var target = $(this).find('input[type=radio]');
            var which = parseInt(target.val());
            if (which == 2) $('#form-field-select-4').addClass('tag-input-style');
            else $('#form-field-select-4').removeClass('tag-input-style');
        });
    }
}