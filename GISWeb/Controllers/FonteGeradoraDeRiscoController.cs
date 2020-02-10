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

namespace GISWeb.Controllers
{
    public class FonteGeradoraDeRiscoController : BaseController
    {


        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

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

        // GET: FonteGeradoraDeRisco
        public ActionResult Index()
        {
            

            ViewBag.Estab = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarWorkArea(FonteGeradoraViewModel entidade)
        {
            try
            {
                List<WorkArea> lista = new List<WorkArea>();

                //string sql = @"select wa.UniqueKey,wa.UKEstabelecimento, wa.Nome, wa.Descricao, 
                //            f.Uniquekey as relwaf, f.FonteGeradora, f.Descricao 	                                  
                //               from [dbGestor].[dbo].[tbWorkArea] wa 
                //            left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UKWorkArea = wa.UniqueKey  and f.DataExclusao = '9999-12-31 23:59:59.997'
                //               where wa.DataExclusao ='9999-12-31 23:59:59.997'  and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + @"'
                //               order by wa.UniqueKey ";



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
                                    Perigos = new List<Perigo>()

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


                                obj.FonteGeradoraDeRisco.Add(oFonte);
                            }

                        }

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
                                    
                                    obj.FonteGeradoraDeRisco.Add(oFonte);
                                }

                                
                                else if (oFonte.FonteGeradora.Equals(row["FonteGeradora"].ToString()))
                                {
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
                                }
                                else
                                {
                                    oFonte = new FonteGeradoraDeRisco()
                                    {
                                        UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                        FonteGeradora = row["FonteGeradora"].ToString(),
                                        Descricao = row["Descricao"].ToString(),
                                        Perigos = new List<Perigo>()
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
                                    Perigos = new List<Perigo>()

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
                                        UKPerigo = rTemp.UniqueKey ,                                     
                                        
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



    }


}