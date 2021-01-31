using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
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

namespace GISWeb.Controllers.Veiculo
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ChecklistVeicularController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Questionario> QuestionarioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        // GET: ChecklistVeicular
        public ActionResult Index()
        {




            return View();
        }



        public ActionResult BuscarQuestionarioFluidos(string UKFonteGeradora)
        {
            try
            {

                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                && a.CPF.Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

                var UKEmpregado = oEmpregado.UniqueKey;


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
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 10 and q.Status = 1
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

                    //Guid UKEmp = Guid.Parse(UKEmpregado);
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

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_CheclistFluido");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
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



                return PartialView("_CheclistFluido", oQuest);
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





        public ActionResult BuscarQuestionarioFluidoMD(string UKFonteGeradora)
        {
            try
            {

                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                && a.CPF.Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

                var UKEmpregado = oEmpregado.UniqueKey;


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
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 10 and q.Status = 1
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

                    //Guid UKEmp = Guid.Parse(UKEmpregado);
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

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_CheclistFluidoMD");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
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



                return PartialView("_CheclistFluidoMD", oQuest);
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