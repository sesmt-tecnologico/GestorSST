jQuery(function ($) {
    $(document).on('click', '.toolbar a[data-target]', function (e) {
        e.preventDefault();
        var target = $(this).data('target');
        $('.widget-box.visible').removeClass('visible');//hide others
        $(target).addClass('visible');//show target
    });
});

function OnBeginDefinirNovaSenha(content) {
    $("#login-box").css({ opacity: "0.5" });
}

function OnSuccessDefinirNovaSenha(content) {
    $('#login-box').removeAttr('style');
    TratarResultadoJSON(content.resultado);
}




function OnSuccessSolicicacaoAcesso(content) {
    $('#forgot-box').removeAttr('style');
    TratarResultadoJSON(content.resultado);
}

function OnBeginSolicicacaoAcesso(content) {
    $("#forgot-box").css({ opacity: "0.5" });
}




function FailMessage(content) {
    ExibirMensagemDeErro("Excessão na página de Definir Nova Senha");
}