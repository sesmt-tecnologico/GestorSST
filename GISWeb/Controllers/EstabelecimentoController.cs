﻿using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO;
using GISWeb.Infraestrutura.Provider.Concrete;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.DTO.Estabelecimento;
using System.Collections.Generic;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EstabelecimentoController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<REL_EstabelecimentoDepartamento> REL_EstabelecimentoDepartamentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion


        public ActionResult Index()
        {


            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            ViewBag.Departamento = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Lista()
        {
            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();



            return View();
        }

        public ActionResult ListarEstabelecimentoPorDepartamento(string idDepartamento)
        {

            return Json(new { resultado = EstabelecimentoBusiness.Consulta.Where(p => p.ID.Equals(idDepartamento)).ToList().OrderBy(p => p.ID) });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(EstabelecimentoViewModel entidade)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (entidade?.Departamento?.Count > 0)
                    {

                        Estabelecimento obj = new Estabelecimento()
                        {
                            UniqueKey = Guid.NewGuid(),
                            NomeCompleto = entidade.NomeCompleto,
                            Codigo = entidade.Codigo,
                            Descricao = entidade.Descricao,
                            TipoDeEstabelecimento = entidade.TipoDeEstabelecimento,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login

                        };
                        EstabelecimentoBusiness.Inserir(obj);

                        if (entidade?.Departamento?.Count > 0)
                        {
                            foreach (string dep in entidade.Departamento)
                            {
                                REL_EstabelecimentoDepartamento rel = new REL_EstabelecimentoDepartamento()
                                {

                                    UKEstabelecimento = obj.UniqueKey,
                                    UKDepartamento = Guid.Parse(dep),
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login


                                };
                                REL_EstabelecimentoDepartamentoBusiness.Inserir(rel);
                            }

                        }
                    }
                    else
                    {
                        throw new Exception("É necessário informar pelo menos um departamento para prosseguir com o cadastro do contrato.");
                    }

                    Extensions.GravaCookie("MensagemSucesso", "O Estabelecimento '" + entidade.NomeCompleto + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });

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
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarEstabelecimento(PesquisaEstabelecimentoViewModel entidade)
        {
            try
            {


                var dep = from r in REL_EstabelecimentoDepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          join e in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on r.UKEstabelecimento equals e.UniqueKey
                          join d in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on r.UKDepartamento equals d.UniqueKey

                          where r.UKEstabelecimento.Equals(entidade.IDEstabelecimento)

                          select new PesquisaEstabelecimentoViewModel()
                          {
                              UKDepartamento = d.UniqueKey,
                              Codigo = d.Codigo,
                              NomeEstabelecimento = e.NomeCompleto,
                              TipoDeEstabelecimento = e.TipoDeEstabelecimento,
                              IDEstabelecimento = e.ID


                          };

                List<PesquisaEstabelecimentoViewModel> lista = dep.ToList();

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }

        public ActionResult Edicao(string id)
        {

            Guid ID = Guid.Parse(id);
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.ToList(), "IDDepartamento", "Sigla");
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(ID)));

            //Guid Guid = Guid.Parse(id);

            //EdicaoEstabelecimentoViewModel obj = null;

            //Estabelecimento oEstabelecimento = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(Guid));
            //if (oEstabelecimento != null)
            //{
            //    ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.ToList(), "IDDepartamento", "Sigla");
            //    ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            //    obj = new EdicaoEstabelecimentoViewModel()
            //    {

            //        IDEstabelecimento = oEstabelecimento.ID,
            //        NomeEstabelecimento = oEstabelecimento.NomeCompleto,
            //        TipoDeEstabelecimento = oEstabelecimento.TipoDeEstabelecimento,

            //    };

            //    //REL_EstabelecimentoDepartamento rel_1 = REL_EstabelecimentoDepartamentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEstabelecimento.Equals(obj.UniqueKey));

            //    //obj.UKDepartamento = rel_1.UniqueKey;



            //}

            //return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid)));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Estabelecimento oEstabelecimento)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Estabelecimento dep = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(oEstabelecimento.UniqueKey));
                    if (dep == null)
                    {


                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Estabelecimento, pois a mesmo não foi localizado." } });
                    }

                    else
                    {
                        dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        EstabelecimentoBusiness.Terminar(dep);


                        Estabelecimento objEstab = new Estabelecimento()
                        {
                            UniqueKey = oEstabelecimento.UniqueKey,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            TipoDeEstabelecimento = oEstabelecimento.TipoDeEstabelecimento,
                            Codigo = oEstabelecimento.Codigo,
                            Descricao = oEstabelecimento.Descricao,
                            NomeCompleto = oEstabelecimento.NomeCompleto
                        };
                        EstabelecimentoBusiness.Inserir(objEstab);


                        Extensions.GravaCookie("MensagemSucesso", "O Estbelecimento '" + oEstabelecimento.NomeCompleto + "' foi atualizado com sucesso.", 10);


                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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

        public ActionResult Excluir(string id)
        {
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");
            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Excluir(Estabelecimento oEstabelecimento)
        {

            try
            {

                if (oEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Estabelecimento, pois o mesmo não foi localizado." } });
                }
                else
                {

                    //oEstabelecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    //oEstabelecimento.UKUsuarioDemissao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    oEstabelecimento.UsuarioExclusao = "Antonio Henriques";
                    oEstabelecimento.DataExclusao = DateTime.Now;
                    EstabelecimentoBusiness.Excluir(oEstabelecimento);

                    Extensions.GravaCookie("MensagemSucesso", "O Estabelecimento '" + oEstabelecimento.NomeCompleto + "' foi excluido com sucesso.", 10);

                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
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
        public ActionResult Terminar(string id)
        {
            Guid ID = Guid.Parse(id);
            try
            {
                Estabelecimento dep = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(ID));
                if (dep == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o departamento, pois a mesmo não foi localizado." } });

                dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                EstabelecimentoBusiness.Terminar(dep);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O departamento '" + dep.NomeCompleto + "' foi excluído com sucesso." } });

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