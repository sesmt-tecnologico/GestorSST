using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Admissao;
using System.Data;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdmissaoController : BaseController
    {

        #region

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }
        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoImagensBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness RiscosDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public IExposicaoBusiness ExposicaoBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }


        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IPerigoPotencialBusiness PerigoPotencialBusiness { get; set; }

        [Inject]
        public IAtividadeFuncaoLiberadaBusiness AtividadeFuncaoLiberadaBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion



        public ActionResult Novo(string id)
        {
            try
            {
                var UKEmpregado = Guid.Parse(id);

                ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Fornecedor).ToList();

                Admissao obj = new Admissao()
                {
                    UKEmpregado = UKEmpregado
                };

                return PartialView("_Novo", obj);
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Admissao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.Status = GISModel.Enums.Situacao.Ativo;
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    AdmissaoBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Admissão realizada com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = entidade.UniqueKey.ToString() }) } });
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
        public ActionResult BuscarAdmissoesAtuais(string UKEmpregado)
        {
            try
            {
                List<Admissao> lista = new List<Admissao>();

                string query = @"select a.UniqueKey, a.DataAdmissao, a.DataDemissao, a.Justificativa, e.NomeFantasia as Empresa, u.Nome as NomeUsuario
                             from tbAdmissao a, tbEmpresa e, tbUsuario u
                             where a.UKEmpregado = '" + UKEmpregado + @"' and a.Status = 1 and a.UsuarioExclusao is null and
	                               a.UKEmpresa = e.UniqueKey and e.UsuarioExclusao is null and
	                               a.UsuarioInclusao = u.Login and u.UsuarioExclusao is null";

                DataTable result = AdmissaoBusiness.GetDataTable(query);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new Admissao()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            DataAdmissao = row["DataAdmissao"].ToString(),
                            DataDemissao = row["DataDemissao"].ToString(),
                            Justificativa = row["Justificativa"].ToString(),
                            Empresa = new Empresa()
                            {
                                NomeFantasia = row["Empresa"].ToString()
                            },
                            UsuarioInclusao = row["NomeUsuario"].ToString(),
                            Alocacoes = new List<Alocacao>()
                        });
                    }
                }

                return PartialView("_BuscarAdmissoesAtuais", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }


        // public ActionResult Empresas()
        // {
        //     ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
        //     return View();
        // }

        // public ActionResult EmpregadosPorEmpresa(string idEmpresa)
        // {
        //     ViewBag.EmpregadoPorEmpresa = AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UKEmpresa.Equals(idEmpresa))).ToList();
        //     return View();
        // }

        // //passando IDEmpregado como parametro para montar o perfil
        // public ActionResult PerfilEmpregado(string Uk)
        // {
        //     var UK  = Guid.Parse(Uk);
        //     ViewBag.Perfil = AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UKEmpregado.Equals(UK))).ToList();
        //     ViewBag.Admissao = AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpregado.Equals(UK) && p.Status == GISModel.Enums.Situacao.Ativo).ToList();
        //     ViewBag.Alocacao = AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.Admissao.UKEmpregado.Equals(UK))).ToList();
        //     ViewBag.idEmpregado = UK;
        //     var perfEmp = from a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                   join e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                   on a.UKEmpregado equals e.UniqueKey into g                         
        //                   from Emp in g.DefaultIfEmpty()
        //                   join j in EmpresaBusiness.Consulta.Where(p=> string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                   on a.UKEmpresa equals j.ID
        //                   where a.UKEmpregado.Equals(UK) 
        //                   select new PerfilEmpregadoViewModel()
        //                   {
        //                       ID_Admissao = j.ID,
        //                       NomeEmpresa = j.NomeFantasia,
        //                       UniqueKey = a.UniqueKey,
        //                       NomeEmpregado = Emp.Nome,
        //                       CPF = Emp.CPF,
        //                       Email = Emp.Email,
        //                       Nascimento = Emp.DataNascimento,
        //                       DataAdmissao = a.DataAdmissao,
        //                       UsuarioInclusao = a.UsuarioInclusao                              
        //                   };


        //     return View();
        // }

        // public ActionResult AlocarAtividadeFuncao()
        // {
        //     return View();
        // }

        // public ActionResult ListaExpoAtivFuncao(string idAlocacao,string idAtividadeFuncaoLiberada,string Nome, string cpf,string idAtividade)
        // {

        //     var TipoRisco = (from EX in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on EX.idTipoDeRisco equals TR.ID
        //                      join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idAtividadesDoEstabelecimento equals ATE.ID
        //                      join ATL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on EX.idAtividadeAlocada equals ATL.ID
        //                      join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idEventoPerigoso equals EP.ID
        //                      join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPossiveisDanos equals PD.ID
        //                      join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPerigoPotencial equals PP.ID
        //                      where EX.idAlocacao.Equals(Guid.Parse(idAlocacao)) && EX.idAtividadeAlocada.Equals(idAtividade)
        //                      select new Exposicao()
        //                      {
        //                          ID = EX.ID,
        //                          TempoEstimado = EX.TempoEstimado,
        //                          EExposicaoCalor = EX.EExposicaoCalor,
        //                          EExposicaoInsalubre = EX.EExposicaoInsalubre,
        //                          EExposicaoSeg = EX.EExposicaoSeg,
        //                          EProbabilidadeSeg = EX.EProbabilidadeSeg,
        //                          ESeveridadeSeg = EX.ESeveridadeSeg,
        //                          idTipoDeRisco = EX.idTipoDeRisco,

        //                          AtividadeAlocada = new AtividadeAlocada()
        //                          {
        //                              idAtividadesDoEstabelecimento = ATL.idAtividadesDoEstabelecimento
        //                          },

        //                          TipoDeRisco = new TipoDeRisco()
        //                          {
        //                              ID = TR.ID,
        //                              EClasseDoRisco = TR.EClasseDoRisco,
        //                              FonteGeradora = TR.FonteGeradora,
        //                              Tragetoria = TR.Tragetoria,
        //                              idPossiveisDanos = TR.idPossiveisDanos,
        //                              idEventoPerigoso = TR.idEventoPerigoso,
        //                              idPerigoPotencial = TR.idPerigoPotencial,

        //                              EventoPerigoso = new EventoPerigoso()
        //                              {
        //                                  Descricao = EP.Descricao
        //                              },
        //                              PossiveisDanos = new PossiveisDanos()
        //                              {
        //                                  DescricaoDanos = PD.DescricaoDanos
        //                              },
        //                              PerigoPotencial = new PerigoPotencial()
        //                              {
        //                                  DescricaoEvento = PP.DescricaoEvento
        //                              }

        //                          }


        //                      }
        //                      ).ToList();

        // ViewBag.Riscos = TipoRisco;



        //     try
        //     {
        //         Exposicao oExposicao = ExposicaoBusiness.Consulta.FirstOrDefault(p => p.idAtividadeAlocada.Equals(idAtividade) && p.idAlocacao.Equals(idAlocacao));


        //         if (oExposicao == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Alerta = "Exposição não encontrada. Solicite ao Administrador que cadastre esta exposição!." } });
        //         }
        //         else
        //         {
        //             return Json(new { data = RenderRazorViewToString("_ListaExpoAtivFuncao", oExposicao) });
        //         }

        //     }
        //     catch (Exception ex)
        //     {

        //         if (ex.GetBaseException() == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //         }
        //         else
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //         }
        //     }




        // }


        // public ActionResult ListaExposicao(string idAlocacao,string idAtividadeAlocada, string Nome, string cpf, string idAtividadeEstabelecimento)
        // {
        //     var Expo = (from EX in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 join ATA in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 on EX.idAtividadeAlocada equals ATA.ID
        //                 join AlOC in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 on ATA.idAlocacao equals AlOC.ID
        //                 join ADM in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 on AlOC.UKAdmissao equals ADM.ID
        //                 join EMP in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 on ADM.UKEmpregado equals EMP.ID
        //                 join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                 on ATA.idAtividadesDoEstabelecimento equals ATE.ID
        //                 where EX.idAlocacao.Equals(idAlocacao) && EX.idAtividadeAlocada.Equals(idAtividadeAlocada)
        //                 select new Exposicao()
        //                 {
        //                     ID = EX.ID,
        //                     TempoEstimado = EX.TempoEstimado,
        //                     EExposicaoCalor = EX.EExposicaoCalor,
        //                     EExposicaoInsalubre = EX.EExposicaoInsalubre,
        //                     EExposicaoSeg = EX.EExposicaoSeg,
        //                     EProbabilidadeSeg = EX.EProbabilidadeSeg,
        //                     ESeveridadeSeg = EX.ESeveridadeSeg,
        //                     AtividadeAlocada = new AtividadeAlocada()
        //                     {
        //                         ID = ATA.ID,
        //                         idAlocacao = ATA.ID,
        //                         idAtividadesDoEstabelecimento = ATA.idAtividadesDoEstabelecimento,

        //                         Alocacao = new Alocacao()
        //                         {
        //                             ID = AlOC.ID,

        //                         },

        //                         AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
        //                         {
        //                             ID = ATE.ID,
        //                             DescricaoDestaAtividade = ATE.DescricaoDestaAtividade,
        //                             IDEstabelecimento = ATE.IDEstabelecimento,
        //                         }
        //                     },


        //                 }

        //                 ).ToList();

        //     ViewBag.Expo = Expo;

        //     ViewBag.Nome = Nome;
        //     ViewBag.cpf = cpf;

        //     List<Exposicao> ListaExpo = (from EXP in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                                  join ATL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                                  on EXP.idAtividadeAlocada equals ATL.ID
        //                                  join ALOC in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                                  on ATL.idAlocacao equals ALOC.ID
        //                                  where EXP.idAlocacao.Equals(idAlocacao) && EXP.idAtividadeAlocada.Equals(idAtividadeAlocada)
        //                                  select new Exposicao()
        //                                  {
        //                                      ID = EXP.ID,
        //                                      TempoEstimado = EXP.TempoEstimado,
        //                                      EExposicaoCalor = EXP.EExposicaoCalor,
        //                                      EExposicaoInsalubre = EXP.EExposicaoInsalubre,
        //                                      EExposicaoSeg = EXP.EExposicaoSeg,
        //                                      EProbabilidadeSeg = EXP.EProbabilidadeSeg,
        //                                      ESeveridadeSeg = EXP.ESeveridadeSeg,
        //                                      AtividadeAlocada = new AtividadeAlocada()
        //                                      {
        //                                          idAlocacao = ATL.idAlocacao,
        //                                          idAtividadesDoEstabelecimento = ATL.idAtividadesDoEstabelecimento,
        //                                          ID = ATL.ID
        //                                      }


        //                                  }).ToList();


        //     ViewBag.ListaAtividade = ListaExpo;



        //     var TipoRisco = (from EX in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on EX.idTipoDeRisco equals TR.ID
        //                      join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idAtividadesDoEstabelecimento equals ATE.ID
        //                      join ATL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on EX.idAtividadeAlocada equals ATL.ID
        //                      join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idEventoPerigoso equals EP.ID
        //                      join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPossiveisDanos equals PD.ID
        //                      join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPerigoPotencial equals PP.ID
        //                      where EX.idAlocacao.Equals(Guid.Parse(idAlocacao)) && EX.idAtividadeAlocada.Equals(idAtividadeAlocada)
        //                      select new Exposicao()
        //                      {
        //                          ID = EX.ID,
        //                          TempoEstimado = EX.TempoEstimado,
        //                          EExposicaoCalor = EX.EExposicaoCalor,
        //                          EExposicaoInsalubre = EX.EExposicaoInsalubre,
        //                          EExposicaoSeg = EX.EExposicaoSeg,
        //                          EProbabilidadeSeg = EX.EProbabilidadeSeg,
        //                          ESeveridadeSeg = EX.ESeveridadeSeg,
        //                          idTipoDeRisco = EX.idTipoDeRisco,

        //                          AtividadeAlocada = new AtividadeAlocada()
        //                          {
        //                              idAtividadesDoEstabelecimento = ATL.idAtividadesDoEstabelecimento
        //                          },

        //                          TipoDeRisco = new TipoDeRisco()
        //                          {
        //                              ID = TR.ID,
        //                              EClasseDoRisco = TR.EClasseDoRisco,
        //                              FonteGeradora = TR.FonteGeradora,
        //                              Tragetoria = TR.Tragetoria,
        //                              idPossiveisDanos = TR.idPossiveisDanos,
        //                              idEventoPerigoso = TR.idEventoPerigoso,
        //                              idPerigoPotencial = TR.idPerigoPotencial,

        //                              EventoPerigoso = new EventoPerigoso()
        //                              {
        //                                  Descricao = EP.Descricao
        //                              },
        //                              PossiveisDanos = new PossiveisDanos()
        //                              {
        //                                  DescricaoDanos = PD.DescricaoDanos
        //                              },
        //                              PerigoPotencial = new PerigoPotencial()
        //                              {
        //                                  DescricaoEvento = PP.DescricaoEvento
        //                              }

        //                          }


        //                      }
        //                      ).ToList();

        //     ViewBag.Riscos = TipoRisco;


        //     #region ConsultaEsquerda

        //     //var TipoRisco = from EX in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).DefaultIfEmpty()
        //     //                join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on EX.idTipoDeRisco equals TR.IDTipoDeRisco into _r
        //     //                from _A in _r.DefaultIfEmpty()

        //     //                join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on _A.idAtividadesDoEstabelecimento equals ATE.IDAtividadesDoEstabelecimento into _t
        //     //                from _T in _r.DefaultIfEmpty()

        //     //                join ATL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on EX.idAtividadeAlocada equals ATL.IDAtividadeAlocada into _z
        //     //                from _Z in _r.DefaultIfEmpty()

        //     //                join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on _A.idEventoPerigoso equals EP.IDEventoPerigoso into _e
        //     //                from _E in _r.DefaultIfEmpty()

        //     //                join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on _A.idPossiveisDanos equals PD.IDPossiveisDanos into _p
        //     //                from _P in _r.DefaultIfEmpty()

        //     //                join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //     //                on _A.idPerigoPotencial equals PP.IDPerigoPotencial into _Q
        //     //                from _q in _r.DefaultIfEmpty()

        //     //                where EX.idAlocacao.Equals(idAlocacao) && EX.idAtividadeAlocada.Equals(idAtividadeAlocada)
        //     //                select new
        //     //                {

        //     //                    IDExposicao = EX.IDExposicao,
        //     //                    TempoEstimado = EX.TempoEstimado,
        //     //                    EExposicaoCalor = EX.EExposicaoCalor,
        //     //                    EExposicaoInsalubre = EX.EExposicaoInsalubre,
        //     //                    EExposicaoSeg = EX.EExposicaoSeg,
        //     //                    EProbabilidadeSeg = EX.EProbabilidadeSeg,
        //     //                    ESeveridadeSeg = EX.ESeveridadeSeg,
        //     //                    idTipoDeRisco = EX.idTipoDeRisco,

        //     //                    idAtividadesDoEstabelecimento = _Z.idAtividadesDoEstabelecimento,

        //     //                    IDTipoDeRisco = _A.IDTipoDeRisco,
        //     //                    EClasseDoRisco = _A.EClasseDoRisco,
        //     //                    FonteGeradora = _A.FonteGeradora,
        //     //                    Tragetoria = _A.Tragetoria,
        //     //                    idPossiveisDanos = _A.idPossiveisDanos,
        //     //                    idEventoPerigoso = _A.idEventoPerigoso,
        //     //                    idPerigoPotencial = _A.idPerigoPotencial,


        //     //                    Descricao = _E.EventoPerigoso.Descricao,

        //     //                    DescricaoDanos = _P.PossiveisDanos.DescricaoDanos,

        //     //                    DescricaoEvento = _q.PerigoPotencial.DescricaoEvento
        //     //                };







        //     //ViewBag.Riscos = TipoRisco;

        //     #endregion

        //     List<Guid> risc = new List<Guid>();

        //     foreach (var iten in TipoRisco)
        //     {
        //         risc.Add(iten.idTipoDeRisco);
        //     }

        //     ViewBag.risc = risc;
        //     ViewBag.totalrisc = risc.Count();






        //     var TodosRiscos = (from TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()

        //                      join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idAtividadesDoEstabelecimento equals ATE.ID
        //                      join AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on ATE.ID equals AL.idAtividadesDoEstabelecimento
        //                      join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idEventoPerigoso equals EP.ID
        //                      join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPossiveisDanos equals PD.ID
        //                      join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                      on TR.idPerigoPotencial equals PP.ID
        //                      where ATE.ID.Equals(idAtividadeEstabelecimento)
        //                      select new TipoDeRisco()
        //                      {
        //                         ID = TR.ID,
        //                         EClasseDoRisco = TR.EClasseDoRisco,
        //                         FonteGeradora = TR.FonteGeradora,
        //                         Tragetoria = TR.Tragetoria,
        //                         idPossiveisDanos = TR.idPossiveisDanos,
        //                         idEventoPerigoso = TR.idEventoPerigoso,
        //                         idPerigoPotencial = TR.idPerigoPotencial,

        //                         EventoPerigoso = new EventoPerigoso()
        //                         {
        //                             Descricao = EP.Descricao
        //                         },
        //                         PossiveisDanos = new PossiveisDanos()
        //                         {
        //                             DescricaoDanos = PD.DescricaoDanos
        //                         },
        //                         PerigoPotencial = new PerigoPotencial()
        //                         {
        //                             DescricaoEvento = PP.DescricaoEvento
        //                         },

        //                         AtividadesDoEstabelecimento = new AtividadesDoEstabelecimento()
        //                         {
        //                             ID = ATE.ID                                        
        //                         }                                
        //                      }
        //                      ).ToList();
        //     ViewBag.TipoRisco = TodosRiscos;

        //     ViewBag.TipoRisc = TodosRiscos.ToString();

        //     ViewBag.Exposi = ListaExpo;


        //     try
        //     {
        //         Exposicao oExposicao = ExposicaoBusiness.Consulta.FirstOrDefault(p => p.idAtividadeAlocada.Equals(idAtividadeAlocada) && p.idAlocacao.Equals(idAlocacao));


        //         if (oExposicao == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Alerta = "Exposição não encontrada. Solicite ao Administrador que cadastre esta exposição!." } });
        //         }
        //         else
        //         {
        //             return Json(new { data = RenderRazorViewToString("_ListaExposicao", oExposicao) });
        //         }

        //     }
        //     catch (Exception ex)
        //     {

        //         if (ex.GetBaseException() == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //         }
        //         else
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //         }
        //     }



        // }

        // //Assim que o empregado for demitido, retorne esta visão
        // public ActionResult EmpregadoDemitido(string id)
        // {

        //     ViewBag.Demitir = AdmissaoBusiness.Consulta.Where((p=>p.ID.Equals(id))).ToList();


        //     return View();
        // }



        // public ActionResult EmpregadoAdmitidoDetalhes(string id)
        // {
        //     ViewBag.empregado = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(id))).ToList();
        //     try
        //     {
        //         Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => p.UKEmpregado.Equals(id));
        //         if (oAdmissao == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Alerta = "Empregado com CPF '" +id+ "' não encontrado." } });
        //         }
        //         else
        //         {
        //             return Json(new { data = RenderRazorViewToString("_Detalhes", oAdmissao) });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex.GetBaseException() == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //         }
        //         else
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //         }
        //     }

        // }


        // public ActionResult Novo(string UK, string cpf)
        //{
        //     var Uk = Guid.Parse(UK);
        //     ViewBag.cpf = cpf;
        //     ViewBag.IDEmpregado = Uk;
        //     ViewBag.Sigla = new SelectList(DepartamentoBusiness.Consulta.ToList(), "ID", "Sigla");
        //     ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p=> string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "NomeFantasia");
        //     ViewBag.Admissao = AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UKEmpregado.Equals(Uk))).ToList();
        //     ViewBag.Empregado = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(Uk))).ToList();
        //     return View();
        // }



        // //O administrador do sistema poderá admitir o empregado em mais de um cnpj. Este é um controle
        // //necessário para melhor organização do sistema
        // public ActionResult AdmitirEmMaisEmpresas(string UK, string cpf)
        // {

        //     var Uk = Guid.Parse(UK);
        //     ViewBag.cpf = cpf;
        //     ViewBag.IDempregado = Uk;
        //     ViewBag.Sigla = new SelectList(DepartamentoBusiness.Consulta.ToList(), "ID", "Sigla");
        //     ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "NomeFantasia");
        //     ViewBag.Admissao = AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UKEmpregado.Equals(Uk))).ToList();
        //     ViewBag.Empregado = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(Uk))).ToList();


        //     return View();

        // }



        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult AdmitirEmMaisEmpresas(Admissao oAdmissao, string EmpID, string id_cpf) {

        //     var UK_empregado = Guid.Parse(EmpID);

        //     oAdmissao.UKEmpregado = UK_empregado;
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {

        //             var tempAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpregado.Equals(UK_empregado));

        //             if(tempAdmissao == null)
        //             {
        //                 throw new Exception("Este empregado não está admitido em nenhuma empresa no momento!");
        //             }

        //             AdmissaoBusiness.Inserir(oAdmissao);

        //             Extensions.GravaCookie("MensagemSucesso", "O empregado foi Admitido com sucesso.", 10);

        //             return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaEmpregadoNaoAdmitido", "Empregado", new { id = EmpID }) } });

        //         }
        //          catch (Exception ex)
        //         {
        //             if(ex.GetBaseException() == null)
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });

        //             }
        //             else
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //             }


        //         }


        //     }
        //     else
        //     {
        //         return Json(new { resultado = TratarRetornoValidacaoToJSON() });
        //     }

        // }




        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Cadastrar(Admissao oAdmissao, string EmpID, string id_cpf)
        // {
        //     var UK_empregado = Guid.Parse(EmpID);
        //     oAdmissao.UKEmpregado = UK_empregado;

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             var tempAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpregado.Equals(UK_empregado));

        //             if (tempAdmissao !=null)
        //             {
        //                 throw new Exception("Este empregado já está admitido em outra empresa, favor consultar o Administrador do sistema e justificar a " +
        //                     "necessidade do cadastro!");

        //             }

        //             AdmissaoBusiness.Inserir(oAdmissao);

        //             Extensions.GravaCookie("MensagemSucesso", "O empregado foi Admitido com sucesso.", 10);                   

        //             return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaEmpregadoNaoAdmitido", "Empregado", new { id = EmpID }) } });
        //         }
        //         catch (Exception ex)
        //         {
        //             if (ex.GetBaseException() == null)
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //             }
        //             else
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //             }
        //         }

        //     }
        //     else
        //     {
        //         return Json(new { resultado = TratarRetornoValidacaoToJSON() });
        //     }
        // }
        // public ActionResult Edicao(string id)
        // {
        //     return View(EstabelecimentoImagensBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Atualizar(EstabelecimentoAmbiente oEstabelecimentoImagens)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             EstabelecimentoImagensBusiness.Alterar(oEstabelecimentoImagens);
        //             Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi atualizada com sucesso.", 10);



        //             //TempData["MensagemSucesso"] = "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi atualizada com sucesso.";

        //             return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EstabelecimentoImagens") } });
        //         }
        //         catch (Exception ex)
        //         {
        //             if (ex.GetBaseException() == null)
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //             }
        //             else
        //             {
        //                 return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //             }
        //         }

        //     }
        //     else
        //     {
        //         return Json(new { resultado = TratarRetornoValidacaoToJSON() });
        //     }
        // }


        // [HttpPost]
        // public ActionResult Terminar(string IDAdmissao)
        // {

        //     try
        //     {
        //         Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDAdmissao));
        //         if (oAdmissao == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta Admissão." } });
        //         }
        //         else
        //         {

        //             oAdmissao.DataExclusao = DateTime.Now;
        //             oAdmissao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
        //             oAdmissao.Status = GISModel.Enums.Situacao.Inativo;
        //             AdmissaoBusiness.Alterar(oAdmissao);

        //             return Json(new { resultado = new RetornoJSON() { Sucesso = "O Empregado '" + oAdmissao.Empregado.Nome + "' foi demitido com sucesso." } });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex.GetBaseException() == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //         }
        //         else
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //         }
        //     }


        // }


        // [HttpPost]
        // public ActionResult TerminarComRedirect(string IDAdmissao)
        // {

        //     try
        //     {
        //         Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDAdmissao));
        //         if (oAdmissao == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta Admissão." } });
        //         }
        //         else
        //         {
        //             oAdmissao.DataExclusao = DateTime.Now;
        //             oAdmissao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
        //             oAdmissao.Status = GISModel.Enums.Situacao.Inativo;
        //             AdmissaoBusiness.Alterar(oAdmissao);

        //             TempData["MensagemSucesso"] = "O Empregado '" + oAdmissao.Empregado.Nome + "' foi demitido com sucesso.";

        //             return Json(new { resultado = new RetornoJSON() { URL = Url.Action("EmpregadoDemitido", "Admissao", new { id = IDAdmissao }) } });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex.GetBaseException() == null)
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //         }
        //         else
        //         {
        //             return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //         }
        //     }


        // }

        // [RestritoAAjax]
        // public ActionResult _Upload()
        // {
        //     try
        //     {
        //         return PartialView("_Upload");
        //     }
        //     catch (Exception ex)
        //     {
        //         Response.StatusCode = 500;
        //         return Content(ex.Message, "text/html");
        //     }
        // }


    }
}
