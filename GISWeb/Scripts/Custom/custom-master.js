jQuery(function ($) {

    $("#modalAlterarSenhaProsseguir").off("click").on("click", function () {
        $("#formAlterarSenha").submit();
    });

   

    $('.btnAlterarValor').on('click', function (e) {
        e.preventDefault();

        

        var elementoClicado = $(this);
        var propriedade = elementoClicado.data('propriedade');
        var tipoCampo = elementoClicado.data('tipo');

        if (tipoCampo == 'exibircombobox' || tipoCampo == 'exibirtextbox') {
            $('#' + elementoClicado.data('label')).hide();
            $('#' + elementoClicado.data('combobox')).show();

            if (propriedade == "Telefone") {

                var idTextBox = elementoClicado.data('idtextbox');
                
                $("#" + idTextBox).inputmask("(99) 9 9999-9999");
            
                $("#" + idTextBox).keydown(function () {
                    try {
                        $("#" + idTextBox).unmask();
                    } catch (e) { }

                    $("#" + idTextBox).inputmask("(99) 9 9999-9999");
                });
            }

        }
        else if (tipoCampo == 'salvarcombobox' || tipoCampo == 'salvartextbox') {
            valorAtual = $('#' + elementoClicado.data('referencia')).val();

            $('.page-content-area').ace_ajax('startLoading');

            $.ajax({
                method: "POST",
                url: "/Home/AlterarPropriedade",
                data: { obid: elementoClicado.closest("[data-ukusuario]").data("ukusuario"), propriedade: propriedade, valor: valorAtual },
                success: function (content) {
                    $('.page-content-area').ace_ajax('stopLoading', true);

                    if (content.erro) {
                        ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                    } else {
                        
                        location.reload();

                    }
                }
            });
        } 
    });

    $('.btnCancelarAlteracao').on('click', function (e) {
        e.preventDefault();

        var elementoClicado = $(this);

        $('#' + elementoClicado.data('label')).show();
        $('#' + elementoClicado.data('combobox')).hide();
    });

});

function OnSuccessAlterarSenha(content) {
    TratarResultadoJSON(content.resultado);

    if (content.resultado.Sucesso != null && content.resultado.Sucesso != undefined && content.resultado.Sucesso != "") {
        $("#modalAlterarSenha").modal("hide");
    }

}
//roda a cada 12 horas
setInterval(function () {

    BuscarTotalDocsInbox();

    

}, 72000);

 function BuscarTotalDocsInbox() {

    $.ajax({
        url: '/Inbox/BuscarTotalDocsInbox',
        global: false,
        type: 'POST',
        async: true,
        success: function (data) {

            $(".liNavbarInboxBadgeTotalPessoal").html(data.resultado.Total);

            //$(".totalDocsPessoal").html(data.resultado.Pessoal + data.resultado.PessoalVeiculo);
            //$(".totalDocsGrupos").html(data.resultado.Grupos + data.resultado.GruposVeiculo);

            $("#liNavbarInbox").show();
        }
    });

}
