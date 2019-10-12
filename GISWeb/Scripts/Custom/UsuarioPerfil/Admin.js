jQuery(function ($) {

    AplicaTooltip();

    Chosen();

    $("#ddlDepartamento").change(function () {
        if ($(this).val() == "") {
            $(".conteudoAjax").html("");
        }
        else {
            ListarUsuarios();
        }
    });

});

function ListarUsuarios() {

    var valOrgao = $.trim($("#ddlDepartamento").val());
    if (valOrgao == "") {
        ExibirMensagemDeAlerta("Selecione um departamento antes de prosseguir!");
    }
    else {
        $(".LoadingLayout").show();
        $.post('/Permissoes/BuscarUsuariosPorDepartamento', { id: valOrgao }, function (partial) {
            $(".LoadingLayout").hide();
            TratarResultadoListarUsuarios(partial);
        });
    }
}

function BuscarUsuario() {

    var sFiltro = $.trim($("#txtMatricula").val());

    if (sFiltro == "") {
        ExibirMensagemDeAlerta("Informe uma matricula ou nome para prossegui na pesquisa do usuário.");
    }
    else if (sFiltro.length < 3) {
        ExibirMensagemDeAlerta("Informe pelo menos 3 caracteres para prossegui na pesquisa do usuário.");
    }
    else {

        $(".LoadingLayout").show();
        $.post('/Usuario/BuscarUsuarioPorParteLoginOuNome', { filtro: sFiltro }, function (partial) {
            $(".LoadingLayout").hide();

            alert(partial);
            alert(partial.length);

        });
    }


}

function TratarResultadoListarUsuarios(partial) {
    if (partial.resultado != undefined) {
        TratarResultadoJSON(partial.resultado);
    }
    else {
        $(".conteudoAjax").html(partial.data);

        if (parseInt(partial.usuarios) == 0) {

        }
        else {

            var orderByTable = "[ ";
            var count = parseInt(partial.colunas);
            for (var i = 0; i < count; i++) {
                orderByTable += "null, ";
            }

            if (orderByTable != null) {
                orderByTable = orderByTable.substring(0, orderByTable.length - 2) + "]";
            }

            var oTable1 = AplicajQdataTable("dynamic-table1", eval(orderByTable), false, 25);

            TableTools.classes.container = "btn-group btn-overlap";

            //initiate TableTools extension
            var tableTools_obj = new $.fn.dataTable.TableTools(oTable1, {
                "sRowSelector": "td:not(:last-child)",
                "sRowSelect": "multi",
                "fnRowSelected": function (row) {
                    //check checkbox when row is selected
                    try { $(row).find('input[type=checkbox]').get(idxCol).checked = true }
                    catch (e) { }
                },
                "fnRowDeselected": function (row) {
                    //uncheck checkbox
                    try { $(row).find('input[type=checkbox]').get(idxCol).checked = false }
                    catch (e) { }
                },

                "sSelectedClass": "success"
            });

            $('#dynamic-table1 > thead > tr > th input[type=checkbox]').on('click', function () {

                idxCol = $(this).attr("rel");

                var th_checked = this.checked;//checkbox inside "TH" table header
                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });

                SalvarPermissoes(th_checked, $(this).attr("id"), "");
            });

            //$('#dynamic-table1').on('click', 'td input[type=checkbox]', function () {
            //var row = $(this).closest('tr').get(0);
            //if (!this.checked) tableTools_obj.fnSelect(row);
            //else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            //});

        }

    }
}

function SalvarPermissoes(Acao, Perfil, UIDsUsuarios) {

    var valOrgao = $("#ddlDepartamento").val();
    var valEmpresa = $("#ddlEmpresa").val();

    if (valEmpresa == "") {
        ShowMsgAlerta("É preciso selecionar pelo menos uma empresa!");
    }
    else {
        var sConfig = valEmpresa;
        if (valOrgao != "") {
            sConfig = valOrgao;
        }

        if (UIDsUsuarios == "") {
            $('#dynamic-table1 > tbody > tr').each(function () {
                UIDsUsuarios += $(this).find("input").get(0).id;
            });
        }

        $.post('/Permissoes/SalvarPermissoes', { Acao: Acao, Perfil: Perfil, UIDsUsuarios: UIDsUsuarios, Config: sConfig }, function (partial) {
            if (partial.resultado != undefined && partial.resultado != "") {
                TratarResultadoJSON(partial.resultado);
            }
            else {

            }
        });
    }
}