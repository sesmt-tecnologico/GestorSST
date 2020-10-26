﻿using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.AnaliseDeRisco;
using GISModel.Entidades.Quest;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.AnaliseRisco
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AnaliseDeRiscoController : BaseController
    {
        #region Inject

        [Inject]
        public IBaseBusiness<Questionario> QuestionarioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Atividade> AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Usuario> UsuarioBusiness { get; set; }


        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Pergunta> PerguntaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Resposta> RespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<RespostaItem> RespostaItemBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }
        [Inject]
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AnaliseDeRiscoEmpregados> REL_AnaliseDeRiscoEmpregadosBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: AnaliseDeRisco
        public ActionResult Index()
        {
            

            var relacao = from rel in REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                          && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                          join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on rel.UKEmpregado equals e.UniqueKey
                          where rel.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                          select new Empregado()
                          {
                              UniqueKey = rel.UniqueKey,
                              Nome = e.Nome,
                              CPF =e.CPF,
                              UsuarioInclusao = rel.UsuarioInclusao,
                              DataInclusao = rel.DataInclusao
                          };
            var data = DateTime.Now.Date;

            List<Empregado> emp = new List<Empregado>();

            foreach(var item in relacao)
            {
                if(item.DataInclusao.Date == data)
                {
                    emp.Add(item);
                }

            }

            REL_AnaliseDeRiscoEmpregados AR = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
            && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));
            if(AR != null)
            {
                ViewBag.Supervisor = AR.UsuarioInclusao;

                ViewBag.UKSupervisor = AR.UKEmpregado;
                ViewBag.Registro = AR.Registro;

                string NumRegist = Convert.ToString(AR.Registro);

                string NR = NumRegist.Substring(0,8);

                ViewBag.Nregist = NR;


            }


            ViewBag.Relacao = emp;

            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();




            return View();
        }
        public ActionResult VincularNome()
        {

           var reg = Guid.NewGuid();

            ViewBag.Registro = reg;

            
            



            // Random radNum = new Random();

            //ViewBag.Registro = radNum.Next();

            ViewBag.Empregados = EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return PartialView("_VincularNome");
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularNomes(string UKNome, string UKRegistro)
        {

            try
            {
                //Guid UK_Registro = Guid.Parse(UKRegistro);               

                if (string.IsNullOrEmpty(UKNome))
                    throw new Exception("Nenhum Nome para vincular.");
                if (string.IsNullOrEmpty(UKRegistro))
                    throw new Exception("Nenhum Registro Encontrado.");


                if (UKNome.Contains(","))
                {
                    foreach (string nom in UKNome.Split(','))
                    {
                        if (!string.IsNullOrEmpty(nom.Trim()))
                        {
                            Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(nom.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
                                {
                                    REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
                                    {
                                        Registro = UKRegistro,
                                        UKEmpregado = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(UKNome.Trim()));

                    if (pTemp != null)
                    {
                        if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
                        {
                            REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
                            {
                                Registro = UKRegistro,
                                UKEmpregado = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Nome(s) registrado com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }



        public ActionResult BuscarQuestionarioAPR(string UKEmpregado, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = UKFonteGeradora;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 4 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    //Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKFonteGeradora + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now;
                            if (oQuest.Periodo == EPeriodo.Dia)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            }
                            else if (oQuest.Periodo == EPeriodo.Mes)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            }
                            else if (oQuest.Periodo == EPeriodo.Ano)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            }

                            if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            {
                                return PartialView("_BuscarAPR");
                            }
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_BuscarAPR", oQuest);
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


        public ActionResult BuscarQuestionarioPorSupervisor(string UKEmpregado, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = UKFonteGeradora;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 3 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKFonteGeradora + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now;
                            if (oQuest.Periodo == EPeriodo.Dia)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            }
                            else if (oQuest.Periodo == EPeriodo.Mes)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            }
                            else if (oQuest.Periodo == EPeriodo.Ano)
                            {
                                DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            }

                            if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            {
                                return PartialView("_BuscarQuestionario");
                            }
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_BuscarQuestionario", oQuest);
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
        public ActionResult BuscarNomeForAutoComplete(string key)
        {
            try
            {
                List<string> riscosAsString = new List<string>();
                List<Empregado> lista = EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Empregado com in lista)
                    riscosAsString.Add(com.Nome);

                return Json(new { Result = riscosAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarnomeForAutoComplete(string key)
        {
            try
            {
                Empregado item = EmpregadoBusiness.Consulta.FirstOrDefault(a => a.Nome.ToUpper().Equals(key.ToUpper()));

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