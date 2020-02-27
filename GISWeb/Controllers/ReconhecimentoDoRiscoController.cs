using GISCore.Business.Abstract;
using GISModel.DTO.GerenciamentoDoRisco;
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
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class ReconhecimentoDoRiscoController : BaseController
    {

        #region inject

        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }

        [Inject]
        public IControleDeRiscoBusiness ControleDeRiscosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_RiscoControle> REL_RiscoControlesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeControle> TipoDeControleBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CriarControle(string UKWorkarea, string UKFonte, string UKPerigo, string UKRisco)
        {

            ViewBag.UKWorkArea = UKWorkarea;

            ViewBag.UKFonte = UKFonte;
            ViewBag.UKPerigo = UKPerigo;
            ViewBag.UKRisco = UKRisco;

            var UKWork = Guid.Parse(UKWorkarea);
            var UKFont = Guid.Parse(UKFonte);

            var UKPerig = Guid.Parse(UKPerigo);
            var UKRisc = Guid.Parse(UKRisco);




            var objWorkArea = WorkAreaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKWork));
            if (objWorkArea == null)
                throw new Exception("Não foi possível recuperar a workarea na base de dados. Tente novamente ou acione o administrador do sitema.");

            ViewBag.WorkArea = objWorkArea.Nome;



            var objFonte =  FonteGeradoraDeRiscoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKFont));
            if (objFonte == null)
                throw new Exception("Não foi possível recuperar a fonte geradora do risco na base de dados. Tente novamente ou acione o administrador do sitema.");

            ViewBag.FonteGeradora = objFonte.FonteGeradora;



            var objPerigo = PerigoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKPerig));
            if (objPerigo == null)
                throw new Exception("Não foi possível recuperar a workarea na base de dados. Tente novamente ou acione o administrador do sitema.");

            ViewBag.Perigo = objPerigo.Descricao;


            Risco objRisco = RiscoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKRisc));
            if (objRisco == null)
                throw new Exception("Não foi possível recuperar o risco na base de dados. Tente novamente ou acione o administrador do sitema.");

            ViewBag.Risco = objRisco.Nome;

            



            ViewBag.UKFonte = UKFont;
            FonteGeradoraDeRisco Fonte = FonteGeradoraDeRiscoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKFont));


            ViewBag.NomeDaFonte = Fonte.FonteGeradora;


                var Nome = RiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKRisc))).ToList();



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


            return PartialView("_CadastrarControleDeRisco");

        }

        public ActionResult CadastrarControleDeRisco(VMNovoReconhecimentoControle entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<TipoDeControle> tiposDeControle = new List<TipoDeControle>();




            ViewBag.FonteGeradora = new SelectList(FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "UniqueKey", "FonteGeradora");

            List<WorkArea> lista = new List<WorkArea>();

            string sql = @"select wa.UniqueKey, wa.Nome, wa.Descricao, 
	                                  f.UniqueKey as ukfonte, f.FonteGeradora,
	                                  r1.Uniquekey as relfp, 
	                                  p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr, 
	                                  r.UniqueKey as ukrisco, r.Nome as risco 									  
                               from [dbGestor].[dbo].[tbWorkArea] wa 
	                               left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on wa.UniqueKey = f.UKWorkArea and f.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join  [dbGestor].[dbo].[REL_FontePerigo] r1 on r1.UKFonteGeradora = f.UniqueKey  and r1.DataExclusao = '9999-12-31 23:59:59.997'
	                               left join [dbGestor].[dbo].[tbPerigo] p on r1.UKPerigo = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join [dbGestor].[dbo].[REL_PerigoRisco] r2 on r2.UKPerigo = p.UniqueKey and r2.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join [dbGestor].[dbo].[tbRisco] r on r2.UKRisco = r.UniqueKey  and r.DataExclusao = '9999-12-31 23:59:59.997' 
                              where wa.DataExclusao = '9999-12-31 23:59:59.997' and wa.UniqueKey = '" + UKWorkarea + @"' and f.UniqueKey = '" + UKFonte + @"' 
                              and r.UniqueKey = '" + UKRisco + @"'
                              order by wa.UniqueKey";

            
            DataTable result = WorkAreaBusiness.GetDataTable(sql);

            if (result.Rows.Count > 0)
            {
                WorkArea obj = null;
                Perigo oPerigo = null;

                foreach (DataRow row in result.Rows)
                {
                    if (obj == null)
                    {
                        obj = new WorkArea()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            Nome = row["Nome"].ToString(),
                            Descricao = row["Descricao"].ToString(),
                            Perigos = new List<Perigo>()
                        };


                        if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                        {
                            oPerigo = new Perigo()
                            {
                                ID = Guid.Parse(row["relfp"].ToString()),
                                UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                Descricao = row["perigo"].ToString(),
                                Riscos = new List<Risco>()
                            };

                            if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                            {
                                oPerigo.Riscos.Add(new Risco()
                                {
                                    ID = Guid.Parse(row["relpr"].ToString()),
                                    UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                    Nome = row["risco"].ToString()
                                });
                            }

                            obj.Perigos.Add(oPerigo);
                        }

                    }
                }
                if (obj != null)
                    lista.Add(obj);
            }




            return PartialView("_CadastrarControleDeRisco", lista);

        }


        public ActionResult CadastrarControleDeRisco(ReconhecimentoDoRisco entidade, ControleDeRiscos oControle, string UKControle, string UKWorkarea, string UKRisco, string UKFonte)
        {
            try
            {
                Guid UK_Workarea = Guid.Parse(UKWorkarea);
                Guid UK_Risco = Guid.Parse(UKRisco);
                Guid UK_Fonte = Guid.Parse(UKFonte);


                    if (entidade.TiposDeControle.Contains(","))
                    {
                        foreach (string ativ in entidade.TiposDeControle.Split(','))
                        {
                            if (!string.IsNullOrEmpty(ativ.Trim()))
                            {
                                TipoDeControle tipoControl = TipoDeControleBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Trim().ToUpper().Equals(ativ.Trim().ToUpper()));

                                if (tipoControl == null)
                                    throw new Exception("Não foi possível encontrar o tipo de controle '" + ativ.Trim() + "' na base de dados.");

                                tiposDeControle.Add(tipoControl);
                            }
                        }
                    }
                    else
                    {
                        TipoDeControle tipoControl = TipoDeControleBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Trim().ToUpper().Equals(entidade.TiposDeControle.Trim().ToUpper()));

                        if (tipoControl == null)
                            throw new Exception("Não foi possível encontrar o tipo de controle '" + entidade.TiposDeControle.Trim() + "' na base de dados.");

                        tiposDeControle.Add(tipoControl);
                    }


//<<<<<<< Johnny-v1
//=======
                ReconhecimentoDoRisco pReconhecimento = new ReconhecimentoDoRisco()
                {
                    UKWorkarea = UK_Workarea,
                    UKRisco = UK_Risco,
                    UKFonteGeradora = UK_Fonte,
                    Tragetoria = entidade.Tragetoria,
                    EClasseDoRisco = entidade.EClasseDoRisco,
                    //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
//>>>>>>> master






                    ReconhecimentoDoRisco oReconhecimento = ReconhecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) &&
                                p.UKWorkarea.Equals(entidade.UKWorkarea) &&
                                p.UKFonteGeradora.Equals(entidade.UKFonteGeradora) &&
                                p.UKPerigo.Equals(entidade.UKPerigo) &&
                                p.UKRisco.Equals(entidade.UKRisco)
                    );

                    if (oReconhecimento == null)
                    {
                        oReconhecimento = new ReconhecimentoDoRisco()
                        {
//<<<<<<< Johnny-v1
                            UKWorkarea = entidade.UKWorkarea,
                            UKFonteGeradora = entidade.UKFonteGeradora,
                            UKPerigo = entidade.UKPerigo,
                            UKRisco = entidade.UKRisco,
                            Tragetoria = entidade.Tragetoria,
                            EClasseDoRisco = entidade.EClasseDoRisco,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };

                        ReconhecimentoBusiness.Inserir(oReconhecimento);
                    }
//=======
                            var pesControRisco = from A in ReconhecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 join B in ControleDeRiscosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 on A.UniqueKey equals B.UKReconhecimentoDoRisco
                                                 select new
                                                 {
                                                     UniqueReconhecimento = A.UniqueKey,
                                                     UKWorka = A.UKWorkarea,
                                                     Risco = A.UKRisco,
                                                     FonteGer = A.UKFonteGeradora,
                                                     UniqueControle = B.UKReconhecimentoDoRisco,
                                                     Control = B.Controle

                                                 };


                            if (pesControRisco != null)
                            {
                                foreach (var item in pesControRisco)
                                {
                                    if (item.Control.Equals(ativ.Trim()) && item.FonteGer.Equals(pRec.UKFonteGeradora))
                                    {
                                        filtro.Add(item.UniqueReconhecimento);
                                    }

                                }
//>>>>>>> master


                    

                    List<Guid> filtro = new List<Guid>();

                    if (entidade.TiposDeControle.Contains(","))
                    {
                        foreach (string ativ in entidade.TiposDeControle.Split(','))
                        {
                            if (!string.IsNullOrEmpty(ativ.Trim()))
                            {
                                TipoDeControle tipoControl = tiposDeControle.FirstOrDefault(a => a.Descricao.Trim().ToUpper().Equals(ativ.Trim().ToUpper()));

                                ControleDeRiscos obj = new ControleDeRiscos()
                                {
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                    UKReconhecimentoDoRisco = oReconhecimento.UniqueKey,
                                    UKTipoDeControle = tipoControl.UniqueKey,
                                    EClassificacaoDaMedia = entidade.EClassificacaoDaMedia,
                                    EControle = entidade.EControle
                                };

                                ControleDeRiscosBusiness.Inserir(obj);

                            }
                        }
                    }
                    else
                    {
                        TipoDeControle tipoControl = tiposDeControle.FirstOrDefault(a => a.Descricao.Trim().ToUpper().Equals(entidade.TiposDeControle.Trim().ToUpper()));

                        ControleDeRiscos obj = new ControleDeRiscos()
                        {
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            UKReconhecimentoDoRisco = oReconhecimento.UniqueKey,
                            UKTipoDeControle = tipoControl.UniqueKey,
                            EClassificacaoDaMedia = entidade.EClassificacaoDaMedia,
                            EControle = entidade.EControle
                        };

                        ControleDeRiscosBusiness.Inserir(obj);
                    }


                    return Json(new { resultado = new RetornoJSON() { Sucesso = "Reconhecimento e controles dos riscos cadastrados com sucesso." } });
                }
                else
                {
                    return Json(new { resultado = TratarRetornoValidacaoToJSON() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }



        public ActionResult ListaReconhecimentoPorWorkArea(string ukWorkArea)
        {

            try
            {

                List<ReconhecimentoDoRisco> lista = new List<ReconhecimentoDoRisco>();

                string sql = @"select w.Nome, f.FonteGeradora, per.Descricao, risc.Nome, r.Tragetoria, r.EClasseDoRisco, tc.Descricao, c.EClassificacaoDaMedia, c.EControle
                               from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
		                                left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and r.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
                               where r.UKWorkArea = '" + ukWorkArea + @"' 
                               order by c.UniqueKey";


                DataTable result = ReconhecimentoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    ReconhecimentoDoRisco obj = null;
                    WorkArea oWork = null;

                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new ReconhecimentoDoRisco()
                            {
                                UniqueKey = Guid.Parse(row["ukreconhecimento"].ToString())//,                                
                            };

                            if (!string.IsNullOrEmpty(row["UniqWa"].ToString()))
                            {
                                oWork = new WorkArea()
                                {
                                    UniqueKey = Guid.Parse(row["UniqWa"].ToString()),
                                    Nome = row["Nome"].ToString(),
                                    FonteGeradoraDeRisco = new List<FonteGeradoraDeRisco>()
                                };

                                oWork.FonteGeradoraDeRisco.Add(new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UKFonte"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString()
                                });
                            }

                        }


                    }

                    if (obj != null)
                         lista.Add(obj);
                }

                     return PartialView("_PesquisaRiscos", lista);
            }


            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message }});
            }



        }

        public ActionResult ListaReconhecimentoPorWorkAreaRisco(string ukWorkArea, string ukFonte, string ukPerigo, string ukRisco)
        {

            try
            {

                List<ReconhecimentoDoRisco> lista = new List<ReconhecimentoDoRisco>();

                string sql = @"select w.Nome, f.FonteGeradora, per.Descricao, risc.Nome, r.Tragetoria, r.EClasseDoRisco, tc.Descricao, c.EClassificacaoDaMedia, c.EControle
                               from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
		                                left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and r.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
                               where r.UKWorkArea = '" + ukWorkArea + @"' 
                               order by c.UniqueKey";


                DataTable result = ReconhecimentoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    ReconhecimentoDoRisco obj = null;
                    WorkArea oWork = null;


                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new WorkArea()
                            {

                                UniqueKey = Guid.Parse(row["ukreconhecimento"].ToString())//,                                
                            };

                            if (!string.IsNullOrEmpty(row["UniqWa"].ToString()))

                            {
                                oRec = new ReconhecimentoDoRisco()
                                {
                                    UniqueKey = Guid.Parse(row["recUniq"].ToString()),
                                    FonteGeradoraDeRiscos = new List<FonteGeradoraDeRisco>(),
                                    Controles = new List<ControleDeRiscos>()
                                };

                                oRec.Controles.Add(new ControleDeRiscos()
                                {

                                    UniqueKey = Guid.Parse(row["UKFonte"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString()
                                });

                            }
                            else
                            {
                                

                                if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                                {
                                    oPerigo = new Perigo()
                                    {
                                        ID = Guid.Parse(row["relfp"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                        Descricao = row["perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    };

                                    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oPerigo.Riscos.Add(new Risco()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                            Nome = row["risco"].ToString()
                                        });
                                    }

                                    oFont.Perigos.Add(oPerigo);
                                }

                                obj.FonteGeradoraDeRisco.Add(oFont);
                            }
                    }
                           
                            else
                            {
                                lista.Add(obj);

                                obj = new WorkArea()
                                {
                                    UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                    Nome = row["Nome"].ToString(),
                                    Descricao = row["Descricao"].ToString(),
                                    FonteGeradoraDeRisco = new List<FonteGeradoraDeRisco>()
                                };

                                if (!string.IsNullOrEmpty(row["ukfonte"].ToString()))
                                {
                                    oFont = new FonteGeradoraDeRisco()
                                    {
                                        UniqueKey = Guid.Parse(row["ukfonte"].ToString()),
                                        Descricao = row["FonteGeradora"].ToString(),
                                        Perigos = new List<Perigo>()
                                    };

                                    if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                                    {
                                        oPerigo = new Perigo()
                                        {
                                            ID = Guid.Parse(row["relfp"].ToString()),
                                            UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                            Descricao = row["perigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        };

                                        if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                        {
                                            oPerigo.Riscos.Add(new Risco()
                                            {
                                                ID = Guid.Parse(row["relpr"].ToString()),
                                                UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                                Nome = row["risco"].ToString()
                                            });
                                        }

                                        oFont.Perigos.Add(oPerigo);
                                    }

                                    obj.FonteGeradoraDeRisco.Add(oFont);
                                }
                            }
                        }
                    if (obj != null)
                        lista.Add(obj);

                }

                return PartialView("_PesquisaRiscos", lista);

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
