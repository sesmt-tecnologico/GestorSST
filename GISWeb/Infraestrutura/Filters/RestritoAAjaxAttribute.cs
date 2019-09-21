using System;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Filters
{
    public class RestritoAAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                throw new InvalidOperationException("Essa página somente pode ser acessada via requisições Ajax.");
        }

    }
}