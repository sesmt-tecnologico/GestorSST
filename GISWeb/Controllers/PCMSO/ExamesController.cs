using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades.PCMSO;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ExamesController : BaseController
    {

        #region
        [Inject]
        public IBaseBusiness<Exames> ExamesBusines { get; set; }
        #endregion
        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        // GET: Exames
        public ActionResult Index()
        {
            ViewBag.Exames = ExamesBusines.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();


            return View();
        }

        public ActionResult Novo()
        {



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Exames oExames)
        {
            if (ModelState.IsValid)
            {


                try
                {

                    oExames.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ExamesBusines.Inserir(oExames);
                    Extensions.GravaCookie("MensagemSucesso", "Exame registrado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Exames") } });

                }catch (Exception ex)
                {
                    if (ex.GetBaseException() == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    }
                }

            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
                                   
        }

    }
}
