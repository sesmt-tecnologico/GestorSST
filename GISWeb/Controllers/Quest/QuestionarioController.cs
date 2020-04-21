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
        public IBaseBusiness<Pergunta> PerguntaBusiness { get; set; }

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
	                              q.UniqueKey as UKQuestionario, q.Nome, q.TipoQuestionario, 
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
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o questionário, pois a mesmo não foi localizado na base de dados." } });

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

                return PartialView("_ListarQuestionariosPorEmpresa", quests);
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