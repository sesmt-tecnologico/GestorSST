using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public static class reCaptchaHelper
    {
        public class reCaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }


        public static IHtmlString reCaptcha(this HtmlHelper helper)
        {
            string publicKey = string.Empty;

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]))
                publicKey = ConfigurationManager.AppSettings["Recaptcha:Public:Internet"];
            else
                publicKey = ConfigurationManager.AppSettings["Recaptcha:Public:Intranet"];

            string componente = "<div class=\"g-recaptcha\" data-sitekey=\"" + publicKey + "\"></div>";

            return MvcHtmlString.Create(componente);
        }

        public static void Validate(string recaptchaResponse)
        {
            if (string.IsNullOrWhiteSpace(recaptchaResponse))
                throw new Exception("A resposta do usuário de validação antibot está faltando.");
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["Recaptcha:GoogleServerSideIntegration"]))
            {
                string privateKey = string.Empty;

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]))
                    privateKey = ConfigurationManager.AppSettings["Recaptcha:Private:Internet"];
                else
                    privateKey = ConfigurationManager.AppSettings["Recaptcha:Private:Intranet"];

                var client = new WebClient();
                var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", privateKey, recaptchaResponse));

                var captchaResponse = JsonConvert.DeserializeObject<reCaptchaResponse>(reply);

                if (!captchaResponse.Success)
                {
                    if (captchaResponse.ErrorCodes.Count > 0)
                    {
                        string erros = string.Empty;

                        foreach (string erro in captchaResponse.ErrorCodes)
                            switch (erro.ToLower())
                            {
                                case ("missing-input-secret"):
                                    erros += "A chave secreta de validação antibot está faltando. ";
                                    break;
                                case ("invalid-input-secret"):
                                    erros += "A chave secreta de validação antibot está inválida. ";
                                    break;

                                case ("missing-input-response"):
                                    erros += "A resposta do usuário de validação antibot está faltando. ";
                                    break;
                                case ("invalid-input-response"):
                                    erros += "A resposta do usuário de validação antibot está inválida. ";
                                    break;

                                default:
                                    erros += "Ocorreu algum erro na validação antibot. Por favor, tente novamente. ";
                                    break;
                            }

                        erros = erros.Remove(erros.Length - 1);

                        throw new Exception(erros);
                    }
                }
            }
        }

    }
}