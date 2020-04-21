using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public static class SSTHelperRazorFactory
    {

        public static DepartamentoRecursivoHelper DepartamentoRecursivoHelperRazor(this HtmlHelper helper)
        {
            return new DepartamentoRecursivoHelper(helper);
        }


        public static CargoRecursivoHelper CargoRecursivoHelperRazor(this HtmlHelper helper)
        {
            return new CargoRecursivoHelper(helper);
        }


        public static QuestionarioRecursivoHelper QuestionarioRecursivoHelperRazor(this HtmlHelper helper)
        {
            return new QuestionarioRecursivoHelper(helper);
        }

    }
}