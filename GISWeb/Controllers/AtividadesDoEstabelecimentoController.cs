using GISCore.Business.Abstract;
using GISModel.DTO.AtividadesAlocada;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AtividadesDoEstabelecimentoController : BaseController
    {

        #region
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
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IExposicaoBusiness ExposicaoBusiness { get; set; }


        #endregion



        // GET: RiscosDoEstabelecimento
        public ActionResult Index(string id, string nome)
        {
            ViewBag.nome = nome;
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(id))).ToList();
            return View();
        }

        public ActionResult Lista(string id, string nome)
        {
            ViewBag.nome = nome;
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(id))).ToList();

           


            return View();
        }



        //Corrigir parametro..inserir o id correto

        public ActionResult BuscarDetalhesDosRiscos(string idEstabelecimento)
        {
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(idEstabelecimento))).ToList();
            try
            {
                AtividadesDoEstabelecimento oAtividadesDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(idEstabelecimento));
                if (oAtividadesDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens4 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oAtividadesDoEstabelecimento) });
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

        public ActionResult BuscarDetalhesEstabelecimentoImagens(string idAtividadesDoEstabelecimento)
        {

           
            var ExisteMedidaControle = from PA in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  on PA.IDTipoDeRisco equals AE.ID
                                  where PA.IDTipoDeRisco.Equals(idAtividadesDoEstabelecimento)
                                  select new MedidasDeControleExistentes
                                  {
                                      IDTipoDeRisco= PA.IDTipoDeRisco,
                                       
                                  };

            List<MedidasDeControleExistentes> MedidasDeControleExistentes = ExisteMedidaControle.ToList();

            var total = MedidasDeControleExistentes.Count();
            ViewBag.total = total;


            var ExistePlanoAcao = from PA in PlanoDeAcaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  on PA.Identificador equals AE.ID
                                  where PA.Identificador.Equals(idAtividadesDoEstabelecimento)
                                  select new PlanoDeAcao
                                  {
                                      Identificador = PA.Identificador,
                                      ID = PA.ID
                                  };

            List<PlanoDeAcao> TotalPlanoDeAcao = ExistePlanoAcao.ToList();

            var TotalPA = TotalPlanoDeAcao.Count();

            ViewBag.TotalPlanoAcao = TotalPA;

            ViewBag.ExistePlanoAcao = ExistePlanoAcao;

            ViewBag.Imagens = TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.idAtividadesDoEstabelecimento.Equals(idAtividadesDoEstabelecimento))).ToList();


            try
            {
                
                TipoDeRisco oIDRiscosDoEstabelecimento = TipoDeRiscoBusiness.Consulta.FirstOrDefault(p => p.idAtividadesDoEstabelecimento.Equals(idAtividadesDoEstabelecimento));

                //var oIDRiscosDoEstabelecimento = ViewBag.Imagens;

                if (oIDRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens2 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oIDRiscosDoEstabelecimento) });
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



       


        public ActionResult AlocarEmAmbiente(string idEstabelecimento, string idAlocacao)
        {
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(idEstabelecimento))).ToList();
            try
            {


                var ListaAmbientes = from Ambiente in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)&&(p.IDEstabelecimento.Equals(idEstabelecimento))).ToList()
                                     join Aloca in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idAlocacao.Equals(idAlocacao)).ToList()
                                     
                                     on Ambiente.ID equals Aloca.idAtividadesDoEstabelecimento                                     
                                     into productGrupo
                                     from item in productGrupo.DefaultIfEmpty()
                                     select new AtividadesAlocadasViewModel
                                     {
                                         DescricaoAtividade = Ambiente.DescricaoDestaAtividade,
                                         //FonteGeradora = Ambiente.FonteGeradora,
                                         NomeDaImagem = Ambiente.NomeDaImagem,
                                         Imagem = Ambiente.Imagem,
                                         AlocaAtividade = (item == null ? false : true),
                                         IDAtividadeEstabelecimento = Ambiente.ID,
                                         IDAlocacao = idAlocacao

                                     };


                List<AtividadesAlocadasViewModel> lAtividades = ListaAmbientes.ToList();



                AtividadesDoEstabelecimento oIDRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimento.Equals(idEstabelecimento));
                if (oIDRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Ambiente não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_AmbientesAlocado", lAtividades), Contar = lAtividades.Count() });
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

        public ActionResult Ambiente(string idEstabelecimento)
        {


            try
            {
                AtividadesDoEstabelecimento oAtividadesDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimento.Equals(idEstabelecimento));


                ViewBag.AtividadeLista = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(idEstabelecimento))).ToList();

                if (oAtividadesDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Ambiente não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_DetalhesAtividade", oAtividadesDoEstabelecimento) });
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




        public ActionResult EstabelecimentoAmbienteAlocado(string idEstabelecimento, string idAlocacao, string idAtividadeAlocada, string idAtividadesDoEstabelecimento, string idEmpregado)
        {

            #region Riscos
            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividadesDoEstabelecimento equals ATE.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        join AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on ATE.ID equals AL.idAtividadesDoEstabelecimento
                                        where ATE.ID.Equals(idAtividadesDoEstabelecimento) && AL.idAlocacao.Equals(idAlocacao)
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
                                                ID = ATE.ID,
                                                EventoPerigoso = new EventoPerigoso()
                                                {
                                                    Descricao = EP.Descricao
                                                },
                                                PossiveisDanos = new PossiveisDanos()
                                                {
                                                    DescricaoDanos = PD.DescricaoDanos,
                                                }
                                            }
                                        }


                                       ).ToList();

            ViewBag.DescricaoRiscos = Riscos;

            ViewBag.Aloc = idAlocacao;
            ViewBag.AtivAloc = idAtividadeAlocada;
            ViewBag.IdEmpregado = idEmpregado;




            //Quero usar esta função abaixo para avisar na view se existe ou não Plano de Ação para o risco
            #region
            //var Plan = (from PA in PlanoDeAcaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
            //            where PA.Identificador.Equals(idTipoDeRisco)
            //            select new PlanoDeAcao()
            //            {
            //                IDPlanoDeAcao = PA.IDPlanoDeAcao

            //            }

            //            ).ToList();

            //var ContarPlan = Plan.Count();

            //ViewBag.ContPlan = ContarPlan;

            #endregion


            List<AtividadeAlocada> AtividadeAloc = (from ATAL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                    join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                    on ATAL.idAtividadesDoEstabelecimento equals ATE.ID
                                                    where ATAL.idAlocacao.Equals(idAlocacao)
                                                    select new AtividadeAlocada()
                                                    {
                                                        ID = ATAL.ID,
                                                        idAlocacao = ATAL.idAlocacao,
                                                        
                                                        AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
                                                        {
                                                            ID = ATE.ID
                                                        }
                                                    }).ToList();

            ViewBag.IDAloc = AtividadeAloc;

            


            #endregion



            ViewBag.IDAtividadeEstab = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(idEstabelecimento))).ToList();
            


            //ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(idEstabelecimento))).ToList();
            try
            {

                AtividadesDoEstabelecimento oIDRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimento.Equals(idEstabelecimento));

                #region ListaAmbientes
                

                List<AtividadeAlocada> ListaAmbientes = (from AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                    join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                    on AL.idAtividadesDoEstabelecimento equals ATE.ID
                                                         join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         on ATE.ID equals TR.idAtividadesDoEstabelecimento
                                                         join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         on TR.idEventoPerigoso equals EP.ID
                                                         join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         on TR.idPerigoPotencial equals PP.ID
                                                         join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         on TR.idPossiveisDanos equals PD.ID
                                                         where AL.ID.Equals(idAtividadeAlocada) 
                                                    select new AtividadeAlocada()
                                                    {
                                                        
                                                        ID = AL.ID,
                                                        idAtividadesDoEstabelecimento = AL.idAtividadesDoEstabelecimento,
                                                        idAlocacao = AL.idAlocacao,
                                                        
                                                        AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
                                                        {
                                                            ID = ATE.ID,
                                                            DescricaoDestaAtividade = ATE.DescricaoDestaAtividade,
                                                            Imagem = ATE.Imagem,
                                                            NomeDaImagem = ATE.NomeDaImagem,


                                                            PossiveisDanos = new PossiveisDanos()
                                                            {
                                                                ID = PD.ID,
                                                                DescricaoDanos = PD.DescricaoDanos,
                                                            },
                                                            EventoPerigoso = new EventoPerigoso()
                                                            {
                                                                ID = EP.ID,
                                                                Descricao = EP.Descricao,
                                                            }
                                                        }
                                                    }
                                                   ).ToList();



                ViewBag.Imagens = ListaAmbientes.ToList();

                var lAtividades = ViewBag.Imagens;
                #endregion

                #region ListaAtividade
                List<AtividadeAlocada> ListaAtividade = (from AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         join ATV in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on AL.idAtividadesDoEstabelecimento equals ATV.ID
                                                         where AL.idAtividadesDoEstabelecimento.Equals(idAtividadesDoEstabelecimento) && AL.idAlocacao.Equals(idAlocacao)
                                                         select new AtividadeAlocada()
                                                         {
                                                             ID = AL.ID,
                                                             idAtividadesDoEstabelecimento = AL.idAtividadesDoEstabelecimento,
                                                             AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
                                                             {
                                                                 ID = ATV.ID,
                                                                 Imagem = ATV.Imagem,
                                                                 NomeDaImagem = ATV.NomeDaImagem,
                                                                 DescricaoDestaAtividade = ATV.DescricaoDestaAtividade
                                                             },

                                                         }).ToList();

                ViewBag.ListaAtividades = ListaAtividade;



                #endregion


                #region MedidaDeControleExistente


                var MedidaDeControleExistente = from MC in MedidasDeControleBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                                join AE in TipoDeRiscoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
                                                on MC.IDTipoDeRisco equals AE.ID
                                                join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                on AE.idAtividadesDoEstabelecimento equals ATE.ID
                                                where AE.idAtividadesDoEstabelecimento.Equals(idAtividadesDoEstabelecimento)
                                                select new MedidasDeControleExistentes()
                                                {
                                                    IDTipoDeRisco = MC.IDTipoDeRisco,
                                                    ID = MC.ID,
                                                    NomeDaImagem = MC.NomeDaImagem,
                                                    Imagem=MC.Imagem,
                                                    EClassificacaoDaMedida = MC.EClassificacaoDaMedida,
                                                    MedidasExistentes = MC.MedidasExistentes,
                                                    EControle = MC.EControle,
                                                    

                                                    
                                                    TipoDeRisco = new TipoDeRisco()
                                                    {
                                                        ID = AE.ID,
                                                        idAtividadesDoEstabelecimento = AE.idAtividadesDoEstabelecimento,

                                                   },
                                                    


                                                };
                List<MedidasDeControleExistentes> MedContEx = MedidaDeControleExistente.ToList();

                #endregion

                var TotalMedidaControle = MedContEx.Count();

                ViewBag.TotalMCE = TotalMedidaControle;

                ViewBag.IDAtivEstab = MedContEx;

                ViewBag.itens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.TipoDeRisco.idAtividadesDoEstabelecimento.Equals(idAtividadesDoEstabelecimento))).ToList();

                if (lAtividades == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Ambiente não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_DetalhesAmbienteAlocado", lAtividades) });
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

        

        //Mudar o parametro aqui
        public ActionResult BuscarDetalhesDeMedidasDeControle(string id)
        {

            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividadesDoEstabelecimento equals ATE.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        where Tip.ID.Equals(id)
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
                                        }).ToList();

            ViewBag.DescricaoRiscos = Riscos;


            var Lista = (from MC in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        on MC.IDTipoDeRisco equals TR.ID
                        where TR.ID.Equals(id)
                        group TR by TR.ID into g
                        select new
                        {
                            IDTipo = g.Key 
                        }).ToList();

            List<string> Filtro = new List<string>();
            
            foreach(var lista in Lista)
            {

                Filtro.Add(lista.IDTipo);
            }

            List<MedidasDeControleExistentes> total = MedidasDeControleBusiness.Consulta.Where(p => Filtro.Contains(p.IDTipoDeRisco)).ToList();

            ViewBag.Total = total.Count();



            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(id))).ToList();
            try
            {
                MedidasDeControleExistentes oMedidasDeControleExistentes = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(id));
               

                if (oMedidasDeControleExistentes == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Medidas de controle não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_ControleAmbienteAlocado", oMedidasDeControleExistentes) });
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



        public ActionResult Novo(string id, string nome, string idEstabelecimento)
        {

            ViewBag.EstabID = id;
            ViewBag.EstabelecimentoID = idEstabelecimento;
            ViewBag.PerigoPotencial = new SelectList(PerigoPotencialBusiness.Consulta.ToList(), "IDPerigoPotencial", "DescricaoEvento");
           ViewBag.Imagens01 = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(id))).ToList();
            ViewBag.EstabelecimentoAmbiente = EstabelecimentoAmbienteBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(id))).ToList();
            //ViewBag.RegistroID = new SelectList(RiscosDoEstabelecimentoBusiness.Consulta, "RegistroID", "Diretoria");
            ViewBag.nome = nome;

            var ExistePlanoAcao = from PA in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join AE in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  on PA.IDTipoDeRisco equals AE.ID
                                 // where AE.AtividadesDoEstabelecimento.IDEstabelecimento.Equals(idEstabelecimento)
                                  select new MedidasDeControleExistentes
                                  {
                                      
                                      IDTipoDeRisco = AE.ID
                                  };

            List<MedidasDeControleExistentes> MedidasDeControleExistentes = ExistePlanoAcao.ToList();

            var total = MedidasDeControleExistentes.Count();
            ViewBag.total = total;

            var Imagens = (from TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           on TR.idAtividadesDoEstabelecimento equals ATE.ID
                           join EST in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           on ATE.IDEstabelecimento equals EST.ID
                           join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           on TR.idPerigoPotencial equals PP.ID
                           join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           on TR.idPossiveisDanos equals PD.ID
                           join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                           on TR.idEventoPerigoso equals EP.ID
                           where ATE.IDEstabelecimentoImagens.Equals(id)
                           select new TipoDeRisco()
                           {
                               ID = TR.ID,
                               PerigoPotencial = new PerigoPotencial()
                               {
                                   ID = PP.ID,
                                   DescricaoEvento = PP.DescricaoEvento,
                               },

                               PossiveisDanos = new PossiveisDanos()
                               {
                                   ID = PD.ID,
                                   DescricaoDanos = PD.DescricaoDanos,

                               },

                               EventoPerigoso = new EventoPerigoso()
                               {
                                   ID = EP.ID,
                                   Descricao = EP.Descricao,
                               },
                               AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
                               {
                                   ID = ATE.ID,
                                   NomeDaImagem = ATE.NomeDaImagem,
                                   DescricaoDestaAtividade = ATE.DescricaoDestaAtividade,
                                   Imagem = ATE.Imagem,
                                   Estabelecimento = new Estabelecimento()
                                   {
                                        ID = EST.ID,
                                        NomeCompleto = EST.NomeCompleto,
                                   }
                               }
                           }).ToList();

            ViewBag.Imagens = Imagens;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(AtividadesDoEstabelecimento oAtividadesDoEstabelecimento, string RegistroID, string EstabID)
        {

            oAtividadesDoEstabelecimento.IDEstabelecimentoImagens = RegistroID;
            oAtividadesDoEstabelecimento.IDEstabelecimento = EstabID;

            if (ModelState.IsValid)
            {
                try
                {

                    AtividadesDoEstabelecimentoBusiness.Inserir(oAtividadesDoEstabelecimento);

                    TempData["MensagemSucesso"] = "A imagem '" + oAtividadesDoEstabelecimento.Imagem + "'foi cadastrada com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "AtividadesDoEstabelecimento", new { id = oAtividadesDoEstabelecimento.IDEstabelecimentoImagens }) } });
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
            return View(AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(id)));
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(AtividadesDoEstabelecimento oRiscosDoEstabelecimento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AtividadesDoEstabelecimentoBusiness.Alterar(oRiscosDoEstabelecimento);

                    TempData["MensagemSucesso"] = "A imagem '" + oRiscosDoEstabelecimento.NomeDaImagem + "' foi atualizada com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "RiscosDoEstabelecimento") } });
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
        public ActionResult Ativar(AtividadesDoEstabelecimento oRiscosDoEstabelecimento)
        {
            if (ModelState.IsValid)
            {


                //var AdmissaoID = oRiscosDoEstabelecimento.Alocacao.IdAdmissao;
                try
                {
                    AtividadesDoEstabelecimentoBusiness.Alterar(oRiscosDoEstabelecimento);

                    TempData["MensagemSucesso"] = "A imagem '" + oRiscosDoEstabelecimento.NomeDaImagem + "' foi atualizada com sucesso.";

                    
                      return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "Alocacao", new {  }) } });
   
                    
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


        //Ativar a Atividade para que somente estas apareçam na pesquisa por empregado
        public ActionResult AtivarAtividades(string IDEstabelecimentoImagens, string IDAdmissao)
        {

            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(IDEstabelecimentoImagens))).ToList();
            try
            {
                AtividadesDoEstabelecimento oIDRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(IDEstabelecimentoImagens));
                if (oIDRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens3 não encontrada." } });
                }
                else
                {
                    //oIDRiscosDoEstabelecimento.IDAlocacao = IDAdmissao;
                    return Json(new { data = RenderRazorViewToString("_AtivarAtividades", oIDRiscosDoEstabelecimento) });
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










        //Listar somente Atividades relacionadas ao Ambiente de trabalho
        public ActionResult ListarAtividadesDoAmbiente(string IDEstabelecimentoImagens)
        {
            

            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(IDEstabelecimentoImagens))).ToList();
            try
            {
                AtividadesDoEstabelecimento oIDRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(IDEstabelecimentoImagens));
                if (oIDRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens2 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_DetalhesDoAmbiente", oIDRiscosDoEstabelecimento) });
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
        public ActionResult Terminar(string IDRiscosDoEstabelecimento)
        {

            try
            {
                AtividadesDoEstabelecimento oRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(IDRiscosDoEstabelecimento));
                if (oRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else
                {

                    //oEmpresa.DataExclusao = DateTime.Now;
                    oRiscosDoEstabelecimento.UsuarioExclusao = "LoginTeste";
                    AtividadesDoEstabelecimentoBusiness.Alterar(oRiscosDoEstabelecimento);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "A imagem '" + oRiscosDoEstabelecimento.NomeDaImagem + "' foi excluída com sucesso." } });
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
        public ActionResult TerminarComRedirect(string IDRiscosDoEstabelecimento)
        {

            try
            {
                AtividadesDoEstabelecimento oRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(IDRiscosDoEstabelecimento));
                if (oRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a imagem  '" + oRiscosDoEstabelecimento.NomeDaImagem + "', pois a mesma não foi localizada." } });
                }
                else
                {
                    //oEmpresa.DataExclusao = DateTime.Now;
                    oRiscosDoEstabelecimento.UsuarioExclusao = "LoginTeste";

                    AtividadesDoEstabelecimentoBusiness.Alterar(oRiscosDoEstabelecimento);

                    TempData["MensagemSucesso"] = "A imagem '" + oRiscosDoEstabelecimento.NomeDaImagem + "' foi excluída com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "RiscosDoEstabelecimento") } });
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




        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult Upload()
        {
            try
            {
                string fName = string.Empty;
                string msgErro = string.Empty;
                foreach (string fileName in Request.Files.AllKeys)
                {
                    HttpPostedFileBase oFile = Request.Files[fileName];
                    fName = oFile.FileName;
                    if (oFile != null)
                    {
                        string sExtensao = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
                        if (sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("GIF"))
                        {
                            //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
                            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                            sLocalFile = Path.Combine(sLocalFile, "RiscosDoEstabelecimento");
                            sLocalFile = Path.Combine(sLocalFile, "LoginTeste");

                            if (!System.IO.Directory.Exists(sLocalFile))
                                Directory.CreateDirectory(sLocalFile);
                            else
                            {
                                //Tratamento de limpar arquivos da pasta, pois o usuário pode estar apenas alterando o arquivo.
                                //Limpar para não ficar lixo.
                                //O arquivo que for salvo abaixo será limpado após o cadastro.
                                //Se o usuário cancelar o cadastro, a rotina de limpar diretórios ficará responsável por limpá-lo.
                                foreach (string iFile in System.IO.Directory.GetFiles(sLocalFile))
                                {
                                    System.IO.File.Delete(iFile);
                                }
                            }

                            sLocalFile = Path.Combine(sLocalFile, oFile.FileName);

                            oFile.SaveAs(sLocalFile);

                        }
                        else
                        {
                            throw new Exception("Extensão do arquivo não permitida.");
                        }

                    }
                }
                if (string.IsNullOrEmpty(msgErro))
                    return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", arquivo = fName, erro = msgErro });
                else
                    return Json(new { erro = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        private string RenderRazorViewToString(string viewName, object model = null)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public RetornoJSON TratarRetornoValidacaoToJSON()
        {

            string msgAlerta = string.Empty;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (System.Web.Mvc.ModelError i in item.Errors)
                    {
                        msgAlerta += i.ErrorMessage;
                    }
                }
            }

            return new RetornoJSON()
            {
                Alerta = msgAlerta,
                Erro = string.Empty,
                Sucesso = string.Empty
            };

        }

    }


}
