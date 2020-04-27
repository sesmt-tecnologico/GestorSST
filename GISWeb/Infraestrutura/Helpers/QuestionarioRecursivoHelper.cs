using GISHelpers.Utils;
using GISModel.Entidades.Quest;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.Helpers
{
    public class QuestionarioRecursivoHelper
    {


        private HtmlHelper helper;
        private static string[] arrCores = ConfigurationManager.AppSettings["Web:PadraoCoresLista"].Split(',');
        private static int idxCores = 0;

        public QuestionarioRecursivoHelper(HtmlHelper helperParam)
        {
            helper = helperParam;
        }


        public IHtmlString MontarListaPerguntas(List<Pergunta> lista)
        {

            HtmlString html = new HtmlString(TratarListaPergunta(lista));
            return html;
        }

        public static string TratarListaPergunta(List<Pergunta> lista)
        {
            StringBuilder sHTML = new StringBuilder();
            sHTML.Append("<ol class=\"dd-list\">");

            idxCores += 1;

            foreach (Pergunta perg in lista)
            {
                sHTML.Append("<li class=\"dd-item\"> ");

                sHTML.Append("<div class=\"dd2-content\" style=\"border-left: 2px solid " + arrCores[2] + ";\">");

                if (perg.TipoResposta == GISModel.Enums.ETipoResposta.Multipla_Selecao || perg.TipoResposta == GISModel.Enums.ETipoResposta.Selecao_Unica)
                {
                    sHTML.Append("<span style=\"width: 434px; float: left; \">Pergunta " + perg.Ordem.ToString() + ": " + perg.Descricao + "</span>");
                    sHTML.Append("<span style=\"float: left; \">Tipo de resposta: " + perg.TipoResposta.GetDisplayName() + "</span>");
                }
                else
                {
                    sHTML.Append("<span style=\"width: 450px; float: left; \">Pergunta " + perg.Ordem.ToString() + ": " + perg.Descricao + "</span>");
                    sHTML.Append("<span style=\"float: left; \">Tipo de resposta: " + perg.TipoResposta.GetDisplayName() + "</span>");
                }
    
                sHTML.Append("<div class=\"pull-right action-buttons\">");
                sHTML.Append("<a class=\"red CustomTooltip\" href=\"#\" title=\"Excluir pergunta\" onclick=\"deletePergunta('" + perg.UniqueKey.ToString() + "', '" + perg.Ordem.ToString() + "'); return false;\">");
                sHTML.Append("  <i class=\"ace-icon fa fa-trash-o bigger-130\"></i>");
                sHTML.Append("</a>");
                sHTML.Append("</div>");
                
                sHTML.Append("</div>");


                if (perg._TipoResposta != null && perg._TipoResposta.TiposResposta != null && perg._TipoResposta.TiposResposta.Count > 0)
                {
                    sHTML.Append("<ol class=\"dd-list\">");
                    foreach (TipoRespostaItem item in perg._TipoResposta.TiposResposta)
                    {
                        sHTML.Append("<li class=\"dd-item\">");

                        sHTML.Append("<div class=\"dd2-content\" style=\"border-left: 2px solid " + arrCores[3] + ";\">");
                        sHTML.Append(item.Nome);

                        string url = @"\Pergunta\NovaPerguntaVinculada\";

                        sHTML.Append("<div class=\"pull-right action-buttons\">");
                        sHTML.Append("  <a class=\"blue CustomTooltip\" href=\"" + url + "?UKT=" + item.UniqueKey + "&UKP=" + perg.UniqueKey + "\" title=\"Nova pergunta vinculada\">");
                        sHTML.Append("      <i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
                        sHTML.Append("  </a>");
                        sHTML.Append("</div>");

                        sHTML.Append("</div>");

                        sHTML.Append("</li>");
                    }

                    sHTML.Append("</ol>");
                }

                sHTML.Append("</li>");
            }
            sHTML.Append("</ol>");

            return sHTML.ToString();
        }



    }
}