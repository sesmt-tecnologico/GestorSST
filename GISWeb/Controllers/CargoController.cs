﻿using GISCore.Business.Abstract;
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
    public class CargoController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Cargo = CargoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).Distinct().ToList();

            return View();
        }

        public ActionResult Novo()
        {
            var Dir = from a in DiretoriaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      join b in EmpresaBusiness.Consulta.ToList()
                      on a.IDEmpresa equals b.ID                      
                      select new Diretoria()
                      {
                          ID = a.ID,
                          Sigla = a.Sigla
                      };

            ViewBag.Diretoria = new SelectList(Dir.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDiretoria", "Sigla");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Cargo oCargo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    CargoBusiness.Inserir(oCargo);

                    Extensions.GravaCookie("MensagemSucesso", "O Cargo '" + oCargo.NomeDoCargo + "' foi cadastrado com sucesso!", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo", new { id = oCargo.ID })}});

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
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            return View(CargoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Cargo oCargo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CargoBusiness.Alterar(oCargo);

                    Extensions.GravaCookie("MensagemSucesso", "O Cargo '" + oCargo.NomeDoCargo + "' foi atualizado com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo") } });
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
        public ActionResult TerminarComRedirect(string IDCargo)
        {

            try
            {
                Cargo oCargo = CargoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDCargo));
                if (oCargo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Cargo." } });
                }
                else
                {
                    oCargo.DataExclusao = DateTime.Now;
                    oCargo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    CargoBusiness.Alterar(oCargo);


                    Extensions.GravaCookie("MensagemSucesso", "O Cargo'" + oCargo.NomeDoCargo + "' foi excluido com sucesso.", 10);
                    
                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo", new { id = IDCargo }) } });
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