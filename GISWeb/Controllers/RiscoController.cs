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

        [Inject]
        public IBaseBusiness<REL_PerigoRisco> REL_PerigoRiscoBusiness { get; set; }



        #endregion


        public ActionResult Index()
        {
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

                    Risco risk = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Trim().ToUpper().Equals(oRisco.Nome.Trim().ToUpper()) && a.Template);
                    if (risk != null)
                        throw new Exception("Já existe um risco com este nome cadastrado no sistema.");


                    oRisco.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    oRisco.Template = true;
                    RiscoBusiness.Inserir(oRisco);

                    Extensions.GravaCookie("MensagemSucesso", "O risco '" + oRisco.Nome + "' foi cadastrado com sucesso!", 10);

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

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularRiscoPerigo(string UKPerigo)
        {

            ViewBag.UKPerigo = UKPerigo;

            return PartialView("_VincularRisco");
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularRisco(string UKPerigo, string UKRisco)
        {

            try
            {
                Guid UK_Perigo = Guid.Parse(UKPerigo);

                if (string.IsNullOrEmpty(UKRisco))
                    throw new Exception("Não foi possível localizar o Risco.");

                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Nenhum Perigo para vincular.");


                if (UKRisco.Contains(","))
                {
                    foreach (string ativ in UKRisco.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            Risco pTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_PerigoRiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(pTemp.UniqueKey) && a.UKPerigo.Equals(UK_Perigo)).Count() == 0)
                                {
                                    REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                                    {
                                        UKPerigo = UK_Perigo,
                                        UKRisco = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Risco pTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(UKRisco.Trim()));

                    if (pTemp != null)
                    {
                        if (REL_PerigoRiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(pTemp.UniqueKey) && a.UKPerigo.Equals(UK_Perigo)).Count() == 0)
                        {
                            REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                            {
                                UKPerigo = UK_Perigo,
                                UKRisco = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Risco vinculado ao perigo com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }




        [RestritoAAjax]
        public ActionResult BuscarRiscoForAutoComplete(string key)
        {
            try
            {
                List<string> riscosAsString = new List<string>();
                List<Risco> lista = RiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.ToUpper().Contains(key.ToUpper()) && a.Template).ToList();

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
                Risco item = RiscoBusiness.Consulta.FirstOrDefault(a => a.Nome.ToUpper().Equals(key.ToUpper()) && a.Template);

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