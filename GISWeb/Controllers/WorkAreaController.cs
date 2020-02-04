using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO;
using GISWeb.Infraestrutura.Provider.Concrete;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.DTO.Estabelecimento;
using System.Collections.Generic;
using GISModel.DTO.WorkArea;
using System.Data;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class WorkAreaController : BaseController
    {

        #region inject

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }

        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_WorkAreaPerigo> REL_WorkAreaPerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_PerigoRisco> REL_PerigoRiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion



        
        public ActionResult Index()
        {
            ViewBag.Estab = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();  

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarWorkArea(WorkAreaViewModel entidade)
        {
            try
            {
                List<WorkArea> lista = new List<WorkArea>();

                string sql = @"select wa.UniqueKey, wa.Nome, wa.Descricao, 
	                                  r1.Uniquekey as relwap, p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr, r.UniqueKey as ukrisco, r.Nome as risco 
                               from tbWorkArea wa 
	                                left join REL_WorkAreaPerigo r1 on r1.UKWorkArea = wa.UniqueKey  and r1.DataExclusao = '9999-12-31 23:59:59.997' 
	                                left join tbPerigo p on r1.UKPerigo = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997' 
	                                left join REL_PerigoRisco r2 on r2.UKPerigo = p.UniqueKey and r2.DataExclusao ='9999-12-31 23:59:59.997' 
	                                left join tbRisco r on r2.UKRisco = r.UniqueKey  and r.DataExclusao = '9999-12-31 23:59:59.997' 
                               where wa.DataExclusao ='9999-12-31 23:59:59.997'  and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + @"' 
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
                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                Perigos = new List<Perigo>()
                            };


                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                oPerigo = new Perigo()
                                {
                                    ID = Guid.Parse(row["relwap"].ToString()),
                                    UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                    Descricao = row["perigo"].ToString(),
                                    Riscos = new List<Risco>()
                                };

                                if (!string.IsNullOrEmpty(row["relpr"].ToString())) {
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
                        else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqueKey"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                if (oPerigo == null)
                                {
                                    oPerigo = new Perigo()
                                    {
                                        ID = Guid.Parse(row["relwap"].ToString()),
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
                                else if (oPerigo.Descricao.Equals(row["perigo"].ToString()))
                                {
                                    if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                    {
                                        oPerigo.Riscos.Add(new Risco()
                                        {
                                            ID = Guid.Parse(row["relpr"].ToString()),
                                            UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                            Nome = row["risco"].ToString()
                                        });
                                    }
                                }
                                else
                                {
                                    oPerigo = new Perigo()
                                    {
                                        ID = Guid.Parse(row["relwap"].ToString()),
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
                        else
                        {
                            lista.Add(obj);

                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                Perigos = new List<Perigo>()
                            };


                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                oPerigo = new Perigo()
                                {
                                    ID = Guid.Parse(row["relwap"].ToString()),
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

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        public ActionResult Novo()
        {

            ViewBag.Estabelecimentos = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Cadastrar(WorkArea entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    WorkAreaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "WorkArea '" + entidade.Nome + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "WorkArea") } });
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
            Guid ID = Guid.Parse(id);
            ViewBag.Workarea = new SelectList(WorkAreaBusiness.Consulta.ToList(), "ID", "Nome");

            var lista = WorkAreaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(ID)));                       


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(WorkArea entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    WorkAreaBusiness.Alterar(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "A WorkArea '" + entidade.Nome + "' foi atualizado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "WorkArea") } });
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
        public ActionResult VincularPerigo(string UKWorkArea) {

            ViewBag.UKWorkArea = UKWorkArea;

            return PartialView("_VincularPegigo");
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularPerigoAWorkArea(string UKWorkArea, string Perigos)
        {
            try
            {
                if (string.IsNullOrEmpty(UKWorkArea))
                    throw new Exception("Não foi possível localizar a identificação da work área.");

                if (string.IsNullOrEmpty(Perigos))
                    throw new Exception("Nenhum perigo recebido como parâmetro para vincular a work área.");

                Guid guidWA = Guid.Parse(UKWorkArea);

                if (Perigos.Contains(","))
                {
                    foreach (string perigo in Perigos.Split(','))
                    {
                        if (!string.IsNullOrEmpty(perigo.Trim()))
                        {
                            Perigo pTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(perigo.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_WorkAreaPerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKWorkArea.Equals(guidWA) && a.UKPerigo.Equals(pTemp.UniqueKey)).Count() == 0)
                                {
                                    REL_WorkAreaPerigoBusiness.Inserir(new REL_WorkAreaPerigo()
                                    {
                                        UKWorkArea = guidWA,
                                        UKPerigo = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Perigo pTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(Perigos.Trim()));
                    if (pTemp != null)
                    {
                        if (REL_WorkAreaPerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKWorkArea.Equals(guidWA) && a.UKPerigo.Equals(pTemp.UniqueKey)).Count() == 0)
                        {
                            REL_WorkAreaPerigoBusiness.Inserir(new REL_WorkAreaPerigo()
                            {
                                UKWorkArea = guidWA,
                                UKPerigo = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Perigo relacionado a work area com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult TerminarRelComWorkArea(string UKWorkArea, string UKPerigo)
        {
            try
            {
                if (string.IsNullOrEmpty(UKWorkArea))
                    throw new Exception("Não foi possível localizar a identificação da work área.");

                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Não foi possível localizar a identificação do perigo.");

                Guid guidWA = Guid.Parse(UKWorkArea);
                Guid guidPerigo = Guid.Parse(UKPerigo);

                REL_WorkAreaPerigo rel = REL_WorkAreaPerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKWorkArea.Equals(guidWA) && a.UKPerigo.Equals(guidPerigo));
                if (rel == null)
                {
                    throw new Exception("Não foi possível localizar o relacionamento entre Work Área e Perigo na base de dados.");
                }

                rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                REL_WorkAreaPerigoBusiness.Terminar(rel);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Perigo desvinculado com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }






        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularRisco(string UKPerigo)
        {

            ViewBag.UKPerigo = UKPerigo;

            return PartialView("_VincularRisco");
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularRiscoAoPerigo(string UKPerigo, string Riscos)
        {
            try
            {
                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Não foi possível localizar a identificação do perigo.");

                if (string.IsNullOrEmpty(Riscos))
                    throw new Exception("Nenhum risco recebido como parâmetro para vincular ao perigo.");

                Guid guidPerigo = Guid.Parse(UKPerigo);

                if (Riscos.Contains(","))
                {
                    foreach (string risk in Riscos.Split(','))
                    {
                        if (!string.IsNullOrEmpty(risk.Trim()))
                        {
                            Risco rTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(risk.Trim()));
                            if (rTemp != null)
                            {
                                if (REL_PerigoRiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKPerigo.Equals(guidPerigo) && a.UKPerigo.Equals(rTemp.UniqueKey)).Count() == 0)
                                {
                                    REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                                    {
                                        UKPerigo = guidPerigo,
                                        UKRisco = rTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Risco rTemp = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(Riscos.Trim()));
                    if (rTemp != null)
                    {
                        if (REL_PerigoRiscoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKPerigo.Equals(guidPerigo) && a.UKPerigo.Equals(rTemp.UniqueKey)).Count() == 0)
                        {
                            REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                            {
                                UKPerigo = guidPerigo,
                                UKRisco = rTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Risco relacionado ao perigo com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult TerminarRelComPerigo(string UKRisco, string UKPerigo)
        {
            try
            {
                if (string.IsNullOrEmpty(UKRisco))
                    throw new Exception("Não foi possível localizar a identificação do risco.");

                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Não foi possível localizar a identificação do perigo.");

                Guid guidRisco = Guid.Parse(UKRisco);
                Guid guidPerigo = Guid.Parse(UKPerigo);

                REL_PerigoRisco rel = REL_PerigoRiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(guidRisco) && a.UKPerigo.Equals(guidPerigo));
                if (rel == null)
                {
                    throw new Exception("Não foi possível localizar o relacionamento entre Risco e Perigo na base de dados.");
                }

                rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                REL_PerigoRiscoBusiness.Terminar(rel);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Perigo desvinculado com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }




    }
}