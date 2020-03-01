

function OnSuccessCadastrarControle(data) {
    $('#formCadastroControle').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);

    if (!(data.resultado.Alerta != null && data.resultado.Alerta != undefined && data.resultado.Alerta != "")) {
        $('#modalAddControle').modal('hide');
    }

}

function OnBeginCadastrarControle()
{
    
    if ($("#TableTiposDeControle").length == 0) {
        ExibirMensagemDeAlerta("Informe pelo menos um tipo de controle para prosseguir com o cadastro.");
        return false;
    }

    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroControle").css({ opacity: "0.5" });

    var idx = 0;
    var arrControles = [];
    $("#TableTiposDeControle tbody>tr").each(function () {

        var sTipoControl = $($(this).children()[0]).data("uk");
        var sClassificacao = $($(this).children()[1]).data("uk");
        var sEficacia = $($(this).children()[2]).data("uk");

        var arrControl = [sTipoControl, sClassificacao, sEficacia];
        arrControles.push(arrControl);

        idx += 1;
    });

    //###########################################################################################################################################

    var doc = {
        UKWorkarea: $("#UKWorkarea").val(),
        UKFonteGeradora: $("#UKFonteGeradora").val(),
        UKPerigo: $("#UKPerigo").val(),
        UKRisco: $("#UKRisco").val(),
        Tragetoria: $("#Tragetoria").val(),
        EClasseDoRisco: $("#EClasseDoRisco").val(),
        Controles: arrControles
    };

    var form = $('#formCadastroControle');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    $.ajax({
        method: "POST",
        url: "/ReconhecimentoDoRisco/CadastrarControleDeRisco",
        data: { __RequestVerificationToken: token, entidade: doc },
        error: function (erro) {
            $('#formCadastroControle').removeAttr('style');
            $(".LoadingLayout").hide();
            $('#btnSalvar').show();

            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (data) {

            $('#formCadastroControle').removeAttr('style');
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