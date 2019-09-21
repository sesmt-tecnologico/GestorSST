using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public static class SSTHelperRazorFactory
    {

        public static DepartamentoRecursivoHelper DepartamentoRecursivoHelperRazor(this HtmlHelper helper)
        {
            return new DepartamentoRecursivoHelper(helper);
        }

    }
}