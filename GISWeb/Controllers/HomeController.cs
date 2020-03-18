using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {

        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }



        public ActionResult Index()
        {
            if (AutorizacaoProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Empregado")).Count() > 0)
            {
                Empregado emp = EmpregadoBusiness.Consulta.FirstOrDefault(a =>
                                        string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                        a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(AutorizacaoProvider.UsuarioAutenticado.Login.ToUpper().Trim()));
                if (emp != null)
                {
                    return RedirectToAction("Desktop", "Empregado", new { id = emp.UniqueKey });
                }
            }

            return View();
        }


        public ActionResult Sobre()
        {
            return View();
        }
    }
}