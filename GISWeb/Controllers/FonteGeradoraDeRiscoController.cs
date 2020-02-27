using GISCore.Business.Abstract;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.DTO.FonteGeradoraDeRisco;
using System.Data;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.Enums;

namespace GISWeb.Controllers
{
    public class FonteGeradoraDeRiscoController : BaseController
    {

        #region

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IArquivoBusiness ArquivoBusiness { get; set; }

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Perigo> PerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraBusiness { get; set; }
        [Inject]
        public IREL_FontePerigoBusiness REL_FontePerigoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public IControleDeRiscoBusiness ControleDeRiscosBusiness { get; set; }

        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeControle> TipoDeControleBusiness { get; set; }

        #endregion




        public ActionResult Index()
        {


            ViewBag.Estab = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult CriarControle(string UKWorkarea, string UKRisco)
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
                WorkArea obj = null;
                Perigo oPerigo = null;

                foreach (DataRow row in result.Rows)
                {
                    if (obj == null)
                    {
                        //obj = new WorkArea()
                        //{
                        //    UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                        //    Nome = row["Nome"].ToString(),
                        //    Descricao = row["Descricao"].ToString(),
                        //    Perigos = new List<Perigo>()
                        //};


                        //if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                        //{
                        //    oPerigo = new Perigo()
                        //    {
                        //        ID = Guid.Parse(row["relwap"].ToString()),
                        //        UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                        //        Descricao = row["perigo"].ToString(),
                        //        Riscos = new List<Risco>()
                        //    };

                        //    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                        //    {
                        //        oPerigo.Riscos.Add(new Risco()
                        //        {
                        //            ID = Guid.Parse(row["relpr"].ToString()),
                        //            UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                        //            Nome = row["risco"].ToString()
                        //        });
                        //    }

                        //    obj.Perigos.Add(oPerigo);
                        //}

                    }
                }
                if (obj != null)
                    lista.Add(obj);
            }




            return PartialView("_CadastrarControleDeRisco", lista);


        }

        public ActionResult CadastrarControleDeRisco(ReconhecimentoDoRisco entidade, ControleDeRiscos oControle, string UKControle, string UKWorkarea, string UKRisco)
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
                    //FonteGeradora = entidade.FonteGeradora,
                    Tragetoria = entidade.Tragetoria,
                    EClasseDoRisco = entidade.EClasseDoRisco,
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
                                                     //FonteGer = A.FonteGeradora,
                                                     UniqueControle = B.UKReconhecimentoDoRisco,
                                                     //Controle = B.Controle

                                                 };


                            if (pesControRisco != null)
                            {
                                foreach (var item in pesControRisco)
                                {
                                    //if (item.Controle.Equals(ativ.Trim()) && item.FonteGer.Equals(pRec.FonteGeradora))
                                    //{
                                    //    filtro.Add(item.UniqueReconhecimento);
                                    //}

                                }

                            }


                            //ControleDeRiscos pTemp = ControleDeRiscosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.EControle.Equals(ativ.Trim()) && a.UKFonteGeradora.Equals(pRec.FonteGeradora));

                            if (filtro.Count == 0)
                            {

                                ControleDeRiscosBusiness.Inserir(new ControleDeRiscos()
                                {
                                    UKReconhecimentoDoRisco = pReconhecimento.UniqueKey,
                                    EClassificacaoDaMedia = oControle.EClassificacaoDaMedia,
                                    //Controle = ativ.Trim(),
                                    EControle = oControle.EControle,
                                    //Descricao = oControle.Descricao,
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
                            //Controle = UKControle.Trim(),
                            EControle = oControle.EControle,
                            //Descricao = oControle.Descricao,
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





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarWorkArea(FonteGeradoraViewModel entidade)
        {
            try
            {
                List<WorkArea> lista = new List<WorkArea>();


                string sql = @"select wa.UniqueKey as UniqWa, wa.UKEstabelecimento, wa.Nome, wa.Descricao,f.UniqueKey as UniqFon, f.FonteGeradora, 
	                                  r1.Uniquekey as relfp,
									  p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr,r.UniqueKey as ukrisco, r.Nome as risco 
                               from tbWorkArea wa 	                               								
									left join tbFonteGeradoraDeRisco f on f.UKWorkarea = wa.UniqueKey and f.DataExclusao ='9999-12-31 23:59:59.997' 
									left join REL_FontePerigo r1 on r1.UKFonteGeradora = f.UniqueKey and r1.DataExclusao ='9999-12-31 23:59:59.997' 
									left join tbPerigo p on r1.UKPerigo = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997' 
									left join REL_PerigoRisco r2 on r2.UKPerigo = p.UniqueKey and r2.DataExclusao ='9999-12-31 23:59:59.997' 	
	                                left join tbRisco r on r2.UKRisco = r.UniqueKey  and r.DataExclusao = '9999-12-31 23:59:59.997' 
                               where wa.DataExclusao ='9999-12-31 23:59:59.997' 
							   and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + @"'
                               order by wa.UniqueKey";





                DataTable result = WorkAreaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    WorkArea obj = null;
                    FonteGeradoraDeRisco oFonte = null;
                    Perigo oPerigo = null;

                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqWa"].ToString()),
                                UKEstabelecimento = Guid.Parse(row["UKEstabelecimento"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                FonteGeradoraDeRisco = new List<FonteGeradoraDeRisco>()
                            };


                            if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                            {
                                oFonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString(),
                                    Perigos = new List<Perigo>(),
                                    Riscos = new List<Risco>()


                                };

                                if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                                {
                                    oFonte.Perigos.Add(new Perigo()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["UKPerigo"].ToString()),
                                        Descricao = row["perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    });
                                }
                                if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                {
                                    oFonte.Riscos.Add(new Risco()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                        Nome = row["risco"].ToString(),

                                    });
                                }


                                obj.FonteGeradoraDeRisco.Add(oFonte);
                            }

                        }
                        //
                        else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqWa"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                            {
                                if (oFonte == null)
                                {
                                    oFonte = new FonteGeradoraDeRisco()
                                    {
                                        UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                        FonteGeradora = row["FonteGeradora"].ToString(),
                                        Descricao = row["Descricao"].ToString(),
                                        Perigos = new List<Perigo>(),
                                        Riscos = new List<Risco>()


                                    };

                                    if (oFonte.Perigos.Equals(row["perigo"].ToString()))
                                    {
                                        oFonte.Perigos.Add(new Perigo()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["UKPerigo"].ToString()),
                                            Descricao = row["perigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        });
                                    }
                                    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oFonte.Riscos.Add(new Risco()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                            Nome = row["risco"].ToString(),

                                        });
                                    }

                                    obj.FonteGeradoraDeRisco.Add(oFonte);
                                }

                                //
                                else if (oFonte.FonteGeradora.Equals(row["FonteGeradora"].ToString()))
                                {
                                    if (oFonte.Perigos.Equals(row["perigo"].ToString()))
                                    {
                                        oFonte.Perigos.Add(new Perigo()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["UKPerigo"].ToString()),
                                            Descricao = row["perigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        });
                                    }
                                    if (string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oFonte.Riscos.Add(new Risco()
                                        {
                                            //ID = Guid.Parse(row["relpr"].ToString()),
                                            //UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                            Nome = row["risco"].ToString(),

                                        });
                                    }
                                }
                                else
                                {
                                    oFonte = new FonteGeradoraDeRisco()
                                    {
                                        UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                        FonteGeradora = row["FonteGeradora"].ToString(),
                                        Descricao = row["Descricao"].ToString(),
                                        Perigos = new List<Perigo>(),
                                        Riscos = new List<Risco>()


                                    };

                                    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oFonte.Perigos.Add(new Perigo()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["UKPerigo"].ToString()),
                                            Descricao = row["perigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        });
                                    }
                                    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oFonte.Riscos.Add(new Risco()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                            Nome = row["risco"].ToString(),

                                        });
                                    }

                                    obj.FonteGeradoraDeRisco.Add(oFonte);
                                }


                            }
                        }
                        else
                        {
                            lista.Add(obj);

                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqWa"].ToString()),
                                UKEstabelecimento = Guid.Parse(row["UKEstabelecimento"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                FonteGeradoraDeRisco = new List<FonteGeradoraDeRisco>()
                            };


                            if (!string.IsNullOrEmpty(row["relfp"].ToString()))
                            {
                                oFonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString(),
                                    Perigos = new List<Perigo>(),
                                    Riscos = new List<Risco>()


                                };

                                if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                {
                                    oFonte.Perigos.Add(new Perigo()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["UKPerigo"].ToString()),
                                        Descricao = row["perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    });
                                }
                                if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                {
                                    oFonte.Riscos.Add(new Risco()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                        Nome = row["risco"].ToString(),

                                    });
                                }

                                obj.FonteGeradoraDeRisco.Add(oFonte);
                            }
                        }
                    }

                    if (obj != null)
                        lista.Add(obj);

                }

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }




        }




        public ActionResult Novo(string UKWorarea)
        {

            ViewBag.workarea = UKWorarea;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(FonteGeradoraDeRisco oFonteGeradoraDeRisco, string UKWorkarea)
        {
            Guid UK_WorkArea = Guid.Parse(UKWorkarea);

            if (ModelState.IsValid)
            {
                try
                {
                    oFonteGeradoraDeRisco.UKWorkArea = UK_WorkArea;
                    FonteGeradoraDeRiscoBusiness.Inserir(oFonteGeradoraDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "a Fonte Geradora de riscos '" + oFonteGeradoraDeRisco.FonteGeradora + "' foi cadastrado com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FonteGeradoraDeRisco", new { id = oFonteGeradoraDeRisco.ID }) } });

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
        [RestritoAAjax]
        public ActionResult VincularPerigo(string UKFonteGeradora)
        {

            ViewBag.UKFonteGeradora = UKFonteGeradora;

            return PartialView("_VincularPerigo");
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularPerigoAFonte(string UKPerigo, string UKFonte)
        {
            try
            {
                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Não foi possível localizar a identificação do perigo.");

                if (string.IsNullOrEmpty(UKFonte))
                    throw new Exception("Nenhuma fonte recebida como parâmetro para vincular ao perigo.");

                Guid guidFonte = Guid.Parse(UKFonte);


                if (UKPerigo.Contains(","))
                {
                    foreach (string risk in UKPerigo.Split(','))
                    {
                        if (!string.IsNullOrEmpty(risk.Trim()))
                        {


                            Perigo rTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(risk.Trim()));

                            if (rTemp != null)
                            {
                                if (REL_FontePerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKFonteGeradora.Equals(guidFonte) && a.UKPerigo.Equals(rTemp.UniqueKey)).Count() == 0)
                                {
                                    REL_FontePerigo FontePerigo = new REL_FontePerigo()
                                    {
                                        UKFonteGeradora = guidFonte,
                                        UKPerigo = rTemp.UniqueKey,

                                        //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    };

                                    REL_FontePerigoBusiness.Inserir(FontePerigo);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Perigo rTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(UKPerigo));
                    if (rTemp != null)
                    {
                        if (REL_FontePerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKFonteGeradora.Equals(guidFonte) && a.UKPerigo.Equals(rTemp.UniqueKey)).Count() == 0)
                        {
                            REL_FontePerigo FontePerigo = new REL_FontePerigo()
                            {
                                UKFonteGeradora = guidFonte,
                                UKPerigo = rTemp.UniqueKey,

                                //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            };

                            REL_FontePerigoBusiness.Inserir(FontePerigo);
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Perigo relacionado a Fonte Geradora de Risco com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }



        [RestritoAAjax]
        public ActionResult BuscarPerigoForAutoComplete(string key)
        {
            try
            {
                List<string> riscosAsString = new List<string>();
                List<Perigo> lista = PerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Perigo com in lista)
                    riscosAsString.Add(com.Descricao);

                return Json(new { Result = riscosAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarPerigoForAutoComplete(string key)
        {
            try
            {
                Perigo item = PerigoBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }




        [HttpPost]
        [RestritoAAjax]
        public ActionResult ListarArquivosAnexados(string UKObjeto)
        {
            Guid uk = Guid.Parse(UKObjeto);
            ViewBag.UKObjeto = UKObjeto;

            List<Arquivo> arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(uk)).ToList();

            return PartialView("_Arquivos", arquivos);
        }



    }
}