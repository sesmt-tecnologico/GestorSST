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
using System.Collections.Generic;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class RiscoController : BaseController
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
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        

        #endregion


        public ActionResult Index()
        {
            ViewBag.Risco = RiscoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Nome);

            return View();
        }

        public ActionResult Novo()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Risco oRisco)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    RiscoBusiness.Inserir(oRisco);

                    Extensions.GravaCookie("MensagemSucesso", "O evento '" + oRisco.Nome + "' foi cadastrado com sucesso!", 10);



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
            var ID = Guid.Parse(id);

            return View(RiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Risco oRisco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RiscoBusiness.Alterar(oRisco);

                    Extensions.GravaCookie("MensagemSucesso", "O Risco '" + oRisco.Nome + "' foi atualizado com sucesso.", 10);


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

        public ActionResult Excluir(string id)
        {
            ViewBag.Risco = new SelectList(RiscoBusiness.Consulta.ToList(), "ID", "Risco");
            return View(RiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Terminar(string id)
        {

            var ID = Guid.Parse(id);

            try
            {
                Risco oRisco = RiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID));
                if (oRisco == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Risco, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oRisco.DataExclusao = DateTime.Now;
                    oRisco.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    RiscoBusiness.Alterar(oRisco);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Risco '" + oRisco.Nome + "' foi excluído com sucesso." } });
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



        [RestritoAAjax]
        public ActionResult BuscarRiscoForAutoComplete(string key)
        {
            try
            {
                List<string> riscosAsString = new List<string>();
                List<Risco> lista = RiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Risco com in lista)
                    riscosAsString.Add(com.Nome);

                return Json(new { Result = riscosAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarRiscoForAutoComplete(string key)
        {
            try
            {
                Risco item = RiscoBusiness.Consulta.FirstOrDefault(a => a.Nome.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }


    }
}