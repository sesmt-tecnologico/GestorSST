using GISModel.Entidades;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public class DepartamentoRecursivoHelper
    {


        private HtmlHelper helper;
        private static string[] arrCores = ConfigurationManager.AppSettings["Web:PadraoCoresLista"].Split(',');
        private static int idxCores = 0;

        public DepartamentoRecursivoHelper(HtmlHelper helperParam)
        {
            helper = helperParam;
        }


        public IHtmlString MontarListaDepartamentos(List<Departamento> lista, string UKEmpresa, string UKDepartment, List<NivelHierarquico> Niveis)
        {

            HtmlString html = new HtmlString(TratarListaDepartamento(lista, UKEmpresa, UKDepartment, Niveis));
            return html;
        }

        public static string TratarListaDepartamento(List<Departamento> lista, string UKEmpresa, string UKDepartment, List<NivelHierarquico> Niveis)
        {
            StringBuilder sHTML = new StringBuilder();
            sHTML.Append("<ol class=\"dd-list\">");

            idxCores += 1;

            foreach (Departamento dep2 in lista.Where(a => a.UKDepartamentoVinculado != null && a.UKDepartamentoVinculado.ToString().Equals(UKDepartment)).OrderBy(b => b.Sigla))
            {
                sHTML.Append("<li class=\"dd-item\" data-id=\"" + dep2.UniqueKey + "\" > ");

                sHTML.Append("<div class=\"dd2-content\" style=\"border-left: 2px solid " + arrCores[Niveis.FindIndex(a => a.UniqueKey == dep2.UKNivelHierarquico)] + ";\">");
                sHTML.Append(dep2.Sigla);
                sHTML.Append("<div class=\"pull-right action-buttons\">");

                sHTML.Append("<a class=\"blue CustomTooltip\" href=\"/Departamento/Novo?UKEmpresa=" + UKEmpresa + "&UKDepartamento=" + dep2.UniqueKey + "\" title=\"Novo departamento\">");
                sHTML.Append("  <i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
                sHTML.Append("</a>");

                sHTML.Append("<a class=\"orange CustomTooltip\" href=\"/Departamento/Edicao?UKEmpresa=" + UKEmpresa + "&UKDepartamento=" + dep2.UniqueKey + "\" title=\"Editar departamento\">");
                sHTML.Append("  <i class=\"ace-icon fa fa-pencil bigger-130\"></i>");
                sHTML.Append("</a>");

                if (lista.Where(a => a.UKDepartamentoVinculado != null && a.UKDepartamentoVinculado.Equals(dep2.UniqueKey)).Count() == 0)
                {
                    sHTML.Append("<a class=\"red CustomTooltip\" href=\"#\" title=\"Excluir departamento\" onclick=\"deleteDepartment('" + dep2.UniqueKey.ToString() + "', '" + dep2.Sigla + "', '" + UKEmpresa + "'); return false; \"> ");
                    sHTML.Append("  <i class=\"ace-icon fa fa-trash-o bigger-130\"></i>");
                    sHTML.Append("</a>");
                }

                sHTML.Append("</div>");

                sHTML.Append("</div>");

                if (lista.Where(a => a.UKDepartamentoVinculado != null && a.UKDepartamentoVinculado.Equals(dep2.UniqueKey)).Count() > 0)
                {
                    sHTML.Append(TratarListaDepartamento(lista, UKEmpresa, dep2.UniqueKey.ToString(), Niveis));
                }

                sHTML.Append("</li>");
            }
            sHTML.Append("</ol>");

            return sHTML.ToString();
        }



    }
}