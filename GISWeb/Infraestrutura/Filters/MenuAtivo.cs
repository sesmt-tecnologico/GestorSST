using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Filters
{
    public class MenuAtivoAttribute : ActionFilterAttribute
    {
        public string MenuAtivo { get; set; }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            filterContext.Controller.ViewBag.MenuAtivo = MenuAtivo;
        }

    }
}