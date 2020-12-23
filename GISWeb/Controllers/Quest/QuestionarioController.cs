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
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Questionario;
using System.Globalization;

namespace GISWeb.Controllers.Quest
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class QuestionarioController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Questionario> QuestionarioBusiness { get; set; }

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
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarQuestionarios()
        {

            List<Empresa> lista = new List<Empresa>();

            string sql = @"select e.UniqueKey as UKEmpresa, e.NomeFantasia, 
	                              q.UniqueKey as UKQuestionario, q.Nome, q.TipoQuestionario, q.Status, q.Tempo, q.Periodo,
	                              p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                              tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                              tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                           from tbQuestionario q
	                               inner join tbEmpresa e on e.UniqueKey = q.UKEmpresa and e.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                           where q.DataExclusao ='9999-12-31 23:59:59.997' 
                           order by e.NomeFantasia, q.Nome, p.Ordem, tri.Ordem ";

            DataTable result = QuestionarioBusiness.GetDataTable(sql);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Empresa oEmpresa = lista.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKEmpresa"].ToString()));
                    if (oEmpresa == null)
                    {
                        oEmpresa = new Empresa()
                        {
                            UniqueKey = Guid.Parse(row["UKEmpresa"].ToString()),
                            NomeFantasia = row["NomeFantasia"].ToString(),
                            Questionarios = new List<Questionario>()
                        };

                        if (!string.IsNullOrEmpty(row["UKQuestionario"].ToString()))
                        {
                            Questionario oQuest = new Questionario()
                            {
                                UniqueKey = Guid.Parse(row["UKQuestionario"].ToString()),
                                Nome = row["Nome"].ToString(),
                                TipoQuestionario = (ETipoQuestionario)Enum.Parse(typeof(ETipoQuestionario), row["TipoQuestionario"].ToString(), true),
                                Status = (Situacao)Enum.Parse(typeof(Situacao), row["Status"].ToString(), true),
                                Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), row["Periodo"].ToString(), true),
                                Tempo = int.Parse(row["Tempo"].ToString()), 
                                Perguntas = new List<Pergunta>()
                            };

                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = new Pergunta()
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
                                            Nome = row["TipoRespostaItem"].ToString(),
                                            Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                        };

                                        oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }

                                    oPergunta._TipoResposta = oTipoResposta;
                                }

                                oQuest.Perguntas.Add(oPergunta);
                            }

                            oEmpresa.Questionarios.Add(oQuest);
                        }

                        lista.Add(oEmpresa);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row["UKQuestionario"].ToString()))
                        {
                            Questionario oQuestionario = oEmpresa.Questionarios.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKQuestionario"].ToString()));
                            if (oQuestionario == null)
                            {
                                oQuestionario = new Questionario()
                                {
                                    UniqueKey = Guid.Parse(row["UKQuestionario"].ToString()),
                                    Nome = row["Nome"].ToString(),
                                    TipoQuestionario = (ETipoQuestionario)Enum.Parse(typeof(ETipoQuestionario), row["TipoQuestionario"].ToString(), true),
                                    Status = (Situacao)Enum.Parse(typeof(Situacao), row["Status"].ToString(), true),
                                    Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), row["Periodo"].ToString(), true),
                                    Tempo = int.Parse(row["Tempo"].ToString()),
                                    Perguntas = new List<Pergunta>()
                                };

                                if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                                {
                                    Pergunta oPergunta = new Pergunta()
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
                                                Nome = row["TipoRespostaItem"].ToString(),
                                                Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuestionario.Perguntas.Add(oPergunta);
                                }

                                oEmpresa.Questionarios.Add(oQuestionario);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                                {
                                    Pergunta oPergunta = oQuestionario.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
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
                                                    Nome = row["TipoRespostaItem"].ToString(),
                                                    Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                                };

                                                oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                            }

                                            oPergunta._TipoResposta = oTipoResposta;
                                        }

                                        oQuestionario.Perguntas.Add(oPergunta);
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString(),
                                                Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                            };

                                            oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return PartialView("_BuscarQuestionarios", lista);
        }

        private List<Pergunta> BuscarPerguntasVinculadas(string UKPergunta, string UKTipoRespostaItem) 
        {
            List<Pergunta> lista = new List<Pergunta>();

            string sql = @"select p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                              tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                              tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                           from REL_PerguntaTipoRespostaItem r
		                           inner join tbPergunta  p on p.UniqueKey = r.UKNovaPergunta and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                           where r.DataExclusao ='9999-12-31 23:59:59.997' and r.UKPerguntaVinculada = '" + UKPergunta + @"' and r.UKTipoRespostaItem = '" + UKTipoRespostaItem + @"'
                           order by p.Ordem, tri.Ordem";

            DataTable result = QuestionarioBusiness.GetDataTable(sql);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Pergunta oPergunta = lista.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
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
                                    Nome = row["TipoRespostaItem"].ToString(),
                                    Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                };

                                oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                            }

                            oPergunta._TipoResposta = oTipoResposta;
                        }

                        lista.Add(oPergunta);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                        {
                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                            {
                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                Nome = row["TipoRespostaItem"].ToString(),
                                Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                            };

                            oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                        }
                    }
                }
            }

            return lista;
        }

        [HttpPost]
        public ActionResult BuscarPerguntasVinculadasView(string UKPergunta, string UKTipoRespostaItem)
        {
            try
            {

                List<Pergunta> lista = new List<Pergunta>();

                string sql = @"select p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                              tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                              tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                           from REL_PerguntaTipoRespostaItem r
		                           inner join tbPergunta  p on p.UniqueKey = r.UKNovaPergunta and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                           left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                           where r.DataExclusao ='9999-12-31 23:59:59.997' and r.UKPerguntaVinculada = '" + UKPergunta + @"' and r.UKTipoRespostaItem = '" + UKTipoRespostaItem + @"'
                           order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        Pergunta oPergunta = lista.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
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
                                        Nome = row["TipoRespostaItem"].ToString(),
                                        Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                    };

                                    oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                }

                                oPergunta._TipoResposta = oTipoResposta;
                            }

                            lista.Add(oPergunta);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                            {
                                TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                {
                                    UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                    Nome = row["TipoRespostaItem"].ToString(),
                                    Perguntas = BuscarPerguntasVinculadas(oPergunta.UniqueKey.ToString(), row["UKTipoRespostaItem"].ToString())
                                };

                                oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                            }
                        }
                    }
                }

                return PartialView("_BuscarPerguntasVinculadasView", lista);
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


        public ActionResult BuscarQuestionarioPorEmpregado(string UKEmpregado, string UKFonteGeradora)
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
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 2 and q.Status = 1
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
                                return PartialView("_BuscarQuestionarioPorEmpregado");
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

                return PartialView("_BuscarQuestionarioPorEmpregado", oQuest);
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


        public ActionResult BuscarQuestionarioPorEmpregadoMD(string UKEmpregado, string UKFonteGeradora)
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
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 2 and q.Status = 1
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
                                return PartialView("_PainelMenor");
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

                return PartialView("_PainelMenor", oQuest);
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
        public ActionResult GravarRespostaQuestionarioAnalise(VMQuestionarioRespondido entidade) { 
            try
            {
                

                if (string.IsNullOrEmpty(entidade.UKEmpregado))
                    throw new Exception("Não foi possível identificar o empregado que está respondendo o questionário.");

                if (string.IsNullOrEmpty(entidade.UKEmpresa))
                    throw new Exception("Não foi possível identificar a empresa do empregado que está respondendo o questionário.");

                if (string.IsNullOrEmpty(entidade.UKQuestionario))
                    throw new Exception("Não foi possível identificar o questionário que o empregado está respondendo.");

                if (string.IsNullOrEmpty(entidade.UKFonteGeradora))
                    throw new Exception("Não foi possível identificar a fonte geradora da análise de risco que está sendo respondida.");

                if (entidade.PerguntasRespondidas == null || entidade.PerguntasRespondidas.Count == 0)
                    throw new Exception("É necessário responder as perguntas para prosseguir na gravação dos dados.");

                foreach (string[] perguntas in entidade.PerguntasRespondidas)
                {
                    if (perguntas.Length < 3)
                    {
                        throw new Exception("É necessário responder todas as perguntas para prosseguir na gravação dos dados.");
                    }
                }

                var situacao = entidade.Status;

                Resposta oResposta = new Resposta()
                {
                    UniqueKey = Guid.NewGuid(),
                    UKEmpregado = Guid.Parse(entidade.UKEmpregado),
                    UKEmpresa = Guid.Parse(entidade.UKEmpresa),
                    UKQuestionario = Guid.Parse(entidade.UKQuestionario),
                    UKObjeto = Guid.Parse(entidade.UKFonteGeradora),
                    Registro = entidade.Registro,
                    Status = situacao != null? situacao:"",
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                };
                RespostaBusiness.Inserir(oResposta);

               

                foreach (string[] pergunta in entidade.PerguntasRespondidas)
                {
                    string PerguntaResultado = pergunta[0];
                    Guid pResutado = Guid.Parse(PerguntaResultado); 
                        
                        string UKPergunta = pergunta[0];
                        string UKTipoRespostaItem = pergunta[1];
                        string strResposta = pergunta[2];

                        if (!string.IsNullOrEmpty(strResposta))
                        {
                            RespostaItem oRespostaItem = new RespostaItem()
                            {
                                UKFonteGeradora = Guid.Parse(entidade.UKFonteGeradora),
                                UKResposta = oResposta.UniqueKey,
                                UKPergunta = Guid.Parse(UKPergunta),
                                Resposta = strResposta,
                                Registro = entidade.Registro,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            };

                            if (!string.IsNullOrEmpty(UKTipoRespostaItem) && !UKTipoRespostaItem.Equals("*"))
                            {
                                oRespostaItem.UKTipoRespostaItem = Guid.Parse(UKTipoRespostaItem);
                            }

                            RespostaItemBusiness.Inserir(oRespostaItem);
                        }

                        

                    

                    
                }

                Extensions.GravaCookie("MensagemSucesso", "Dados do questionário salvos com sucesso.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AnaliseDeRisco", new {  }) } });
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
        public ActionResult Ativar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                Questionario temp = QuestionarioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível ativar o questionário, pois o mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                QuestionarioBusiness.Terminar(temp);

                QuestionarioBusiness.Inserir(new Questionario()
                {
                    UniqueKey = temp.UniqueKey,
                    Nome = temp.Nome,
                    UKEmpresa = temp.UKEmpresa,
                    Periodo = temp.Periodo,
                    Tempo = temp.Tempo,
                    TipoQuestionario = temp.TipoQuestionario,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                    Status = Situacao.Ativo
                });

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Questionário '" + temp.Nome + "' ativado com sucesso." } });
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
        public ActionResult Desativar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                Questionario temp = QuestionarioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível desativar o questionário, pois o mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                QuestionarioBusiness.Terminar(temp);

                QuestionarioBusiness.Inserir(new Questionario()
                {
                    UniqueKey = temp.UniqueKey,
                    Nome = temp.Nome,
                    UKEmpresa = temp.UKEmpresa,
                    Periodo = temp.Periodo,
                    Tempo = temp.Tempo,
                    TipoQuestionario = temp.TipoQuestionario,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                    Status = Situacao.Inativo
                });

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Questionário '" + temp.Nome + "' desativado com sucesso." } });
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



        public ActionResult Novo(string id)
        {

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.NomeFantasia).ToList();

            Questionario obj = new Questionario();

            if (!string.IsNullOrEmpty(id))
            {
                Guid UK = Guid.Parse(id);
                obj.UKEmpresa = UK;
            }
            
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Questionario entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.Nome))
                        throw new Exception("Informe o nome do questionário para prosseguir.");

                    if (entidade.UKEmpresa == Guid.Empty)
                        throw new Exception("Selecione antes uma empresa para prosseguir.");

                    if (entidade.TipoQuestionario == null)
                        throw new Exception("Selecione o tipo de questionário para prosseguir.");

                    if (entidade.TipoQuestionario == ETipoQuestionario.Analise_de_Risco)
                    {
                        if (QuestionarioBusiness.Consulta.Any(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpresa.Equals(entidade.UKEmpresa) && a.TipoQuestionario == entidade.TipoQuestionario))
                        {
                            throw new Exception("A empresa selecionada já possui um questionário para Análise de Risco.");
                        }
                    }
                    

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    entidade.Status = Situacao.Ativo;
                    QuestionarioBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "O questionário '" + entidade.Nome + "' foi cadastrado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Questionario") } });
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
        public ActionResult Terminar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                Questionario temp = QuestionarioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o questionário, pois o mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                QuestionarioBusiness.Terminar(temp);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O questionário '" + temp.Nome + "' foi excluído com sucesso." } });

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
        public ActionResult ListarQuestionariosPorEmpresa(string UKEmpresa) 
        { 
            try
            {
                List<Questionario> quests = QuestionarioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpresa.ToString().Equals(UKEmpresa)).OrderBy(a => a.Nome).ToList();

                return Json(new { data = quests });

                //return PartialView("_ListarQuestionariosPorEmpresa", quests);
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