using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LinkController : BaseController
    {

        [Inject]
        public IBaseBusiness<Link> LinkBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListarLinks()
        {
            try
            {
                List<Link> lista = LinkBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
                return PartialView("_ListarLinks", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }


        public ActionResult Novo()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Link entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    LinkBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "O link '" + entidade.Nome + "' foi cadastrado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Link") } });
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
        public ActionResult Terminar(string UK)
        {
            try
            {
                Guid guidUK = Guid.Parse(UK);
                Link obj = LinkBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(guidUK));
                if (obj == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o link, pois o mesmo não foi localizado." } });
                }
                else
                {
                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    LinkBusiness.Terminar(obj);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O link '" + obj.Nome + "' foi excluído com sucesso." } });
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