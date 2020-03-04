using GISModel.DTO;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ListaEnumeradaController : BaseController
    {
        
        public ActionResult ClassificacaoDaMedida()
        {

            List<EntidadeBase> listagem = (from EClassificacaoDaMedia e in Enum.GetValues(typeof(EClassificacaoDaMedia))
                                           select new EntidadeBase
                                           {
                                                 id = (int)e,
                                                 name = e.ToString()
                                           }).ToList();

            return View(listagem);
        }

    }
}