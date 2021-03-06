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
    public class EventoPerigosoController : BaseController
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
            ViewBag.EventoPerigoso = EventoPerigosBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p=>p.Descricao);

            return View();
        }

        public ActionResult Novo()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(EventoPerigoso oEventoPerigoso)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    EventoPerigosoBusiness.Inserir(oEventoPerigoso);

                    Extensions.GravaCookie("MensagemSucesso", "O evento '" + oEventoPerigoso.Descricao + "' foi cadastrado com sucesso!", 10);


                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EventoPerigoso") } });

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

            return View(EventoPerigosoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(EventoPerigoso oEventoPerigoso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EventoPerigosoBusiness.Alterar(oEventoPerigoso);

                    Extensions.GravaCookie("MensagemSucesso", "O Evento Perigoso '" + oEventoPerigoso.Descricao + "' foi atualizado com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EventoPerigoso") } });
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
            ViewBag.EventoPerigoso = new SelectList(EventoPerigosoBusiness.Consulta.ToList(), "IDEventoPerigoso", "Descricao");
            return View(EventoPerigosoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Terminar(string IDEventoPerigoso)
        {

            var ID = Guid.Parse(IDEventoPerigoso);

            try
            {
                EventoPerigoso oEventoPerigoso = EventoPerigosoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID));
                if (oEventoPerigoso == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Evento Perigoso, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oEventoPerigoso.DataExclusao = DateTime.Now;
                    oEventoPerigoso.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EventoPerigosoBusiness.Alterar(oEventoPerigoso);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Evento Perigoso '" + oEventoPerigoso.Descricao + "' foi excluído com sucesso." } });
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