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
using System.Collections.Generic;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PlanoDeAcaoController : BaseController
    {

        #region Inject

        [Inject]
        public IPlanoDeAcaoBusiness PlanoDeAcaoBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {

            ViewBag.PlanoDeAcao = PlanoDeAcaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d => d.ID).ToList();

            ViewBag.AtividadeEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

            var IDAtividadeEstab = from AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                   join PA in PlanoDeAcaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                   on AE.ID equals PA.Identificador
                                   select new AtividadesDoEstabelecimento()
                                   {
                                       ID = AE.ID,
                                       DescricaoDestaAtividade = AE.DescricaoDestaAtividade
                                   };

            ViewBag.AtivEstab = IDAtividadeEstab;



            ViewBag.DataAtual = DateTime.Now; 

            return View();
        }

        public ActionResult ListarPlanoDeAcao()
        {

            try
            {
               List<PlanoDeAcao> oPlanoDeAcao = PlanoDeAcaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

                return View("ListarPlanoDeAcao", oPlanoDeAcao) ;

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

        public ActionResult Detalhes(string IDPlanoDeAcao)
        {
            //ViewBag.PlanoDeAcao = PlanoDeAcaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && d.IDPlanoDeAcao.Equals(IDPlanoDeAcao)).ToList();
            ViewBag.data = "31/12/9999";
            PlanoDeAcao oPlanoDeAcao = PlanoDeAcaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDPlanoDeAcao));
            ViewBag.DataExclusao = oPlanoDeAcao.DataExclusao.ToString("dd/MM/yyyy");

            try
            {

                return Json(new { data = RenderRazorViewToString("_Detalhes", oPlanoDeAcao) });
                
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

        
            


        public ActionResult CriarPlanoDeAcao(string IDIdentificador)
        {

            Guid Ident = Guid.Parse(IDIdentificador);

            ViewBag.IDIentificador = IDIdentificador;
            ViewBag.PlanoDeAcao = PlanoDeAcaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToString();
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.ToList(), "Sigla", "Sigla");
           PlanoDeAcao oPlanoDeAcao = PlanoDeAcaoBusiness.Consulta.FirstOrDefault(p=>p.Identificador.Equals(Ident));
            if (PlanoDeAcaoBusiness.Consulta.Any(u =>string.IsNullOrEmpty(u.UsuarioExclusao) && (u.Identificador.Equals(Ident))))
                
            {
                //return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Detalhes", "PlanoDeAcao", new { IDPlanoDeAcao = IDIdentificador }) } });
                return Json(new { resultado = new RetornoJSON() { Erro = "Já existe um Plano de Ação em andamento para este Risco!" } });
            }

            if(MedidasDeControleBusiness.Consulta.Any(u=>string.IsNullOrEmpty(u.UsuarioExclusao) &&(u.IDTipoDeRisco.Equals(Ident))))
            {
                return Json(new { resultado = new RetornoJSON() { Erro = "Já existe controle para este risco!" } });
            }
            try
            {                
                
                    return Json(new { data = RenderRazorViewToString("_PlanoDeAcao", oPlanoDeAcao) });


                
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

        public ActionResult Novo(string IDIdentificador)
        {

            ViewBag.IDIentificador = IDIdentificador;
            ViewBag.PlanoDeAcao = PlanoDeAcaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PlanoDeAcao oPlanoDeAcao, string IdentificadorID, string IDDepartamento)
        {
            oPlanoDeAcao.Identificador = Guid.Parse(IdentificadorID);
            oPlanoDeAcao.status = IDDepartamento;
            if (ModelState.IsValid)
            {
                try
                {
                    PlanoDeAcaoBusiness.Inserir(oPlanoDeAcao);

                    Extensions.GravaCookie("MensagemSucesso", "O Plano de Ação'" + oPlanoDeAcao.DescricaoDoPlanoDeAcao + "' foi cadastrado com sucesso!", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PlanoDeAcao") } });

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
        public ActionResult TerminarComRedirect(string IDplano, string IDidentificador)
        {

            try
            {
                MedidasDeControleExistentes oMedidasDeControle = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(IDidentificador));

                PlanoDeAcao oPlanoDeAcao = PlanoDeAcaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDplano));
                if (oPlanoDeAcao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível Excluir este plano!" } });
                }
                if (oMedidasDeControle == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Você deve criar um controle antes de encerrar o Plano de Ação!" } });
                }

                else
                {
                    oPlanoDeAcao.DataExclusao = DateTime.Now;
                    oPlanoDeAcao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    oPlanoDeAcao.status = "Entregue";
                    PlanoDeAcaoBusiness.Alterar(oPlanoDeAcao);

                    Extensions.GravaCookie("MensagemSucesso", "O Plano '" + oPlanoDeAcao.DescricaoDoPlanoDeAcao + "' foi encerrado com sucesso.", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PlanoDeAcao", new { id = IDplano }) } });
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

        public ActionResult Edicao(string id)
        {
            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Atividade oAtividadeDeRisco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AtividadeBusiness.Alterar(oAtividadeDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oAtividadeDeRisco.Descricao + "' foi atualizada com sucesso.", 10);

                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AtividadeDeRisco") } });
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
            //ViewBag.Cargo = new SelectList(CargoBusiness.Consulta.ToList(), "IDCargo", "NomeDoCargo");
            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        public ActionResult Excluir(Atividade oAtividadeDeRisco)
        {

            try
            {

                if (oAtividadeDeRisco == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a Atividade, pois a mesma não foi localizada." } });
                }
                else
                {

                    //oDepartamento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    // oDepartamento.UKUsuarioDemissao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    oAtividadeDeRisco.UsuarioExclusao = "Antonio Henriques";
                    oAtividadeDeRisco.DataExclusao = DateTime.Now;
                    AtividadeBusiness.Excluir(oAtividadeDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oAtividadeDeRisco.Descricao + "' foi excluida com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AtividadeDeRisco") } });
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
        public ActionResult _Upload()
        {
            try
            {
                return PartialView("_Upload");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

    }
}