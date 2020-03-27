using GISWeb.Infraestrutura.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IndicadoresController : Controller
    {
        // GET: Indicadores
        public ActionResult Indicadores()
        {

            ViewBag.midia = "https://player.vimeo.com/video/394755088";
            return View();
        }


        public ActionResult TaxaFrequenciaTotal()
        {
                        

            return View();
        }

        public ActionResult CustoDoAcidente()
        {


            return View();
        }

        public ActionResult AvalPsicossocial()
        {


            return View();
        }
        public ActionResult DocumenstosDeSaude()
        {


            return View();
        }

        public ActionResult Documentos()
        {


            return View();
        }


    }
}