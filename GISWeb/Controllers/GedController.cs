using GISCore.Business.Abstract;
using GISCore.Business.Concrete;
using GISModel.DTO.Ged;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class GedController : Controller
    {
        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        // GET: Ged

        public ActionResult Index(string id)
        {
            Guid UK = Guid.Parse(id);

            Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UK));

            var ged = new GedViewModel();
            ged.Empregado = oEmp;
            return View( ged );
        }
    }
}