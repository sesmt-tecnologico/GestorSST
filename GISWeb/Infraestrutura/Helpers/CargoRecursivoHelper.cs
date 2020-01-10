using GISModel.DTO.Funcao;
using GISModel.Entidades;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public class CargoRecursivoHelper
    {


        private HtmlHelper helper;
        private static string[] arrCores = ConfigurationManager.AppSettings["Web:PadraoCoresLista"].Split(',');
        private static int idxCores = 0;

        public CargoRecursivoHelper(HtmlHelper helperParam)
        {
            helper = helperParam;
        }


        public IHtmlString MontarListaCargo(List<ListaFuncaoViewModel> lista, string UKCargo, List<FuncCargo> Func)
        {

            HtmlString html = new HtmlString(TratarListaCargo(lista, UKCargo, Func));
            return html;
        }

        public static string TratarListaCargo(List<ListaFuncaoViewModel> lista, string UKCargo, List<FuncCargo> Func)
        {
            StringBuilder sHTML = new StringBuilder();
            sHTML.Append("<ol class=\"dd-list\">");

            idxCores += 1;

            foreach (ListaFuncaoViewModel dep2 in lista.Where(a => a.Uk_Cargo != null && a.Uk_Cargo.ToString().Equals(UKCargo)).OrderBy(b => b.nomeCargo))
            {
                sHTML.Append("<li class=\"dd-item\" data-id=\"" + dep2.Uk_Cargo + "\" > ");

                sHTML.Append("<div class=\"dd2-content\" style=\"border-left: 2px solid " + arrCores[Func.FindIndex(a => a.UniqueKey == dep2.Uk_Funcao)] + ";\">");
                sHTML.Append(dep2.NomeFuncao);
                sHTML.Append("<div class=\"pull-right action-buttons\">");

                sHTML.Append("<a class=\"blue CustomTooltip\" href=\"/Cargo/Novo?UKDepartamento=" + dep2.Uk_Cargo + "\" title=\"Novo cargo\">");
                sHTML.Append("  <i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
                sHTML.Append("</a>");

                sHTML.Append("<a class=\"orange CustomTooltip\" href=\"/Cargo/Edicao?UKDCargo=" + dep2.Uk_Cargo + "\" title=\"Editar cargo\">");
                sHTML.Append("  <i class=\"ace-icon fa fa-pencil bigger-130\"></i>");
                sHTML.Append("</a>");

                if (lista.Where(a => a.Uk_Funcao != null && a.Uk_Funcao.Equals(dep2.Uk_Funcao)).Count() == 0)
                {
                    sHTML.Append("<a class=\"red CustomTooltip\" href=\"#\" title=\"Excluir cargo\" onclick=\"deleteCargo('" + dep2.Uk_Cargo.ToString() + "', '" + dep2.nomeCargo + "'); return false; \"> ");
                    sHTML.Append("  <i class=\"ace-icon fa fa-trash-o bigger-130\"></i>");
                    sHTML.Append("</a>");
                }

                sHTML.Append("</div>");

                sHTML.Append("</div>");

                if (lista.Where(a => a.Uk_Funcao != null && a.Uk_Funcao.Equals(dep2.Uk_Funcao)).Count() > 0)
                {
                    sHTML.Append(TratarListaCargo(lista, dep2.Uk_Funcao.ToString(), Func));
                }

                sHTML.Append("</li>");
            }
            sHTML.Append("</ol>");

            return sHTML.ToString();
        }



    }
}