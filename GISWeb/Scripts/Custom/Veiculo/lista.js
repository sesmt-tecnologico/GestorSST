



function OnClickKMChegada(UniqueKey)
{

    $('#modalKMX').show();

    $('#modalKMFechar').removeClass('disabled');
    $('#modalKMFechar').removeAttr('disabled');
    $('#modalKMFechar').on('click', function (e) {
        e.preventDefault();
        $('#modalKM').modal('hide');
    });

    $('#modalKMProsseguir').hide();

    $('#modalKMCorpo').html('');
    $('#modalKMLoading').show();




    $.ajax({

        method: "GET",
        url: "/MovimentacaoVeicular/Edicao",
        data: { UniqueKey: UniqueKey },
        error: function (erro) {
            $('#modalKM').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('#modalKMLoading').hide();
            $('#modalKMCorpo').html(content);

            

            Chosen();

           
        }
    });


}