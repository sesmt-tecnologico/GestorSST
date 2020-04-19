using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
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

            ViewBag.HiddenPerguntaVinculada = true;

            return View(obj);
        }

        public ActionResult NovaPerguntaVinculada(string UKQuestionario, string UKPergunta)
        {
            Guid UKQuest = Guid.Parse(UKQuestionario);
            Guid UKPerg = Guid.Parse(UKPergunta);

            Pergunta obj = new Pergunta()
            {
                UKQuestionario = UKQuest
                //, UKPerguntaVinculada = UKPerg
            };

            Questionario quest = QuestionarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKQuestionario));
            List<Questionario> questionarios = new List<Questionario>();
            questionarios.Add(quest);
            ViewBag.Questionarios = questionarios;

            ViewBag.TiposDeRespostas = TipoRespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.Nome).ToList();

            ViewBag.HiddenPerguntaVinculada = false;

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
        public ActionResult Terminar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                Pergunta temp = PerguntaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a pergunta, pois a mesma não foi localizada na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                PerguntaBusiness.Terminar(temp);

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