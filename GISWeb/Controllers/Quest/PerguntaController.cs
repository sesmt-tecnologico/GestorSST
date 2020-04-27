using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Pergunta;
using GISModel.DTO.Shared;
using GISModel.Entidades.Quest;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.Quest
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PerguntaController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Questionario> QuestionarioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Pergunta> PerguntaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoResposta> TipoRespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoRespostaItem> TipoRespostaItemBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_PerguntaTipoRespostaItem> REL_PerguntaTipoRespostaItemBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }




        public ActionResult Novo(string id)
        {
            Guid UKQuestionario = Guid.Parse(id);

            Pergunta obj = new Pergunta()
            {
                UKQuestionario = UKQuestionario
            };

            Questionario quest = QuestionarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKQuestionario));
            List<Questionario> questionarios = new List<Questionario>();
            questionarios.Add(quest);
            ViewBag.Questionarios = questionarios;

            ViewBag.TiposDeRespostas = TipoRespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.Nome).ToList();

            return View(obj);
        }

        public ActionResult NovaPerguntaVinculada(string UKT, string UKP)
        {
            Guid UKTipoRespostaItem = Guid.Parse(UKT);
            Guid UKPerg = Guid.Parse(UKP);

            TipoRespostaItem oTipoRespItem = TipoRespostaItemBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKTipoRespostaItem));
            Pergunta oPergunta = PerguntaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKPerg));
            
            VMNovaPerguntaVinculada obj = new VMNovaPerguntaVinculada()
            {
                PerguntaVinculada = oPergunta,
                TipoRespostaItemVinculada = oTipoRespItem,
            };

            ViewBag.TiposDeRespostas = TipoRespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.Nome).ToList();

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Pergunta entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.Descricao))
                        throw new Exception("Informe a pergunta antes de prosseguir com a criação.");

                    if (entidade.UKQuestionario == null || entidade.UKQuestionario == Guid.Empty)
                        throw new Exception("Não foi possível localizar o questionário vinculado a pergunta a ser cadastrada.");

                    if (entidade.TipoResposta == GISModel.Enums.ETipoResposta.Multipla_Selecao)
                    {
                        if (entidade.UKTipoResposta == null || entidade.UKTipoResposta == Guid.Empty)
                        {
                            throw new Exception("Não foi possível localizar as possíveis respostas para a pergunta de múltipla escolha a ser cadastrada.");
                        }
                    }

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PerguntaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "A pergunta foi cadastrada com sucesso.", 10);

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
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarVinculada(VMNovaPerguntaVinculada entidade) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (entidade.PerguntaVinculada == null || entidade.PerguntaVinculada.UniqueKey == null || entidade.PerguntaVinculada.UniqueKey == Guid.Empty)
                        throw new Exception("Não foi possível localizar a pergunta vinculada a pergunta a ser cadastrada.");

                    if (entidade.TipoRespostaItemVinculada == null || entidade.TipoRespostaItemVinculada.UniqueKey == null || entidade.TipoRespostaItemVinculada.UniqueKey == Guid.Empty)
                        throw new Exception("Não foi possível localizar a resposta vinculada a pergunta a ser cadastrada.");

                    if (string.IsNullOrEmpty(entidade.Descricao))
                        throw new Exception("Informe a pergunta antes de prosseguir com a criação.");

                    if (entidade.TipoResposta == GISModel.Enums.ETipoResposta.Multipla_Selecao || entidade.TipoResposta == GISModel.Enums.ETipoResposta.Selecao_Unica)
                    {
                        if (entidade.UKTipoResposta == null || entidade.UKTipoResposta == Guid.Empty)
                        {
                            throw new Exception("Não foi possível localizar as possíveis respostas para a pergunta de múltipla escolha a ser cadastrada.");
                        }
                    }


                    Pergunta oPergunta = new Pergunta()
                    {
                        UniqueKey = Guid.NewGuid(),
                        Descricao = entidade.Descricao,
                        TipoResposta = entidade.TipoResposta,
                        UKTipoResposta = entidade.UKTipoResposta,
                        Ordem = entidade.Ordem,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                    };
                    PerguntaBusiness.Inserir(oPergunta);




                    REL_PerguntaTipoRespostaItem oRel = new REL_PerguntaTipoRespostaItem()
                    {
                        UKPerguntaVinculada = entidade.PerguntaVinculada.UniqueKey,
                        UKTipoRespostaItem = entidade.TipoRespostaItemVinculada.UniqueKey,
                        UKNovaPergunta = oPergunta.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                    };

                    REL_PerguntaTipoRespostaItemBusiness.Inserir(oRel);




                    Extensions.GravaCookie("MensagemSucesso", "A pergunta vinculada foi cadastrada com sucesso.", 10);

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
                Guid UKPergunta = Guid.Parse(id);
                Pergunta temp = PerguntaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKPergunta));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a pergunta, pois a mesma não foi localizada na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                PerguntaBusiness.Terminar(temp);

                List<REL_PerguntaTipoRespostaItem> rels = REL_PerguntaTipoRespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKNovaPergunta.Equals(UKPergunta)).ToList();

                if (rels?.Count > 0)
                {
                    foreach (REL_PerguntaTipoRespostaItem rel in rels)
                    {
                        rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        REL_PerguntaTipoRespostaItemBusiness.Terminar(rel);
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "A pergunta foi excluída com sucesso." } });

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