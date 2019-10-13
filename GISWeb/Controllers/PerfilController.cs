using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PerfilController : BaseController
    {

        #region Inject

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Perfis = PerfilBusiness.Consulta.ToList();

            return View();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Perfil Perfil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Perfil.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PerfilBusiness.Inserir(Perfil);

                    Extensions.GravaCookie("MensagemSucesso", "O perfil '" + Perfil.Nome + "' foi cadastrado com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perfil") } });
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

            Guid UKPerfil = Guid.Parse(id);
            Perfil obj = PerfilBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(UKPerfil));

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Perfil Perfil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PerfilBusiness.Alterar(Perfil);

                    Extensions.GravaCookie("MensagemSucesso", "O perfil '" + Perfil.Nome + "' foi atualizado com sucesso.", 10);
                    
                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perfil") } });
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

    }
}