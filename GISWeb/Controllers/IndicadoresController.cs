﻿using GISCore.Business.Abstract;
using GISModel.DTO.Indicadores;
using GISModel.DTO.Resposta;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.Quest;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IndicadoresController : Controller
    {
        #region Inject

        [Inject]
        public IBaseBusiness<Resposta> RespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: Indicadores
        public ActionResult Indicadores()
        {

            ViewBag.midia = "https://player.vimeo.com/video/394755088";
            return View();
        }


        public ActionResult TaxaFrequenciaTotal()
        {
                        

            return View();
        }

        public ActionResult CustoDoAcidente()
        {


            return View();
        }

        public ActionResult AvalPsicossocial()
        {


            bool isSuperAdmin = CustomAuthorizationProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Super Administrador")).Count() > 0;
            if (isSuperAdmin)
            {
                ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.NomeFantasia).ToList();
                return View();
            }
            else
            {
                bool isAdmin = CustomAuthorizationProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Administrador")).Count() > 0;
                if (isAdmin)
                {
                    VMPesquisaResposta obj = new VMPesquisaResposta();

                    string sql = @"select e.UniqueKey, e.NomeFantasia
                                   from tbPerfil p, tbUsuarioPerfil up, tbUsuario u, tbEmpresa e
                                   where p.Nome = 'Administrador' and p.DataExclusao = '9999-12-31 23:59:59.997' and
	                                     p.UniqueKey = up.UKPerfil and up.DataExclusao = '9999-12-31 23:59:59.997' and up.UKUsuario = '" + CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey.ToString() + @"' and
	                                     up.UKUsuario = u.UniqueKey and u.DataExclusao = '9999-12-31 23:59:59.997' and
	                                     u.UKEmpresa = e.UniqueKey and e.DataExclusao = '9999-12-31 23:59:59.997'";

                    List<Empresa> lista = new List<Empresa>();
                    DataTable result = RespostaBusiness.GetDataTable(sql);
                    if (result.Rows.Count > 0)
                    {
                        foreach (DataRow row in result.Rows)
                        {
                            lista.Add(new Empresa()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                NomeFantasia = row["NomeFantasia"].ToString()
                            });

                            obj.UKEmpresa = Guid.Parse(row["UniqueKey"].ToString());
                        }
                    }

                    ViewBag.Empresas = lista;

                    return View(obj);
                }

                throw new Exception("O usuário autenticado no sistema não possui permissão para acessar esta tela.");
            }
                                
           
        }
            
               


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pesquisar(VMIndicadorResposta entidade)
        {
            try
            {

                string sFrom = string.Empty;
                string sWhere = string.Empty;

                //if (entidade.UKEmpresa == null)
                //    throw new Exception("Selecione uma empresa para prosseguir na pesquisa.");

                //if (entidade.UKQuestionario == null && entidade.UKEmpregado == null)
                //    throw new Exception("Selecione um questionário ou um empregado para prosseguir na pesquisa.");


                //if ((entidade.UKEmpresa == null || entidade.UKEmpresa == Guid.Empty) &&
                //    (entidade.UKEmpregado == null || entidade.UKEmpregado == Guid.Empty) &&
                //    (entidade.UKQuestionario == null || entidade.UKQuestionario == Guid.Empty) &&
                //    string.IsNullOrEmpty(entidade.Periodo))
                //    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");



                if (entidade.UKEmpresa != null && entidade.UKEmpresa != Guid.Empty)
                    sWhere += " and r.UKEmpresa = '" + entidade.UKEmpresa.ToString() + "'";

                if (entidade.UKQuestionario != null && entidade.UKQuestionario != Guid.Empty)
                    sWhere += " and r.UKQuestionario = '" + entidade.UKQuestionario.ToString() + "'";

                if (entidade.UKEmpregado != null && entidade.UKEmpregado != Guid.Empty)
                    sWhere += " and r.UKEmpregado = '" + entidade.UKEmpregado + "'";

                if (!string.IsNullOrEmpty(entidade.Periodo))
                {
                    string dt1 = entidade.Periodo.Substring(0, entidade.Periodo.IndexOf(" - "));
                    string dt2 = entidade.Periodo.Substring(entidade.Periodo.IndexOf(" - ") + 3);

                    DateTime date1 = DateTime.ParseExact(dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime date2 = DateTime.ParseExact(dt2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    sWhere += " AND r.DataInclusao between '" + date1.ToString("yyyy/MM/dd") + " 00:00:00.001' and '" + date2.ToString("yyyy/MM/dd") + " 23:59:59.997'";
                }



                string sql = @"select top 100 emp.UniqueKey as UKEmpregado, emp.Nome,
	                                          q.Nome as Questionario, q.TipoQuestionario,
	                                          r.UniqueKey, r.DataInclusao, r.UKObjeto,
	                                          (select FonteGeradora from tbFonteGeradoraDeRisco where UniqueKey = r.UKObjeto) as FonteGeradora,
	                                          p.UniqueKey as UKPergunta, p.Descricao as Pergunta, ri.Resposta
                                       from tbResposta r, tbQuestionario q, tbEmpregado emp, tbRespostaItem ri, tbPergunta p
                                       where r.DataExclusao = '9999-12-31 23:59:59.997' and
	                                         r.UKQuestionario = q.UniqueKey and r.DataExclusao = '9999-12-31 23:59:59.997' and
	                                         r.UKEmpregado = emp.UniqueKey and emp.DataExclusao = '9999-12-31 23:59:59.997' and
	                                         r.UniqueKey = ri.UKResposta and ri.DataExclusao = '9999-12-31 23:59:59.997' and
	                                         ri.UKPergunta = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997' " + sWhere + @"
                                       order by emp.Nome, r.DataInclusao desc, q.Nome, r.UniqueKey, p.Ordem";

                var total = sql.Count();

                List<VMIndicadorPesquisaEmpregado> lista = new List<VMIndicadorPesquisaEmpregado>();
                List<VMIndicadorPesquisaPergunta> ListaPergunta = new List<VMIndicadorPesquisaPergunta>();

                DataTable result = RespostaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {

                        VMIndicadorPesquisaEmpregado oEmp = lista.FirstOrDefault(a => a.UniqueKey.Equals(row["UKEmpregado"].ToString()));
                        if (oEmp == null)
                        {
                            oEmp = new VMIndicadorPesquisaEmpregado()
                            {
                                UniqueKey = row["UKEmpregado"].ToString(),
                                Nome = row["Nome"].ToString(),
                                Questionarios = new List<VMIndicadorPesquisaQuestionario>()
                            };

                            if (!string.IsNullOrEmpty(row["Questionario"].ToString()))
                            {

                                VMIndicadorPesquisaQuestionario oQuest = new VMIndicadorPesquisaQuestionario()
                                {
                                    Nome = row["Questionario"].ToString(),
                                    TipoQuestionario = int.Parse(row["TipoQuestionario"].ToString()),
                                    UKObjeto = row["UKObjeto"].ToString(),
                                    UKResposta = row["UniqueKey"].ToString(),
                                    Objeto = row["FonteGeradora"].ToString(),
                                    DataEnvio = (DateTime)row["DataInclusao"],
                                    Perguntas = new List<VMIndicadorPesquisaPergunta>()
                                };

                                if (!string.IsNullOrEmpty(row["Pergunta"].ToString()))
                                {
                                    VMIndicadorPesquisaPergunta oPergunta = new VMIndicadorPesquisaPergunta()
                                    {
                                        UKPergunta = row["UKPergunta"].ToString(),
                                        Pergunta = row["Pergunta"].ToString(),
                                        Resposta = row["Resposta"].ToString(),

                                    };

                                    oQuest.Perguntas.Add(oPergunta);
                                }

                                oEmp.Questionarios.Add(oQuest);

                            }

                            lista.Add(oEmp);
                        }
                        else
                        {

                            VMIndicadorPesquisaQuestionario oQuest = oEmp.Questionarios.FirstOrDefault(a => a.UKResposta.Equals(row["UniqueKey"].ToString()));
                            if (oQuest == null)
                            {
                                oQuest = new VMIndicadorPesquisaQuestionario()
                                {
                                    Nome = row["Questionario"].ToString(),
                                    TipoQuestionario = int.Parse(row["TipoQuestionario"].ToString()),
                                    UKObjeto = row["UKObjeto"].ToString(),
                                    UKResposta = row["UniqueKey"].ToString(),
                                    Objeto = row["FonteGeradora"].ToString(),
                                    DataEnvio = (DateTime)row["DataInclusao"],
                                    Perguntas = new List<VMIndicadorPesquisaPergunta>()
                                };

                                if (!string.IsNullOrEmpty(row["Pergunta"].ToString()))
                                {
                                    VMIndicadorPesquisaPergunta oPergunta = new VMIndicadorPesquisaPergunta()
                                    {
                                        UKPergunta = row["UKPergunta"].ToString(),
                                        Pergunta = row["Pergunta"].ToString(),
                                        Resposta = row["Resposta"].ToString()
                                    };

                                    oQuest.Perguntas.Add(oPergunta);
                                }

                                oEmp.Questionarios.Add(oQuest);
                            }
                            else
                            {
                                VMIndicadorPesquisaPergunta oPergunta = ListaPergunta.FirstOrDefault(a => a.UKPergunta.Equals(row["UKPergunta"].ToString()));

                                //if (!string.IsNullOrEmpty(row["Pergunta"].ToString()))
                                if (oPergunta == null)
                                {
                                    oPergunta = new VMIndicadorPesquisaPergunta()
                                    {
                                        UKPergunta = row["UKPergunta"].ToString(),
                                        Pergunta = row["Pergunta"].ToString(),
                                        Resposta = row["Resposta"].ToString()
                                    };

                                    oQuest.Perguntas.Add(oPergunta);


                                    
                                }

                            }
                            
                           
                        }
                       
                    }
                }
               

                

                return PartialView("_IndicadoresAVP",lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }



        public ActionResult DocumenstosDeSaude()
        {


            return View();
        }

        public ActionResult Documentos()
        {


            return View();
        }


    }
}