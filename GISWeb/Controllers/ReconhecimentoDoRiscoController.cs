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



            var objFonte = FonteGeradoraDeRiscoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKFont));
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
                                    Descricao = row["Descricao"].ToString(),
                                    Riscos = new List<Risco>()
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
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
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
                                    Descricao = row["Descricao"].ToString(),
                                    Riscos = new List<Risco>()
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