using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.Entidades.PPRA;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.DTO.PPRA;

namespace GISWeb.Controllers.PPRA
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MedicoesController : BaseController
    {
        #region
        [Inject]
        public IBaseBusiness<Medicoes> MedicoesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Risco> RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion

        // GET: Medicoes
        public ActionResult Index(string ukwork)
        {
            

            Guid ukwa = Guid.Parse(ukwork);

          var work =  WorkAreaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(ukwa));

            ViewBag.WorkArea = work.Nome;

           List<VMListaMedicoes> pesquisa = (from  w in WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(ukwa)).ToList()
                            join f in FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                            on w.UniqueKey equals f.UKWorkArea
                            join a in MedicoesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                            on f.UKWorkArea equals a.UKworkarea
                            select new VMListaMedicoes()
                            {
                                WorkArea = w.Descricao,
                               FonteGeradora = f.FonteGeradora,
                                TipoMedicao = a.TipoMedicoes,
                                ValorMedicoes = a.ValorMedicao,
                                MaxExposicao = a.MaxExpDiaria,
                                Observacao = a.Observacoes,
                                UsuarioInclusao = a.UsuarioInclusao,
                                DataInclusao = a.DataInclusao,
                                

                            }).ToList();            

            List<VMListaMedicoes> oMedicoes = new List<VMListaMedicoes>();           


            foreach (var item in pesquisa)
            {
                if(item != null)
                {

                    oMedicoes.Add(new VMListaMedicoes()
                    {

                        //TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), item.TipoMedicoes.GetDisplayName()),
                        WorkArea = item.WorkArea,
                        FonteGeradora = item.FonteGeradora,
                        TipoMedicao = item.TipoMedicao,
                        ValorMedicoes = item.ValorMedicoes,
                        MaxExposicao = item.MaxExposicao, 
                        Observacao = item.Observacao, 
                        DataInclusao =item.DataInclusao,
                        UsuarioInclusao = item.UsuarioInclusao

                    });

                }

            }

            ViewBag.Medicoes = oMedicoes.ToList().OrderByDescending(p => p.DataInclusao);


            List<VMLGrupoRiscoHom> Emp = (from al in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          join a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on al.UKAdmissao equals a.UniqueKey
                                          join f in FuncaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on al.UKFuncao equals f.UniqueKey
                                          join e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on a.UKEmpregado equals e.UniqueKey
                                          join est in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on al.UKEstabelecimento equals est.UniqueKey
                                          join wa in WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                          on est.UniqueKey equals wa.UKEstabelecimento
                                          where wa.UniqueKey.Equals(ukwa)
                                          select new VMLGrupoRiscoHom()
                                          {
                                              Empregado = e.Nome,
                                              Funcao = f.NomeDaFuncao,
                                              DataInclusao = al.DataInclusao,
                                              UsuarioInclusao = al.UsuarioInclusao

                                          }

                        ).ToList();


            ViewBag.Empregado = Emp.ToList().OrderBy(p=>p.DataInclusao);

            //List<VMLGrupoRiscoHom> empregadoGRH = new List<VMLGrupoRiscoHom>();


            



            return View();
        }

        public ActionResult Novo(string UkExpo, string UKWork, string ukrisco)
        {
            Guid Exposicao = Guid.Parse(UkExpo);

            Guid work = Guid.Parse(UKWork);

            Guid risk = Guid.Parse(ukrisco);

            var risco = RiscoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(risk));

            ViewBag.Risco = risco.Nome;

            List<VMListaMedicoes> pesquisa = (from w in WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(work)).ToList()
                                              join f in FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                              on w.UniqueKey equals f.UKWorkArea
                                              join a in MedicoesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                              on f.UKWorkArea equals a.UKworkarea
                                              select new VMListaMedicoes()
                                              {
                                                  WorkArea = w.Descricao,
                                                  FonteGeradora = f.FonteGeradora,
                                                  TipoMedicao = a.TipoMedicoes,
                                                  ValorMedicoes = a.ValorMedicao,
                                                  MaxExposicao = a.MaxExpDiaria,
                                                  Observacao = a.Observacoes,
                                                  UsuarioInclusao = a.UsuarioInclusao,
                                                  DataInclusao = a.DataInclusao,


                                              }).ToList();

            List<VMListaMedicoes> oMedicoes = new List<VMListaMedicoes>();


            ViewBag.Medicoes = pesquisa.ToList().OrderByDescending(p => p.DataInclusao);


            var enumData = from ETipoMedicoes e in Enum.GetValues(typeof(ETipoMedicoes))
                           select new
                           {
                               ID = (int)e,
                               Name = e.GetDisplayName()
                           };
            ViewBag.TipoMedicao = new SelectList(enumData, "ID", "Name");




           

            ViewBag.Exposicao = Exposicao;

            ViewBag.ukWorkarea = work;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Medicoes oMedicoes, string ukExposicao, string ukworkarea)
        {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Guid UKExposicao = Guid.Parse(ukExposicao);
                        Guid ukw = Guid.Parse(ukworkarea);


                        oMedicoes.UKExposicao = UKExposicao;
                        oMedicoes.UKworkarea = ukw;
                        oMedicoes.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                        MedicoesBusiness.Inserir(oMedicoes);

                        Extensions.GravaCookie("MensagemSucesso", "Medição registrada com sucesso.", 10);


                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FonteGeradoraDeRisco") } });



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

     }
       


    
}