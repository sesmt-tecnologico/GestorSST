using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.DTO.WorkArea;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

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
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public IControleDeRiscoBusiness ControleDeRiscoBusiness { get; set; }

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

                string sql = @"select wa.UniqueKey,wa.UKEstabelecimento, wa.Nome, wa.Descricao, f.UniqueKey as UniqFon, f.FonteGeradora
                               from tbWorkArea wa
                               left join tbFonteGeradoraDeRisco f on f.UKWorkarea = wa.UniqueKey and f.DataExclusao ='9999-12-31 23:59:59.997'
                               where wa.DataExclusao ='9999-12-31 23:59:59.997'  and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + @"' 
                               order by wa.UniqueKey";

                DataTable result = WorkAreaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    WorkArea obj = null;
                    FonteGeradoraDeRisco oFonte = null;

                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                UKEstabelecimento = Guid.Parse(row["UKEstabelecimento"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                FonteGeradoraDeRisco = new List<FonteGeradoraDeRisco>()
                                //Perigos = new List<Perigo>()
                            };
                            if (!string.IsNullOrEmpty(row["UniqFon"].ToString()))
                            {
                                oFonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString(),



                                };
                            }
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

                            // obj.Perigos.Add(oPerigo);
                            obj.FonteGeradoraDeRisco.Add(oFonte);
                        }

                        //}
                        //else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqueKey"].ToString())))
                        //{
                        //    if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                        //    {
                        //        if (oPerigo == null)
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

                        //        else if (oPerigo.Descricao.Equals(row["perigo"].ToString()))
                        //        {
                        //            if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                        //            {
                        //                oPerigo.Riscos.Add(new Risco()
                        //                {
                        //                    ID = Guid.Parse(row["relpr"].ToString()),
                        //                    UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                        //                    Nome = row["risco"].ToString()
                        //                });
                        //            }
                        //        }
                        //        else
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
                            if (!string.IsNullOrEmpty(row["UniqFon"].ToString()))
                            {
                                oFonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UniqFon"].ToString()),
                                    FonteGeradora = row["FonteGeradora"].ToString(),
                                    Descricao = row["Descricao"].ToString(),



                                };

                                obj.FonteGeradoraDeRisco.Add(oFonte);
                            }

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

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        [HttpPost]
        public ActionResult BuscarWorkAreaParaPerfilEmpregado(string UKEstabelecimento)
        {
            try
            {
                List<WorkArea> lista = new List<WorkArea>();

                string sql = @"select wa.UniqueKey, wa.Nome, wa.Descricao, 
	                                  f.UniqueKey as ukfonte, f.FonteGeradora,
	                                  r1.Uniquekey as relfp, 
	                                  p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr, 
	                                  r.UniqueKey as ukrisco, r.Nome as risco 
                               from tbWorkArea wa 
	                               left join tbFonteGeradoraDeRisco f on wa.UniqueKey = f.UKWorkArea and f.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join REL_FontePerigo r1 on r1.UKFonteGeradora = f.UniqueKey  and r1.DataExclusao = '9999-12-31 23:59:59.997'
	                               left join tbPerigo p on r1.UKPerigo = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join REL_PerigoRisco r2 on r2.UKPerigo = p.UniqueKey and r2.DataExclusao = '9999-12-31 23:59:59.997'  
	                               left join tbRisco r on r2.UKRisco = r.UniqueKey  and r.DataExclusao = '9999-12-31 23:59:59.997' 
                              where wa.DataExclusao = '9999-12-31 23:59:59.997' and wa.UKEstabelecimento = '" + UKEstabelecimento + @"' 
                              order by wa.UniqueKey ";

                DataTable result = WorkAreaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    WorkArea obj = null;
                    FonteGeradoraDeRisco oFont = null;
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
                        else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqueKey"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(row["ukfonte"].ToString()))
                            {
                                if (oFont == null)
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
                                else if (oFont.UniqueKey.Equals(Guid.Parse(row["ukfonte"].ToString())))
                                {

                                    Perigo pTemp = oFont.Perigos.FirstOrDefault(a => a.Descricao.Equals(row["perigo"].ToString()));
                                    if (pTemp == null)
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
                                    else
                                    {
                                        Risco riskTemp = pTemp.Riscos.FirstOrDefault(a => a.Nome.Equals(row["risco"].ToString()));
                                        if (riskTemp == null)
                                        {
                                            pTemp.Riscos.Add(new Risco()
                                            {
                                                ID = Guid.Parse(row["relpr"].ToString()),
                                                UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                                Nome = row["risco"].ToString()
                                            });
                                        }
                                    }
                                }
                                else
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

                return PartialView("_PesquisaParaPerfilEmpregado", lista);
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
        public ActionResult Terminar(string id) 
        {
            try
            {
                Guid guidUK = Guid.Parse(id);
                WorkArea obj = WorkAreaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(guidUK));
                if (obj == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a workarea, pois a mesmo não foi localizada na base de dados." } });

                obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                WorkAreaBusiness.Terminar(obj);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "A WorkArea '" + obj.Nome + "' foi excluída com sucesso." } });
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




        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularPerigo(string UKPerigo)
        {

            ViewBag.UKPerigo = UKPerigo;

            return PartialView("_VincularPerigo");
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
                                    Risco r = new Risco()
                                    {
                                        UniqueKey = Guid.NewGuid(),
                                        Nome = rTemp.Nome,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    };
                                    RiscoBusiness.Inserir(r);

                                    REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                                    {
                                        UKPerigo = guidPerigo,
                                        UKRisco = r.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                                else
                                {
                                    throw new Exception("O risco selecionado já está vinculado com o perigo.");
                                }
                            }
                            else
                            {
                                throw new Exception("Não foi possível encontrar o risco na base de dados.");
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
                            Risco r = new Risco()
                            {
                                UniqueKey = Guid.NewGuid(),
                                Nome = rTemp.Nome,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            };
                            RiscoBusiness.Inserir(r);

                            REL_PerigoRiscoBusiness.Inserir(new REL_PerigoRisco()
                            {
                                UKPerigo = guidPerigo,
                                UKRisco = r.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                        else
                        {
                            throw new Exception("O risco selecionado já está vinculado com o perigo.");
                        }
                    }
                    else
                    {
                        throw new Exception("Não foi possível encontrar o risco na base de dados.");
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
                    throw new Exception("Não foi possível localizar o relacionamento entre Risco e Perigo na base de dados.");

                Risco rBanco = RiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(guidRisco));
                if (rBanco == null)
                    throw new Exception("Não foi possível localizar o risco na base de dados.");




                //Opicionais ##############################################################################################
                //#########################################################################################################

                ReconhecimentoDoRisco rec = ReconhecimentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKRisco.Equals(guidRisco));

                if (rec != null)
                {
                    ControleDeRiscos con = ControleDeRiscoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKReconhecimentoDoRisco.Equals(rec.UniqueKey));
                    if (con != null)
                    {
                        con.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        ControleDeRiscoBusiness.Terminar(con);
                    }

                    rec.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ReconhecimentoBusiness.Terminar(rec);
                }
                //#########################################################################################################




                rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                REL_PerigoRiscoBusiness.Terminar(rel);

                rBanco.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                RiscoBusiness.Terminar(rBanco);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Risco excluído com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }


    }
}