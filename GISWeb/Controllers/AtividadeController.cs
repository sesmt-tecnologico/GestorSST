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

        //[Inject]
        //public IExposicaoBusiness ExposicaoBusiness { get; set; }




        #endregion
        // GET: TipoDeRisco
        public ActionResult Index()
        {
            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d=>d.idFuncao).ToList();

            return View();
        }



        public ActionResult AlocarAtivFuncao(string IDFuncao, string IDEmpregado,string IDAlocacao)
        {
            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.idFuncao.Equals(IDFuncao))).ToList();
            try
            {


                var ListaAtividades = from A in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.idFuncao.Equals(IDFuncao))).ToList()
                                      join AFL in AtividadeFuncaoLiberadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDAlocacao.Equals(IDAlocacao)).ToList() on A.ID equals AFL.IDAtividade  
                                      into productGrupo
                                      from item in productGrupo.DefaultIfEmpty()
                                      select new AtividadeAlocadaFuncaoViewModel
                                      {
                                          Descricao = A.Descricao,
                                          NomeDaImagem = A.NomeDaImagem,
                                          Imagem = A.Imagem,
                                          AlocaAtividade = (item == null ? false : true),                                         
                                          IDAlocacao = Guid.Parse(IDAlocacao), 
                                          IDAtividade = A.ID,
                                          IDFuncao = A.Funcao.ID
                                      };


                List<AtividadeAlocadaFuncaoViewModel> lAtividades = ListaAtividades.ToList();



                Atividade oIDAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => p.idFuncao.Equals(IDFuncao));
                if (oIDAtividade == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Atividade não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_AlocarAtivFuncao", lAtividades), Contar = lAtividades.Count() });
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








        //recebe parametro de Funcao/index e listaFuncao para listar atividades relacionadas a função
        public ActionResult ListaAtividadePorFuncao(string IDFuncao, string NomeFuncao)
        {

            ViewBag.Funcao = NomeFuncao;

            ViewBag.ListaAtividadeFuncao = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(IDFuncao)).OrderBy(p=>p.Descricao).ToList();

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
        public ActionResult Novo(string id, string nome, string idDiretoria, string nomeDiretoria)
        {

            Guid ID = Guid.Parse(id);

            ViewBag.AtivId = id;

            TempData["Funcao"] = nome;

            ViewBag.NomeFuncao = nome;

            ViewBag.NomeDiretoria = nomeDiretoria;

            ViewBag.Atividade = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.ID.Equals(ID))).ToList();

            ViewBag.Diretoria = idDiretoria;

            ViewBag.AtividadeTotal = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.idFuncao.Equals(id))).Count();

            List<Atividade> Ativ = (from a in FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                    join b in AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                    on a.ID equals b.idFuncao
                                    where a.ID.Equals(id)
                                    select new Atividade()
                                    {
                                        ID = b.ID,
                                        Descricao = b.Descricao,
                                        Funcao = new Funcao()
                                        {
                                            NomeDaFuncao = a.NomeDaFuncao,
                                            IdCargo = a.IdCargo

                                        }
                                    }

                                  ).ToList();
            ViewBag.FuncaoCargo = Ativ;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Atividade oAtividade, string AtivId)
        {
            oAtividade.idFuncao = Guid.Parse(AtivId);
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
                                            PerigoPotencial = new Perigo()
                                            {
                                                Descricao = PP.Descricao,
                                            },
                                            EventoPerigoso = new EventoPerigoso()
                                            {
                                                Descricao = EP.Descricao,
                                            },
                                            Atividade = new Atividade()
                                            {
                                                ID = ATE.ID
                                            }
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








        public ActionResult AtividadesDaFuncao(string idAtividade, string idAlocacao, string idAtivFuncaoLiberada, string idEmpregado)
        {

            //listar a atividade e os riscos relacionados a função
            List<TipoDeRisco> IRiscosDeAtivFuncaoLiberada = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                             join ATE in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                             on Tip.idAtividade equals ATE.ID
                                                             join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                             on Tip.idPossiveisDanos equals PD.ID
                                                             join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                             on Tip.idPerigoPotencial equals PP.ID
                                                             join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                             on Tip.idEventoPerigoso equals EP.ID
                                                             where Tip.idAtividade.Equals(idAtividade) && ATE.ID.Equals(idAtividade)
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
                                                                 PerigoPotencial = new Perigo()
                                                                 {
                                                                     Descricao = PP.Descricao,
                                                                 },
                                                                 EventoPerigoso = new EventoPerigoso()
                                                                 {
                                                                     ID = EP.ID,
                                                                     Descricao = EP.Descricao,
                                                                 },
                                                                 Atividade = new Atividade()
                                                                 {
                                                                     ID = ATE.ID,
                                                                     Imagem = ATE.Imagem,
                                                                     NomeDaImagem = ATE.NomeDaImagem,
                                                                     Descricao = ATE.Descricao

                                                                }
                                                            }
                                                                         
                                                                      ).ToList();



            ViewBag.ListaRiscosDeAtivFuncaoLiberada = IRiscosDeAtivFuncaoLiberada;

            var lAtividades = ViewBag.ListaRiscosDeAtivFuncaoLiberada;



            List<Atividade> IAtividade = (from A in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          join AFL in AtividadeFuncaoLiberadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on A.ID equals AFL.IDAtividade
                                          where AFL.IDAtividade.Equals(idAtividade)
                                          select new Atividade()
                                          {
                                              ID = A.ID,
                                              NomeDaImagem = A.NomeDaImagem,
                                              Imagem = A.Imagem,
                                              Descricao = A.Descricao
                                          }
                                          ).ToList();


            ViewBag.Atividade = IAtividade;


            var LiataAtividade01 = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(idAtividade))).ToList().Take(1);

            ViewBag.listaAtividade01 = LiataAtividade01;

            List<MedidasDeControleExistentes> ITipodeRisco = (from MCO in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                              join TP in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                              on MCO.IDTipoDeRisco equals TP.ID
                                                              join A in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                              on TP.idAtividade equals A.ID
                                                              where MCO.IDTipoDeRisco.Equals(TP.ID) && A.ID.Equals(idAtividade)
                                                              select new MedidasDeControleExistentes()
                                                              {
                                                                  ID = MCO.ID,
                                                                  IDTipoDeRisco = MCO.IDTipoDeRisco,

                                                                  TipoDeRisco = new TipoDeRisco()
                                                                  {
                                                                      idAtividade = TP.idAtividade,
                                                                      ID = TP.ID,
                                                                      Atividade = new Atividade()
                                                                      {
                                                                          ID = A.ID
                                                                      }
                                                                  }
                                                              }
                                             ).ToList();



            var lista01 = TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idAtividade == p.Atividade.ID).ToList().Take(1);

            ViewBag.ListaTipoDerisco = lista01;

            var MedidaDeControleExistente = from MC in MedidasDeControleBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                            join AE in TipoDeRiscoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                            on MC.IDTipoDeRisco equals AE.ID
                                            join ATE in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                            on AE.idAtividade equals ATE.ID
                                            where AE.idAtividade.Equals(idAtividade)
                                            select new MedidasDeControleExistentes()
                                            {
                                                IDTipoDeRisco = MC.IDTipoDeRisco,
                                                ID = MC.ID,
                                                NomeDaImagem = MC.NomeDaImagem,
                                                Imagem = MC.Imagem,
                                                EClassificacaoDaMedida = MC.EClassificacaoDaMedida,
                                                MedidasExistentes = MC.MedidasExistentes,
                                                EControle = MC.EControle,
                                                TipoDeRisco = new TipoDeRisco()
                                                {
                                                    ID = AE.ID,
                                                    idAtividadesDoEstabelecimento = AE.idAtividadesDoEstabelecimento
                                                }
                                            };

            List<MedidasDeControleExistentes> MedContEx = MedidaDeControleExistente.ToList();

            var TotalMedidaControle = MedContEx.Count();

            ViewBag.TotalMCE = TotalMedidaControle;

            try
            {
                

                if (lAtividades == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Ambiente não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_ListaExpoAtivFuncao", lAtividades) });
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






        public ActionResult Edicao(string id)
        {
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Atividade oAtividadeDeRisco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AtividadeBusiness.Alterar(oAtividadeDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oAtividadeDeRisco.Descricao + "' foi atualizada com sucesso.", 10);
                                                          
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AtividadeDeRisco") } });
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
        public ActionResult Excluir(Atividade oAtividadeDeRisco)
        {

            try
            {

                if (oAtividadeDeRisco == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a Atividade, pois a mesma não foi localizada." } });
                }
                else
                {

                    //oDepartamento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    // oDepartamento.UKUsuarioDemissao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    oAtividadeDeRisco.UsuarioExclusao = "Antonio Henriques";
                    oAtividadeDeRisco.DataExclusao = DateTime.Now;
                    AtividadeBusiness.Excluir(oAtividadeDeRisco);

                    TempData["MensagemSucesso"] = "A Atividade '" + oAtividadeDeRisco.Descricao + "' foi excluida com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AtividadeDeRisco") } });
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