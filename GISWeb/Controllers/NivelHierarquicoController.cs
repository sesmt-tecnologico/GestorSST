using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;


namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NivelHierarquicoController : BaseController
    {

        #region Inject

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public INivelHierarquicoBusiness NivelHierarquicoBusiness { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.NiveisHierarquicos = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(NivelHierarquico nivelHierarquico)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nivelHierarquico.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    NivelHierarquicoBusiness.Inserir(nivelHierarquico);

                    Extensions.GravaCookie("MensagemSucesso", "O nível '" + nivelHierarquico.Nome + "' foi cadastrado com sucesso.", 10);

                                                           
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "NivelHierarquico") } });
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
            Guid UKNivel = Guid.Parse(id);
            NivelHierarquico oNivel = NivelHierarquicoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKNivel));
            return View(oNivel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(NivelHierarquico nivel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nivel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    nivel.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    NivelHierarquicoBusiness.Alterar(nivel);

                    Extensions.GravaCookie("MensagemSucesso", "O nível '" + nivel.Nome + "' foi atualizado com sucesso.", 10);

                                      
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "NivelHierarquico") } });
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
                Guid UKNivel = Guid.Parse(id);

                NivelHierarquico nivel = NivelHierarquicoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKNivel));
                if (nivel == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o nível, pois a mesmo não foi localizado." } });

                nivel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                NivelHierarquicoBusiness.Terminar(nivel);

                List<NivelHierarquico> niveis = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O menu '" + nivel.Nome + "' foi excluído com sucesso.", Conteudo = RenderRazorViewToString("_ListagemNiveis", niveis) } });

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