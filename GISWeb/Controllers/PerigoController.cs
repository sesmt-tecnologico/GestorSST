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
    public class PerigoController : BaseController
    {

        [Inject]
        public IBaseBusiness<Perigo> PerigoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuscarPerigos()
        {
            return PartialView("_BuscarPerigos", PerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList());
        }



        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Perigo entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PerigoBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Perigo '" + entidade.Descricao + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perigo") } });
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
            Perigo obj = PerigoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(UKPerigo));
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Perigo entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Perigo oTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (oTemp == null)
                    {
                        throw new Exception("Não foi possível localizar o perigo através de sua identificação.");
                    }
                    else
                    {
                        oTemp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        PerigoBusiness.Terminar(oTemp);

                        entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        PerigoBusiness.Inserir(entidade);

                        Extensions.GravaCookie("MensagemSucesso", "O perigo '" + entidade.Descricao + "' foi atualizado com sucesso.", 10);

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perigo") } });
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
                Guid idPerigo = Guid.Parse(id);
                Perigo oPerigo = PerigoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(idPerigo));
                if (oPerigo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o perigo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oPerigo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PerigoBusiness.Terminar(oPerigo);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O perigo '" + oPerigo.Descricao + "' foi excluído com sucesso." } });
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