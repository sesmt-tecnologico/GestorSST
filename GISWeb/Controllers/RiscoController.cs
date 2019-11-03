using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class RiscoController : BaseController
    {

        [Inject]
        public IBaseBusiness<Risco> RiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuscarRiscos()
        {
            return PartialView("_BuscarRiscos", RiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList());
        }


        public ActionResult Novo(string UKWorkArea)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Risco entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    RiscoBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Risco '" + entidade.Nome + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Risco") } });
                }
                catch (Exception ex)
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



        public ActionResult Edicao(string id)
        {
            Guid UKPerigo = Guid.Parse(id);
            Risco obj = RiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(UKPerigo));
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Risco entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Risco oTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (oTemp == null)
                    {
                        throw new Exception("Não foi possível localizar o risco através de sua identificação.");
                    }
                    else
                    {

                        oTemp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        RiscoBusiness.Terminar(oTemp);

                        entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        RiscoBusiness.Inserir(entidade);

                        Extensions.GravaCookie("MensagemSucesso", "O risco '" + entidade.Nome + "' foi atualizado com sucesso.", 10);

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Risco") } });
                    }
                }
                catch (Exception ex)
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



        [HttpPost]
        public ActionResult Terminar(string id)
        {
            try
            {
                Guid idRisco = Guid.Parse(id);
                Risco oRisco = RiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(idRisco));
                if (oRisco == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o perigo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oRisco.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    RiscoBusiness.Terminar(oRisco);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O risco '" + oRisco.Nome + "' foi excluído com sucesso." } });
                }
            }
            catch (Exception ex)
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

    }
}