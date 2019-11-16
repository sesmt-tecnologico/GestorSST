using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TipoDeRiscoController : BaseController
    {
        
        #region Inject

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IPerigoPotencialBusiness PerigoPotencialBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(d=>string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo(string id, string Nome, string Ativida)
        {
            ViewBag.EventoPerigoso = new SelectList(EventoPerigosoBusiness.Consulta.ToList(), "IDEventoPerigoso", "Descricao");
            ViewBag.PossiveisDanos = new SelectList(PossiveisDanosBusiness.Consulta.ToList(), "IDPossiveisDanos", "DescricaoDanos");
            ViewBag.EventPeriPotencial = new SelectList(PerigoPotencialBusiness.Consulta.ToList(), "IDPerigoPotencial", "DescricaoEvento");
            ViewBag.AtivEstabelecimento = new SelectList(AtividadesDoEstabelecimentoBusiness.Consulta.ToList(), "IDAtividadesDoEstabelecimento", "DescricaoDestaAtividade");
            ViewBag.idAtividadeEstabel = id;
            ViewBag.Nome = Nome;
            ViewBag.Ativiade = Ativida;

            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividadesDoEstabelecimento equals ATE.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        where ATE.ID.Equals(id)
                                        select new TipoDeRisco()
                                        {
                                            ID = Tip.ID,
                                            EClasseDoRisco = Tip.EClasseDoRisco,
                                            FonteGeradora = Tip.FonteGeradora,
                                            Tragetoria = Tip.Tragetoria,
                                            PossiveisDanos = new PossiveisDanos()
                                            {
                                                DescricaoDanos = PD.DescricaoDanos,

                                            },
                                            PerigoPotencial = new PerigoPotencial()
                                            {
                                                DescricaoEvento = PP.DescricaoEvento,
                                            },
                                            EventoPerigoso = new EventoPerigoso()
                                            {
                                                Descricao = EP.Descricao,
                                            },
                                            AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
                                            {
                                                ID = ATE.ID
                                            }



                                        }


                                        ).ToList();

            ViewBag.DescricaoRiscos = Riscos;


            return View();
        }

        public ActionResult NovoRisco(string idAtividade, string Descricao, string AtivId, string NomeFuncao, string Diretoria, string NomeDiretoria)
        {
            ViewBag.EventoPerigoso = new SelectList(EventoPerigosoBusiness.Consulta.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEventoPerigoso", "Descricao");
            ViewBag.PossiveisDanos = new SelectList(PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDPossiveisDanos", "DescricaoDanos");
            ViewBag.EventPeriPotencial = new SelectList(PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDPerigoPotencial", "DescricaoEvento");
            ViewBag.AtivEstabelecimento = new SelectList(AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDAtividadesDoEstabelecimento", "DescricaoDestaAtividade");
            ViewBag.Descricao = Descricao;
            ViewBag.Nome = NomeFuncao;
            ViewBag.Ativiade = idAtividade;
            ViewBag.AtivId = AtivId;
            ViewBag.Diretoria = Diretoria;
            ViewBag.NomeDiretoria = NomeDiretoria;
            TempData["Funcao"] = NomeFuncao;




            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join AT in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividade equals AT.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        where AT.ID.Equals(idAtividade)
                                        select new TipoDeRisco()
                                        {
                                            ID = Tip.ID,
                                            EClasseDoRisco = Tip.EClasseDoRisco,
                                            FonteGeradora = Tip.FonteGeradora,
                                            Tragetoria = Tip.Tragetoria,
                                            PossiveisDanos = new PossiveisDanos()
                                            {
                                                DescricaoDanos = PD.DescricaoDanos,

                                            },
                                            PerigoPotencial = new PerigoPotencial()
                                            {
                                                DescricaoEvento = PP.DescricaoEvento,
                                            },
                                            EventoPerigoso = new EventoPerigoso()
                                            {
                                                Descricao = EP.Descricao,
                                            },
                                            Atividade = new Atividade()
                                            {
                                                
                                                ID = AT.ID,
                                                Descricao =AT.Descricao
                                            }



                                        }


                                        ).ToList();

            ViewBag.DescricaoRiscos = Riscos;


            var AtividadeList =  AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(idAtividade)));

            ViewBag.ListaAtividade = AtividadeList;


            try
            {
                TipoDeRisco oTipoRiscos = new TipoDeRisco();
                if (oTipoRiscos == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Tipos não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_NovosRiscos", oTipoRiscos) });
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
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarNovoRisco(TipoDeRisco oTipoDeRisco, string idAtividade, string Nome, string AtivId, string NomeFuncao, string Diretoria, string NomeDiretoria)
        {

            
            

            if (ModelState.IsValid)
            {

                try
                {
                    AtividadesDoEstabelecimento oAtividadesDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(idAtividade));

                    oTipoDeRisco.idAtividade = Guid.Parse(idAtividade);
                    TipoDeRiscoBusiness.Inserir(oTipoDeRisco);


                    Extensions.GravaCookie("MensagemSucesso", "O Risco foi cadastrado com sucesso!", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "Atividade", new { id= AtivId, nome= NomeFuncao, idDiretoria= Diretoria, nomeDiretoria= NomeDiretoria }) } });

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
        public ActionResult Cadastrar(TipoDeRisco oTipoDeRisco, string idAtividadeEstabel)
        {


            
            if (ModelState.IsValid)
            {

                try
                {
                    AtividadesDoEstabelecimento oAtividadesDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(idAtividadeEstabel));

                    oTipoDeRisco.idAtividadesDoEstabelecimento = Guid.Parse(idAtividadeEstabel);
                    TipoDeRiscoBusiness.Inserir(oTipoDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "O Risco foi cadastrado com sucesso!", 10);
                   

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "TipoDeRisco" , new { id = idAtividadeEstabel, Nome = oAtividadesDoEstabelecimento.Estabelecimento.NomeCompleto, Ativida = oAtividadesDoEstabelecimento.DescricaoDestaAtividade })  } });

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
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            return View(TipoDeRiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(TipoDeRisco oTipoDeRisco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TipoDeRiscoBusiness.Alterar(oTipoDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "O Tipo de Risco foi atualizado com sucesso.", 10);

                                       
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "TipoDeRisco") } });
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
            ViewBag.Riscos = new SelectList(TipoDeRiscoBusiness.Consulta.ToList(), "IDTipoDeRisco", "DescricaoDoRisco");
            return View(TipoDeRiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Terminar(string IDTipodeRisco)
        {

            try
            {
                TipoDeRisco oTipoDeRisco = TipoDeRiscoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDTipodeRisco));
                if (oTipoDeRisco == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Risco, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oTipoDeRisco.DataExclusao = DateTime.Now;
                    oTipoDeRisco.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    TipoDeRiscoBusiness.Alterar(oTipoDeRisco);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O risco foi excluído com sucesso." } });
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
