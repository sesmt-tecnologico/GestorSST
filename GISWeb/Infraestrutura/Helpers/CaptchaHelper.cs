using BotDetect.Web.Mvc;


namespace GISWeb.Infraestrutura.Helpers
{
    public class CaptchaHelper
    {
        public static MvcCaptcha GetLoginCaptcha()
        {
            MvcCaptcha loginCaptcha = new MvcCaptcha("LoginCaptcha");
            loginCaptcha.UserInputID = "CaptchaCode";
            loginCaptcha.ImageSize = new System.Drawing.Size(255, 50);

            return loginCaptcha;
        }

    }
}