using GISCore.Business.Abstract;
using GISModel.DTO.PCMSO;
using GISModel.Entidades;
using GISModel.Entidades.PCMSO;
using GISModel.Entidades.PPRA;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.PCMSO
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ASOController : Controller
    {
        #region
        [Inject]
        public IBaseBusiness<Medicoes> MedicoesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Exames> ExamesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Perigo> PerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_RiscosExames> REL_RiscosExamesBusiness { get; set; }


        [Inject]
        public IAdmissaoBusiness IAdmissaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Risco> RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_PerigoRisco> REL_PerigoRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_FontePerigo> REL_FontePerigoBusiness { get; set; }       

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

        // GET: ASO



        public ActionResult ListaASOEmpregado(string ukEmpregado)
        {
            Guid emp = Guid.Parse(ukEmpregado);

            List<VMLaso> ListaASO = (from al in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     join ad in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpregado.Equals(emp)).ToList()
                                     on al.UKAdmissao equals ad.UniqueKey
                                     join e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(emp)).ToList()
                                     on ad.UKEmpregado equals e.UniqueKey
                                     join f in FuncaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on al.UKFuncao equals f.UniqueKey
                                     join est in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on al.UKEstabelecimento equals est.UniqueKey
                                     join wa in WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on al.UKEstabelecimento equals wa.UKEstabelecimento
                                     join fon in FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on wa.UniqueKey equals fon.UKWorkArea
                                     join fp in REL_FontePerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on fon.UniqueKey equals fp.UKFonteGeradora
                                     //join re in REL_RiscosExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     //on fp.UKPerigo equals re.ukPerigo
                                     join pr in REL_PerigoRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on fp.UKPerigo equals pr.UKPerigo
                                     join r in RiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on pr.UKRisco equals r.UniqueKey
                                     join p in PerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on fp.UKPerigo equals p.UniqueKey
                                     
                                     //where e.UniqueKey.Equals(emp) && ad.UKEmpregado.Equals(emp)
                                     select new VMLaso()
                                     {
                                        ukPerigo = fp.UKPerigo,
                                        Estabelecimento = est.NomeCompleto,
                                        NomeEmpregado = e.Nome,
                                        CPF = e.CPF,
                                        Funcao = f.NomeDaFuncao,
                                        Perigo = p.Descricao,


                                     }).ToList();

            List<VMLaso> asolist = new List<VMLaso>();
            VMLaso aso = null;
            foreach(var item in ListaASO)
            {
                aso = new VMLaso()
                {
                    ukPerigo = item.ukPerigo
                };
                asolist.Add(aso);
            }

            

         

            List<VMLExamesRiscos> ListaE = new List<VMLExamesRiscos>();
            VMLExamesRiscos obj = null;
           
            if(obj == null)
            {
                foreach(var item3 in asolist)
                {
                    

                    List<VMLExamesRiscos> ListaExame = (from r in REL_RiscosExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ukPerigo.Equals(item3.ukPerigo)).ToList()
                                                          join e in ExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                          on r.ukExame equals e.UniqueKey
                                                        join p in PerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                        on r.ukPerigo equals p.UniqueKey
                                                        where r.ukPerigo.Equals(item3.ukPerigo)
                                                        select new VMLExamesRiscos()
                                                          {
                                                              TipoExame = r.TipoExame,
                                                              Exame = e.Nome, 
                                                              Obrigatoriedade = r.Obrigariedade
                                                          }).ToList();

                    foreach (var item2 in ListaExame)
                    {

                        if (item2 != null)
                        {
                            obj = new VMLExamesRiscos()
                            {
                                TipoExame = item2.TipoExame,
                                Exame =item2.Exame,
                                Obrigatoriedade = item2.Obrigatoriedade

                            };
                        }


                        ListaE.Add(obj);
                    }

                        ViewBag.ListaExame = ListaE.ToList().OrderBy(p => p.TipoExame);
                }


            }


           

            ViewBag.listaASO = ListaASO.ToList();




            return View();
        }
    }
}