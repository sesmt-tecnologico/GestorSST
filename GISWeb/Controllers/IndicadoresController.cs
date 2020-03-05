using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
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



    }
}