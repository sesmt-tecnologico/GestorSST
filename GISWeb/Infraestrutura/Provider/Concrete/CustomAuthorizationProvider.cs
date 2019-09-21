using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISWeb.Infraestrutura.Utils;
using Ninject;
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace GISWeb.Infraestrutura.Provider.Concrete
{
    public class CustomAuthorizationProvider : ICustomAuthorizationProvider
    {

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        public bool Autenticado
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(cookie.Value))
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                        if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                        {
                            var AuthenticationModel = Serializador.Deserializar(ticket.UserData);
                            if (AuthenticationModel != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public AutenticacaoModel UsuarioAutenticado
        {
            get
            {
                if (Autenticado)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                            if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                            {
                                return Serializador.Deserializar(ticket.UserData);
                            }
                        }
                    }


                }
                return null;
            }
        }

        public void LogIn(AutenticacaoModel autenticacaoModel, out string msgErro)
        {
            msgErro = string.Empty;
            AutenticacaoModel usuarioPersistido = UsuarioBusiness.ValidarCredenciais(autenticacaoModel);
            if (usuarioPersistido == null)
            {
                throw new Exception("Não foi possível relacionar um usuário com as permissões do módulo " + ConfigurationManager.AppSettings["Web:NomeModulo"] + " à credencial fornecida. Por favor, entre em contato com um dos administradores do módulo.");
            }
            else
            {
                int expiracao;
                try
                {
                    expiracao = Convert.ToInt32(FormsAuthentication.Timeout.TotalMinutes);
                }
                catch { expiracao = 120; }

                GerarTicketEArmazenarComoCookie(usuarioPersistido);
            }
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();

            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
        }

        private void GerarTicketEArmazenarComoCookie(AutenticacaoModel autenticacaoModel, int expiracaoEmMinutos = 480)
        {
            var ticketEncriptado = GerarTicketEncriptado(autenticacaoModel, expiracaoEmMinutos);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado);
            authCookie.Expires = DateTime.Now.AddMinutes(expiracaoEmMinutos);

            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public string GerarTicketEncriptado(AutenticacaoModel autenticacaoModel, int expiracaoEmMinuto = 180)
        {
            var autenticacaoModelSerializado = Serializador.Serializar(autenticacaoModel);
            var ticket = new FormsAuthenticationTicket(1, autenticacaoModel.Login,
                DateTime.Now, DateTime.Now.AddMinutes(expiracaoEmMinuto), false, autenticacaoModelSerializado,
                FormsAuthentication.FormsCookiePath);
            var ticketEncriptado = FormsAuthentication.Encrypt(ticket);
            return ticketEncriptado;
        }

    }
}