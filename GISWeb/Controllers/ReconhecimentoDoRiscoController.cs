﻿using GISCore.Business.Abstract;
using GISHelpers.Utils;
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
        public IArquivoBusiness ArquivoBusiness { get; set; }

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
        public IBaseBusiness<ClassificacaoMedida> ClassificacaoMedidaBusiness { get; set; }


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
                               Name = e.GetDisplayName()
                           };
            ViewBag.Eclasse = new SelectList(enumData, "ID", "Name");


            var enumData01 = from ETrajetoria e in Enum.GetValues(typeof(ETrajetoria))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.GetDisplayName()
                             };
            ViewBag.ETrajetoria = new SelectList(enumData01, "ID", "Name");


            

            return PartialView("_CadastrarControleDeRisco");

        }

        public ActionResult CadastrarControleDeRisco(VMNovoReconhecimentoControle entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<TipoDeControle> tiposDeControle = new List<TipoDeControle>();

                    if (entidade?.Controles?.Count == 0)
                        throw new Exception("Nenhum tipo de controle foi identificado.");

                    foreach (string[] item in entidade.Controles)
                    {
                        Guid UKTipo = Guid.Parse(item[0]);

                        TipoDeControle tipoControl = TipoDeControleBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKTipo));

                        if (tipoControl == null)
                            throw new Exception("Não foi possível encontrar um do(s) tipo(s) de controle na base de dados.");

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




                    foreach (string[] item in entidade.Controles)
                    {
                        Guid UKTipo = Guid.Parse(item[0]);
                        Guid UKClassificacaoMedida = Guid.Parse(item[1]);

                        ControleDeRiscos obj = new ControleDeRiscos()
                        {
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            UKReconhecimentoDoRisco = oReconhecimento.UniqueKey,
                            UKTipoDeControle = UKTipo,
                            UKClassificacaoDaMedia = UKClassificacaoMedida,
                            EControle = (EControle)Enum.Parse(typeof(EControle), item[2], true)
                        };

                        if (item[3] != null && !string.IsNullOrEmpty(item[3]))
                        {
                            obj.UKLink = Guid.Parse(item[3]);
                        }

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



        public ActionResult ListaReconhecimentoPorWorkArea(string id)
        {
            try
            {
                string sql = @"select w.UniqueKey as UKWorkArea, w.Nome as workarea, 
                                      f.UniqueKey as UKFonte, f.FonteGeradora, 
                                      per.Descricao as Perigo, 
                                      risc.UniqueKey as UKRisco, risc.Nome as Risco, 
                                      r.Tragetoria, r.EClasseDoRisco, 
                                      tc.UniqueKey as UKTipoControle, tc.Descricao as TipoControle, 
                                      c.UKClassificacaoDaMedia,  c.EControle, 
                                      cm.UniqueKey as UKcm, cm.Nome as NomeClass,
                                      lk.UniqueKey as UKLink, lk.URL as URLLInk
                               from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
		                                left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and r.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
										left join ClassificacaoMedidas cm on cm.UniqueKey = c.UKClassificacaoDaMedia and cm.DataExclusao ='9999-12-31 23:59:59.997'
                                        left join tbLink lk on lk.UniqueKey = c.UKLink and lk.DataExclusao ='9999-12-31 23:59:59.997'
                              where r.UKWorkArea = '" + id + @"' 
                              order by f.FonteGeradora, per.Descricao, risc.Nome";

                DataTable result = ReconhecimentoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    VMReconhecimento obj = null;
                    FonteGeradoraDeRisco fonte = null;
                    Perigo per = null;
                    Risco risk = null;
                    
                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new VMReconhecimento()
                            {
                                UKWorkArea = row["UKWorkArea"].ToString(),
                                WorkArea = row["workarea"].ToString(),
                                FontesGeradoras = new List<FonteGeradoraDeRisco>()
                            };

                            if (!string.IsNullOrEmpty(row["FonteGeradora"]?.ToString()))
                            {
                                fonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UKFonte"].ToString()),
                                    Descricao = row["FonteGeradora"].ToString(),
                                    Perigos = new List<Perigo>()
                                };

                                if (!string.IsNullOrEmpty(row["Perigo"]?.ToString()))
                                {
                                    per = new Perigo()
                                    {
                                        Descricao = row["Perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    };

                                    if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                    {
                                        risk = new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["UKRisco"].ToString()),
                                            Nome = row["Risco"].ToString(),
                                            Reconhecimento = new ReconhecimentoDoRisco()
                                            {
                                                Tragetoria = (ETrajetoria)Enum.Parse(typeof(ETrajetoria), row["Tragetoria"].ToString(), true),
                                                EClasseDoRisco = (EClasseDoRisco)Enum.Parse(typeof(EClasseDoRisco), row["EClasseDoRisco"].ToString(), true) ,
                                            },
                                            Controles = new List<ControleDeRiscos>()
                                        };

                                        if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                        {

                                            ControleDeRiscos control = new ControleDeRiscos()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoControle"].ToString()),
                                                TipoDeControle = row["TipoControle"].ToString(),
                                                Link = new Link(),
                                                UKClassificacaoDaMedia = Guid.Parse(row["UKClassificacaoDaMedia"].ToString()),
                                                EControle = (EControle)Enum.Parse(typeof(EControle), row["EControle"].ToString(), true),
                                                ClassificacaoMedida = new ClassificacaoMedida()
                                                { 
                                                    Nome = row["NomeClass"].ToString()
                                                }
                                            };

                                            if (!string.IsNullOrEmpty(row["UKLink"].ToString()))
                                            {
                                                control.Link.UniqueKey = Guid.Parse(row["UKLink"].ToString());
                                                control.Link.URL = row["URLLInk"].ToString();
                                            }

                                            risk.Controles.Add(control);
                                        } 
                                       


                                        per.Riscos.Add(risk);
                                    }

                                    fonte.Perigos.Add(per);
                                }

                                obj.FontesGeradoras.Add(fonte);
                            }
                        }
                        else
                        {
                            if (fonte.Descricao.Equals(row["FonteGeradora"].ToString()))
                            {
                                if (per.Descricao.Equals(row["Perigo"].ToString()))
                                {
                                    if (risk.Nome.Equals(row["Risco"].ToString()))
                                    {
                                        var control = new ControleDeRiscos()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoControle"].ToString()),
                                            TipoDeControle = row["TipoControle"].ToString(),
                                            Link = new Link(),
                                            UKClassificacaoDaMedia = Guid.Parse(row["UKClassificacaoDaMedia"].ToString()),
                                            EControle = (EControle)Enum.Parse(typeof(EControle), row["EControle"].ToString(), true),
                                            ClassificacaoMedida = new ClassificacaoMedida()
                                            {
                                                Nome = row["NomeClass"].ToString()
                                            }
                                        };

                                        if (!string.IsNullOrEmpty(row["UKLink"].ToString()))
                                        {
                                            control.Link.UniqueKey = Guid.Parse(row["UKLink"].ToString());
                                            control.Link.URL = row["URLLInk"].ToString();
                                        }

                                        risk.Controles.Add(control);

                                    }
                                    else
                                    {
                                        risk = new Risco()
                                        {
                                            Nome = row["Risco"].ToString(),
                                            Reconhecimento = new ReconhecimentoDoRisco()
                                            {
                                                Tragetoria = (ETrajetoria)Enum.Parse(typeof(ETrajetoria), row["Tragetoria"].ToString(), true),
                                                EClasseDoRisco = (EClasseDoRisco)Enum.Parse(typeof(EClasseDoRisco), row["EClasseDoRisco"].ToString(), true),
                                            },
                                            Controles = new List<ControleDeRiscos>()
                                        };

                                        if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                        {
                                            var control = new ControleDeRiscos()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoControle"].ToString()),
                                                TipoDeControle = row["TipoControle"].ToString(),
                                                Link = new Link(),
                                                UKClassificacaoDaMedia = Guid.Parse(row["UKClassificacaoDaMedia"].ToString()),
                                                EControle = (EControle)Enum.Parse(typeof(EControle), row["EControle"].ToString(), true),
                                                ClassificacaoMedida = new ClassificacaoMedida()
                                                {
                                                    Nome = row["NomeClass"].ToString()
                                                }
                                            };

                                            if (!string.IsNullOrEmpty(row["UKLink"].ToString()))
                                            {
                                                control.Link.UniqueKey = Guid.Parse(row["UKLink"].ToString());
                                                control.Link.URL = row["URLLInk"].ToString();
                                            }

                                            risk.Controles.Add(control);
                                        }


                                        per.Riscos.Add(risk);
                                    }

                                }
                                else
                                {

                                    per = new Perigo()
                                    {
                                        Descricao = row["Perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    };

                                    if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                    {
                                        risk = new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["UKRisco"].ToString()),
                                            Nome = row["Risco"].ToString(),
                                            Reconhecimento = new ReconhecimentoDoRisco()
                                            {
                                                Tragetoria = (ETrajetoria)Enum.Parse(typeof(ETrajetoria), row["Tragetoria"].ToString(), true),
                                                EClasseDoRisco = (EClasseDoRisco)Enum.Parse(typeof(EClasseDoRisco), row["EClasseDoRisco"].ToString(), true),
                                            },
                                            Controles = new List<ControleDeRiscos>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoControle"]?.ToString()))
                                        {
                                            var control = new ControleDeRiscos()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoControle"].ToString()),
                                                TipoDeControle = row["TipoControle"].ToString(),
                                                Link = new Link(),
                                                UKClassificacaoDaMedia = Guid.Parse(row["UKClassificacaoDaMedia"].ToString()),
                                                EControle = (EControle)Enum.Parse(typeof(EControle), row["EControle"].ToString(), true),
                                                ClassificacaoMedida = new ClassificacaoMedida()
                                                {
                                                    Nome = row["NomeClass"].ToString()
                                                }
                                            };

                                            if (!string.IsNullOrEmpty(row["UKLink"].ToString()))
                                            {
                                                control.Link.UniqueKey = Guid.Parse(row["UKLink"].ToString());
                                                control.Link.URL = row["URLLInk"].ToString();
                                            }

                                            risk.Controles.Add(control);
                                        }


                                        per.Riscos.Add(risk);
                                    }

                                    fonte.Perigos.Add(per);

                                }
                            }
                            else
                            {
                                fonte = new FonteGeradoraDeRisco()
                                {
                                    UniqueKey = Guid.Parse(row["UKFonte"].ToString()),
                                    Descricao = row["FonteGeradora"].ToString(),
                                    Perigos = new List<Perigo>()
                                };

                                if (!string.IsNullOrEmpty(row["Perigo"]?.ToString()))
                                {
                                    per = new Perigo()
                                    {
                                        Descricao = row["Perigo"].ToString(),
                                        Riscos = new List<Risco>()
                                    };

                                    if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                    {
                                        risk = new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["UKRisco"].ToString()),
                                            Nome = row["Risco"].ToString(),
                                            Reconhecimento = new ReconhecimentoDoRisco()
                                            {
                                                Tragetoria = (ETrajetoria)Enum.Parse(typeof(ETrajetoria), row["Tragetoria"].ToString(), true),
                                                EClasseDoRisco = (EClasseDoRisco)Enum.Parse(typeof(EClasseDoRisco), row["EClasseDoRisco"].ToString(), true),
                                            },
                                            Controles = new List<ControleDeRiscos>()
                                        };

                                        if (!string.IsNullOrEmpty(row["Risco"]?.ToString()))
                                        {
                                            var control = new ControleDeRiscos()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoControle"].ToString()),
                                                TipoDeControle = row["TipoControle"].ToString(),
                                                Link = new Link(),
                                                UKClassificacaoDaMedia = Guid.Parse(row["UKClassificacaoDaMedia"].ToString()),
                                                EControle = (EControle)Enum.Parse(typeof(EControle), row["EControle"].ToString(), true),
                                                ClassificacaoMedida = new ClassificacaoMedida()
                                                {
                                                    Nome = row["NomeClass"].ToString()
                                                }
                                            };

                                            if (!string.IsNullOrEmpty(row["UKLink"].ToString()))
                                            {
                                                control.Link.UniqueKey = Guid.Parse(row["UKLink"].ToString());
                                                control.Link.URL = row["URLLInk"].ToString();
                                            }

                                            risk.Controles.Add(control);
                                        }


                                        per.Riscos.Add(risk);
                                    }

                                    fonte.Perigos.Add(per);
                                }

                                obj.FontesGeradoras.Add(fonte);
                            }
                        }
                    }

                    return View("ReconhecimentoPorWorkArea", obj);
                }

                return View("ReconhecimentoPorWorkArea");
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

                string sql = @"select w.Nome as workarea, f.FonteGeradora, per.Descricao as Perigo, risc.Nome as Risco, r.Tragetoria, r.EClasseDoRisco, tc.Descricao as TipoControle, c.EClassificacaoDaMedia, c.EControle
                               from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
		                                left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and r.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
                               where r.UKWorkArea = '" + ukWorkArea + @"' 
                               order by f.FonteGeradora, per.Descricao, risc.Nome";

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