using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class ReconhecimentoDoRiscoController : Controller
    {
        #region inject

        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<ControleDeRiscos> ControleDeRiscosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_RiscoControle> REL_RiscoControlesBusiness { get; set; }

        #endregion

        // GET: ReconhecimentoDoRisco
        public ActionResult Index()
        {
            return View();
        }

       

        public ActionResult CriarControle(string UKWorkarea, string UKRisco)
        {
           
            ViewBag.UKWorkArea = UKWorkarea;
            ViewBag.UKRisco = UKRisco;

            var UKRisc = Guid.Parse(UKRisco);
            var UKWork = Guid.Parse(UKWorkarea);
            


            var Nome = RiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKRisc))).ToList();

            List<string> Nom = new List<string>();

            foreach(var item in Nome)
            {
                if(item != null)
                {
                    Nom.Add(item.Nome);
                }
            }

            ViewBag.NomeRisco = Nom;

           var WArea = WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKWork))).ToList();


            List<string> WA = new List<string>();
            foreach (var item01 in WArea)
            {
                if (item01 != null)
                {
                    Nom.Add(item01.Nome);
                }
            }

            ViewBag.NomeWorkarea = WA;





            return PartialView("_CadastrarControleDeRisco");

           
        }


        public ActionResult CadastrarControleDeRisco(string UKControle, string UKWorkarea, string UKRisco, string ukNomeWA, string ukNomeRisc)
        {
            try
            {
                Guid UK_Workarea = Guid.Parse(UKWorkarea);
                Guid UK_Risco = Guid.Parse(UKRisco);



                if (string.IsNullOrEmpty(UKControle))
                    throw new Exception("Não foi possível localizar o Controle.");

                if (string.IsNullOrEmpty(UKRisco))
                    throw new Exception("Nenhum localizar o risco.");


                if (UKControle.Contains(","))
                {
                    foreach (string ativ in UKControle.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            ControleDeRiscos pTemp = ControleDeRiscosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_RiscoControlesBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(pTemp.UniqueKey) && a.UKRisco.Equals(UK_Risco)).Count() == 0)
                                {
                                    REL_RiscoControlesBusiness.Inserir(new REL_RiscoControle()
                                    {
                                        UKRisco = pTemp.UniqueKey,
                                       // UKReconhecimentoRisco = 
                                        //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Risco pTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(UKRisco.Trim()));

                    if (pTemp != null)
                    {
                        //if (REL_PerigoRiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(pTemp.UniqueKey) && a.UKPerigo.Equals(UK_Perigo)).Count() == 0)
                        //{
                        //    REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                        //    {
                        //        UKPerigo = UK_Perigo,
                        //        UKRisco = pTemp.UniqueKey,
                        //        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        //    });
                        //}
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Risco vinculado ao perigo com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        [RestritoAAjax]
        public ActionResult BuscarControlesForAutoComplete(string key)
        {
            try
            {
                List<string> riscosAsString = new List<string>();
               List<ControleDeRiscos> lista = ControleDeRiscosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (ControleDeRiscos com in lista)
                    riscosAsString.Add(com.Descricao);

                return Json(new { Result = riscosAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarControleForAutoComplete(string key)
        {
            try
            {
                ControleDeRiscos item = ControleDeRiscosBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }


    }



}
