jQuery(function ($) {
    $(document).on('click', '.toolbar a[data-target]', function (e) {
        e.preventDefault();
        var target = $(this).data('target');
        $('.widget-box.visible').removeClass('visible');//hide others
        $(target).addClass('visible');//show target
    });

    if ($("#formLoginAutomatico").length !== 0) {
        $('#formLoginAutomatico').submit();
    }
});

(function () {
    $.validator.setDefaults({
        // only validate fields when the form is submitted:
        // the Captcha input must only be validated when the whole code string 
        // is typed in, not after each individual character (onkeyup must be
        // false); onfocusout validation could be left on in more complex
        // forms, but doesn't fit this sample
        onkeyup: false,
        onfocusout: false,
        // always reload the Captcha image if remote validation failed,
        // since it will not be usable any more (a failed validation attempt
        // removes the attempted code for necessary Captcha security
        showErrors: function (errorMap, errorList) {
            for (var i = 0; i < errorList.length; i++) {
                var element = errorList[i].element;
                var message = errorList[i].message;
                // check element css class and does the error message match 
                // remote validation failure
                if (element.className.match(/captchaVal/) &&
                    message === this.settings.messages[element.id].remote) {
                    element.Captcha.ReloadImage();
                    $('#formLogin').valid();
                }
            }
        }
    });
})();

$(document).ready(function () {
    // add validation rules by CSS class, so we don't have to know the
    // exact client id of the Captcha code textbox
    if ($('#LoginCaptcha_CaptchaDiv').length > 0) {
        $(".captchaVal").rules('add', {
            required: true,
            remote: $(".captchaVal").get(0).Captcha.ValidationUrl,
            messages: {
                required: "Your input doesn't match displayed characters",
                remote: "Incorrect code, please try again"
            }
        });
    }
});

function OnSuccessLogin(content) {

    if (content.alerta && content.alerta != "") {
        ExibirMensagemGritter(content.titulo, content.alerta, 'gritter-warning');

        if ($('#LoginCaptcha_CaptchaDiv').length > 0) {
            var captcha = $('#CaptchaCode');
            captcha.get(0).Captcha.ReloadImage();
        }

        $('#Senha').val('');
    }
    else if (content.url) {
        location.href = content.url;
    }
}

function OnSuccessSolicicacaoAcesso(content) {
    alert("a");
    $('#forgot-box').removeAttr('style');
    TratarResultadoJSON(content.resultado);
}

function OnBeginSolicicacaoAcesso(content) {
    alert("b");
    $("#forgot-box").css({ opacity: "0.5" });
}

function FailMessage(content) {
    alert("c");
    ExibirMensagemDeErro("Excessão na página de Login");
}