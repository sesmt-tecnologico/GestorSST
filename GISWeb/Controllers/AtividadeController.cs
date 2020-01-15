using GISCore.Business.Abstract;
using GISModel.DTO.AtividadeAlocadaFuncao;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Provider.Concrete;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AtividadeController : BaseController
    {
        #region Inject

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }


        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public IAtividadeFuncaoLiberadaBusiness AtividadeFuncaoLiberadaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }


        [Inject]
        public IPlanoDeAcaoBusiness PlanoDeAcaoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPerigoPotencialBusiness PerigoPotencialBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoAmbienteBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IExposicaoBusiness ExposicaoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }




        #endregion
        // GET: TipoDeRisco
        public ActionResult Index()
        {
            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d => d.Descricao).ToList();

            return View();
        }


        //recebe parametro de Funcao/index e listaFuncao para listar atividades relacionadas a função
        public ActionResult ListaAtividadePorFuncao(string IDFuncao, string NomeFuncao)
        {

            ViewBag.Funcao = NomeFuncao;

            //ViewBag.ListaAtividadeFuncao = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(IDFuncao)).OrderBy(p=>p.Descricao).ToList();

            try
            {
                // Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(id));

                if (ViewBag.ListaAtividadeFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens4 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes") });
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


        //parametro id da função, nome da função e id da Diretoria, passados de index/função e ListaFunção
        public ActionResult Novo()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Atividade oAtividade)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    AtividadeBusiness.Inserir(oAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oAtividade.Descricao + "' foi cadastrada com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });

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




        public ActionResult BuscarDetalhesDeMedidasDeControleAtividadeFuncao(string idTipoRisco, string idAtividade)
        {

            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join ATE in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividade equals ATE.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        where Tip.ID.Equals(idTipoRisco) && ATE.ID.Equals(idAtividade)
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
                                        }



                                        ).ToList();

            ViewBag.DescricaoRiscos = Riscos;


            var Lista = (from MC in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                         join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                         on MC.IDTipoDeRisco equals TR.ID
                         where TR.ID.Equals(idTipoRisco)
                         group TR by TR.ID into g
                         select new
                         {
                             IDTipo = g.Key
                         }
                        ).ToList();


            List<Guid> Filtro = new List<Guid>();

            foreach (var lista in Lista)
            {

                Filtro.Add(lista.IDTipo);
            }

            List<MedidasDeControleExistentes> total = MedidasDeControleBusiness.Consulta.Where(p => Filtro.Contains(p.IDTipoDeRisco)).ToList();

            ViewBag.Total = total.Count();



            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(idTipoRisco) && (p.TipoDeRisco.idAtividade.Equals(idAtividade)))).ToList();
            try
            {
                MedidasDeControleExistentes oMedidasDeControleExistentes = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(idTipoRisco));


                if (oMedidasDeControleExistentes == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Medidas de controle não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_ControleRiscoFuncao", oMedidasDeControleExistentes) });
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





        public ActionResult Edicao(string id, string Uk)
        {
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            Guid ID_Ativ = Guid.Parse(id);
            Guid UK_Ativ = Guid.Parse(Uk);

            ViewBag.Atividades = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UK_Ativ))).ToList();

            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UK_Ativ))));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Atividade pAtividade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    pAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    AtividadeBusiness.Alterar(pAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + pAtividade.Descricao + "' foi atualizada com sucesso.", 10);

                    //return RedirectToAction("Index");

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });
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
        public ActionResult TerminarComRedirect(string ID, string Descricao)
        {
            var ID_Atividade = Guid.Parse(ID);

            try
            {
                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID_Atividade));
                if (oAtividade == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Documento." } });
                }
                else
                {
                    oAtividade.DataExclusao = DateTime.Now;
                    oAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    AtividadeBusiness.Excluir(oAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "Atividade '"+ oAtividade.Descricao +"' foi removida com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });
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
        public ActionResult BuscarAtividadeForAutoComplete(string key)
        {
            try
            {
                List<string> atividadeAsString = new List<string>();
                List<Atividade> lista = AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Atividade com in lista)
                    atividadeAsString.Add(com.Descricao);

                return Json(new { Result = atividadeAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarAtividadeForAutoComplete(string key)
        {
            try
            {
                Atividade item = AtividadeBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
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
