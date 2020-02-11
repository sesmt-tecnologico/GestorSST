using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
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
        public IControleDeRiscoBusiness ControleDeRiscosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_RiscoControle> REL_RiscoControlesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeControle> TipoDeControleBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_WorkAreaPerigo> REL_WorkAreaPerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        // GET: ReconhecimentoDoRisco
        public ActionResult Index()
        {
            return View();
        }

       

        public ActionResult CriarControle( string UKWorkarea, string UKRisco)
        {
           
            ViewBag.UKWorkArea = UKWorkarea;
            ViewBag.UKRisco = UKRisco;

            var UKRisc = Guid.Parse(UKRisco);
            var UKWork = Guid.Parse(UKWorkarea);
           

            var Nome = RiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKRisc))).ToList();         
            
           var WArea = WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKWork))).ToList();

            var enumData = from EClasseDoRisco e in Enum.GetValues(typeof(EClasseDoRisco))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewBag.Eclasse = new SelectList(enumData, "ID", "Name");


            var enumData01 = from ETrajetoria e in Enum.GetValues(typeof(ETrajetoria))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewBag.ETrajetoria = new SelectList(enumData01, "ID", "Name");


            var enumData02 = from EClassificacaoDaMedia e in Enum.GetValues(typeof(EClassificacaoDaMedia))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };
            ViewBag.EClassificacaoDaMedia = new SelectList(enumData02, "ID", "Name");

            var enumData03 = from EControle e in Enum.GetValues(typeof(EControle))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };
            ViewBag.EControle = new SelectList(enumData03, "ID", "Name");




            ViewBag.FonteGeradora = new SelectList(FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "FonteGeradora");

            List<WorkArea> lista = new List<WorkArea>();

            string sql = @"select wa.UniqueKey, wa.Nome, wa.Descricao, 
	                                  r1.Uniquekey as relwap, p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr, r.UniqueKey as ukrisco, r.Nome as risco 
                               from tbWorkArea wa 
	                                left join [dbGestor].[dbo].[REL_WorkAreaPerigo]  r1 on r1.UKWorkArea = wa.UniqueKey  and r1.DataExclusao = '9999-12-31 23:59:59.997' 
	                                left join  [dbGestor].[dbo].[tbPerigo] p on r1.UKPerigo = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997' 
	                                left join [dbGestor].[dbo].[REL_PerigoRisco] r2 on r2.UKPerigo = p.UniqueKey and r2.DataExclusao ='9999-12-31 23:59:59.997' 
	                                left join [dbGestor].[dbo].[tbRisco] r on r2.UKRisco = r.UniqueKey  and r.DataExclusao = '9999-12-31 23:59:59.997' 
                               where wa.DataExclusao ='9999-12-31 23:59:59.997'  and wa.UniqueKey = '" + UKWorkarea + @"'
                               and r.UniqueKey = '" + UKRisco + @"'
                               order by wa.UniqueKey ";

            DataTable result = WorkAreaBusiness.GetDataTable(sql);

            if (result.Rows.Count > 0)
            {
                //WorkArea obj = null;
                //Perigo oPerigo = null;

                //foreach (DataRow row in result.Rows)
                //{
                //    if (obj == null)
                //    {
                //        obj = new WorkArea()
                //        {
                //            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                //            Nome = row["Nome"].ToString(),
                //            Descricao = row["Descricao"].ToString(),
                //            Perigos = new List<Perigo>()
                //        };


                //        if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                //        {
                //            oPerigo = new Perigo()
                //            {
                //                ID = Guid.Parse(row["relwap"].ToString()),
                //                UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                //                Descricao = row["perigo"].ToString(),
                //                Riscos = new List<Risco>()
                //            };

                //            if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                //            {
                //                oPerigo.Riscos.Add(new Risco()
                //                {
                //                    ID = Guid.Parse(row["relpr"].ToString()),
                //                    UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                //                    Nome = row["risco"].ToString()
                //                });
                //            }

                //            obj.Perigos.Add(oPerigo);
                //        }

                //    }
                //}
                //if (obj != null)
                //    lista.Add(obj);
            }




            return PartialView("_CadastrarControleDeRisco", lista);

           
        }


        public ActionResult CadastrarControleDeRisco(ReconhecimentoDoRisco entidade,ControleDeRiscos oControle,string UKControle, string UKWorkarea, string UKRisco)
        {
            try
            {
                Guid UK_Workarea = Guid.Parse(UKWorkarea);
                Guid UK_Risco = Guid.Parse(UKRisco);



                if (string.IsNullOrEmpty(UKControle))
                    throw new Exception("Não foi possível localizar o Controle.");

                if (string.IsNullOrEmpty(UKRisco))
                    throw new Exception("Não foi possivel localizar o risco.");

                if (string.IsNullOrEmpty(UKWorkarea))
                    throw new Exception("Não foi possivel localizar a workArea.");

                ReconhecimentoDoRisco oReconhecimento = ReconhecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKRisco.Equals(UK_Risco) && p.UKWorkarea.Equals(UK_Workarea));

               
                    ReconhecimentoDoRisco pReconhecimento = new ReconhecimentoDoRisco()
                    {
                        UKWorkarea = UK_Workarea,
                        UKRisco = UK_Risco,
                        FonteGeradora = entidade.FonteGeradora,
                        Tragetoria = entidade.Tragetoria,
                        EClasseDoRisco=entidade.EClasseDoRisco,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login

                    };

                    ReconhecimentoBusiness.Inserir(pReconhecimento);

                ReconhecimentoDoRisco pRec = ReconhecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKRisco.Equals(UK_Risco));

                List<Guid> filtro = new List<Guid>();

                if (UKControle.Contains(","))
                {
                   

                    foreach (string ativ in UKControle.Split(','))
                    {
                        

                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            var pesControRisco = from A in ReconhecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 join B in ControleDeRiscosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 on A.UniqueKey equals B.UKReconhecimentoDoRisco                                                 
                                                 select new 
                                                 {
                                                     UniqueReconhecimento = A.UniqueKey,
                                                     UKWorkarea = A.UKWorkarea,
                                                     Risco = A.UKRisco,
                                                     FonteGer = A.FonteGeradora,
                                                     UniqueControle = B.UKReconhecimentoDoRisco,                                                     
                                                     Controle = B.Controle

                                                 };
                                                                                  

                            if (pesControRisco != null)
                            {
                                foreach(var item in pesControRisco)
                                {
                                    if(item.Controle.Equals(ativ.Trim()) && item.FonteGer.Equals(pRec.FonteGeradora))
                                    {
                                        filtro.Add(item.UniqueReconhecimento);
                                    }
                                   
                                }

                            }


                            //ControleDeRiscos pTemp = ControleDeRiscosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.EControle.Equals(ativ.Trim()) && a.UKFonteGeradora.Equals(pRec.FonteGeradora));
                           
                            if (filtro.Count == 0)
                            {
                               
                                    ControleDeRiscosBusiness.Inserir(new ControleDeRiscos()
                                    {
                                        UKReconhecimentoDoRisco = pReconhecimento.UniqueKey,                                        
                                        EClassificacaoDaMedia = oControle.EClassificacaoDaMedia,
                                        Controle = ativ.Trim(),
                                        EControle = oControle.EControle,
                                        Descricao = oControle.Descricao,                                        
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });

                            }
                            else
                            {
                                ReconhecimentoDoRisco qReconhecimento = ReconhecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKRisco.Equals(UK_Risco) && p.UKWorkarea.Equals(UK_Workarea));


                                qReconhecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                ReconhecimentoBusiness.Terminar(qReconhecimento);

                                throw new Exception("Este controle já existe para este Risco e para esta fonte Geradora.");

                               // return Json(new { resultado = new RetornoJSON() { Erro = "Este controle já existe para este Risco e para esta fonte Geradora." } });

                            }


                        }
                    }
                }
                else
                {
                    
                    //ControleDeRiscos pTemp = ControleDeRiscosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.EControle.Equals(UKControle.Trim()) && a.UKFonteGeradora.Equals(pRec.FonteGeradora));




                    if (filtro.Count == 0)
                    {
                                               

                            ControleDeRiscosBusiness.Inserir(new ControleDeRiscos()
                            {
                                UKReconhecimentoDoRisco = pReconhecimento.UniqueKey,
                                EClassificacaoDaMedia = oControle.EClassificacaoDaMedia,
                                Controle = UKControle.Trim(),
                                EControle = oControle.EControle,
                                Descricao = oControle.Descricao,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });

                            ReconhecimentoBusiness.Inserir(pReconhecimento);

                    }
                    else
                    {

                        pReconhecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        ReconhecimentoBusiness.Terminar(pReconhecimento);

                        throw new Exception("Este controle já existe para este Risco e para esta fonte Geradora.");

                        // return Json(new { resultado = new RetornoJSON() { Erro = "Este controle já existe para este Risco e para esta fonte Geradora." } });

                    }
                }
                

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Controles dos riscos vinculados com sucesso." } });
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
               List<TipoDeControle> lista = TipoDeControleBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (TipoDeControle com in lista)
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
                TipoDeControle item = TipoDeControleBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

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
