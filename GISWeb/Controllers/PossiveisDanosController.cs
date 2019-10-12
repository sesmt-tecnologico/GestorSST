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

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PossiveisDanosController : BaseController
    {

        #region Inject

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }


        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.PossiveisDanos = PossiveisDanosBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.DescricaoDanos);

            return View();
        }

        public ActionResult Novo()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PossiveisDanos oPossiveisDanoso)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    PossiveisDanosBusiness.Inserir(oPossiveisDanoso);

                    TempData["MensagemSucesso"] = "O Possivel dano '" + oPossiveisDanoso.DescricaoDanos + "' foi cadastrado com sucesso!";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PossiveisDanos") } });

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


            return View(PossiveisDanosBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(PossiveisDanos oPossiveisDanos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PossiveisDanosBusiness.Alterar(oPossiveisDanos);

                    TempData["MensagemSucesso"] = "O Possivel Dano '" + oPossiveisDanos.DescricaoDanos + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PossiveisDanos") } });
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

        public ActionResult Excluir(string id)
        {
            ViewBag.PossiveisDanos = new SelectList(PossiveisDanosBusiness.Consulta.ToList(), "IDPossiveisDanos", "DescricaoDanos");
            return View(PossiveisDanosBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Terminar(string IDPossiveisDanos)
        {

            try
            {
                PossiveisDanos oPossiveisDanos = PossiveisDanosBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDPossiveisDanos));
                if (oPossiveisDanos == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Possivel Dano, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oPossiveisDanos.DataExclusao = DateTime.Now;
                    oPossiveisDanos.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PossiveisDanosBusiness.Alterar(oPossiveisDanos);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Possivel Dano '" + oPossiveisDanos.DescricaoDanos + "' foi excluído com sucesso." } });
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