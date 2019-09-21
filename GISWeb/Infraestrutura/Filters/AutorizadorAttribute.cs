using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Filters
{
    public class AutorizadorAttribute : AuthorizeAttribute
    {

        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        private string msgErro;
        private bool LoginAutomatico;
        private bool DMZ;
        private string RawUrl;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (!AutorizacaoProvider.Autenticado)
                {

                    DMZ = ConfigurationManager.AppSettings["AD:DMZ"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]);
                    LoginAutomatico = ConfigurationManager.AppSettings["AD:LoginAutomatico"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["AD:LoginAutomatico"]);

                    if (!DMZ && LoginAutomatico)
                    {
                        RawUrl = httpContext.Request.RawUrl;
                        msgErro = "AUTOLOGON";
                        return false;
                    }
                    else
                    {
                        msgErro = "Você precisa estar autenticado e com as devidas permissões para acessar essa página.";
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                msgErro = ex.Message;

                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            var defaultController = ConfigurationManager.AppSettings["Web:DefaultController"].ToString();
            var defaultAction = ConfigurationManager.AppSettings["Web:DefaultAction"].ToString();

            if (!LoginAutomatico && ((string)filterContext.RouteData.Values["controller"] != defaultController || (string)filterContext.RouteData.Values["action"] != defaultAction))
                filterContext.Controller.TempData["MensagemErro"] = msgErro;

            if (!string.IsNullOrWhiteSpace(RawUrl))
                filterContext.Controller.TempData["UrlAnterior"] = RawUrl;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                filterContext.HttpContext.Response.StatusCode = 403;

                var urlRedirect = string.Empty;

                if (System.Web.Security.FormsAuthentication.LoginUrl.Replace("~/", "").Split('/').Length == 2)
                {
                    urlRedirect = urlHelper.Action(System.Web.Security.FormsAuthentication.LoginUrl.Replace("~/", "").Split('/')[1], System.Web.Security.FormsAuthentication.LoginUrl.Replace("~/", "").Split('/')[0]);
                }
                else
                {
                    urlRedirect = urlHelper.Action(System.Web.Security.FormsAuthentication.LoginUrl.Replace("~/", "").Split('/')[2], System.Web.Security.FormsAuthentication.LoginUrl.Replace("~/", "").Split('/')[1]);
                }

                if (msgErro == "AUTOLOGON")
                    urlRedirect = urlRedirect + "Automatico";

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LogOnUrl = urlRedirect
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);

                if (msgErro == "AUTOLOGON")
                    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "Automatico");
                else
                    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl);
            }

        }

    }
}