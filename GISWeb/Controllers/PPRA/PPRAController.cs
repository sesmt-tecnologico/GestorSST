using GISCore.Business.Abstract;
using GISModel.DTO.GerenciamentoDoRisco;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.PPRA;
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
using System.Web.SessionState;

namespace GISWeb.Controllers.PPRA
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PPRAController : Controller
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
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<ClassificacaoMedida> ClassificacaoMedidaBusiness { get; set; }


        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Link> LinkBusiness { get; set; }

        [Inject]
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion






        // GET: PPRA
        public ActionResult Index()
        {
            return View();
        }
   



        public ActionResult PPRAReconhecimentoWA()

        {

            try
            {
                //      string sql = @"select w.UniqueKey as UKWorkArea, w.Nome as workarea, 
                //                            f.UniqueKey as UKFonte, f.FonteGeradora, 
                //                            per.Descricao as Perigo, 
                //                            risc.UniqueKey as UKRisco, risc.Nome as Risco, 
                //                            r.Tragetoria, r.EClasseDoRisco, 
                //                            tc.UniqueKey as UKTipoControle, tc.Descricao as TipoControle, 
                //                            c.UKClassificacaoDaMedia,  c.EControle, 
                //                            cm.UniqueKey as UKcm, cm.Nome as NomeClass,
                //                            lk.UniqueKey as UKLink, lk.URL as URLLInk
                //                     from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
                //                        left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
                //                        left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
                //                        left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
                //                        left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
                //                        left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and c.DataExclusao ='9999-12-31 23:59:59.997' 
                //                        left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
                //left join ClassificacaoMedidas cm on cm.UniqueKey = c.UKClassificacaoDaMedia and cm.DataExclusao ='9999-12-31 23:59:59.997'
                //                              left join tbLink lk on lk.UniqueKey = c.UKLink and lk.DataExclusao ='9999-12-31 23:59:59.997'                             
                //                    order by f.FonteGeradora, per.Descricao, risc.Nome";

                string sql = @" select w.UniqueKey as UKWorkArea, w.Nome as workarea, 
                                      f.UniqueKey as UKFonte, f.FonteGeradora, 
                                      per.Descricao as Perigo, 
                                      risc.UniqueKey as UKRisco, risc.Nome as Risco, 
                                      r.Tragetoria, r.EClasseDoRisco, 
                                      tc.UniqueKey as UKTipoControle, tc.Descricao as TipoControle, 
                                      c.UKClassificacaoDaMedia,  c.EControle, 
                                      cm.UniqueKey as UKcm, cm.Nome as NomeClass,
                                      lk.UniqueKey as UKLink, lk.URL as URLLInk,ex.EExposicaoInsalubre as ExpoInsalubre,ex.EExposicaoCalor as ExpoCalor,ex.Observacao as exObs,
									  ex.EExposicaoSeg as ExpoSeguranca,ex.EProbabilidadeSeg as Probabilidade,ex.EGravidade as Gravidade, ex.UniqueKey as exUK,
									  tm.UniqueKey as UKtm,tm.TipoMedicoes as Tipo, tm.ValorMedicao as Valor,tm.MaxExpDiaria as MaxExpoDiaria, tm.Observacoes as Obs, tm.UKExposicao as UKExpo
                               from [dbGestor].[dbo].[tbReconhecimentoDoRisco] r
		                                left join [dbGestor].[dbo].[tbWorkArea]  w on w.UniqueKey = r.UKWorkArea and w.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbFonteGeradoraDeRisco] f on f.UniqueKey = r.UKFonteGeradora and f.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbPerigo] per on per.UniqueKey = r.UKPerigo and per.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbRisco]  risc on risc.UniqueKey = r.UKRisco and risc.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbControleDoRisco]  c on c.UKReconhecimentoDoRisco = r.UniqueKey and c.DataExclusao ='9999-12-31 23:59:59.997' 
		                                left join [dbGestor].[dbo].[tbTipoDeControle]  tc on tc.UniqueKey = c.UKTipoDeControle and tc.DataExclusao ='9999-12-31 23:59:59.997' 
										left join ClassificacaoMedidas cm on cm.UniqueKey = c.UKClassificacaoDaMedia and cm.DataExclusao ='9999-12-31 23:59:59.997'
                                        left join tbLink lk on lk.UniqueKey = c.UKLink and lk.DataExclusao ='9999-12-31 23:59:59.997' 
										left join tbExposicao ex on   ex.UKRisco = risc.UniqueKey and ex.DataExclusao = '9999-12-31 23:59:59.997'
										left join tbMedicoes tm on ex.UniqueKey = tm.UKExposicao and tm.DataExclusao = '9999-12-31 23:59:59.997'
                                        where r.EClasseDoRisco = '2'
										                           
                              order by f.FonteGeradora, per.Descricao, risc.Nome";






                DataTable result = ReconhecimentoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    VMReconhecimento obj = null;
                    FonteGeradoraDeRisco fonte = null;
                    Perigo per = null;
                    Risco risk = null;
                    Exposicao exp = null;
                    Medicoes med = null;

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
                                                EClasseDoRisco = (EClasseDoRisco)Enum.Parse(typeof(EClasseDoRisco), row["EClasseDoRisco"].ToString(), true),
                                            },
                                            Exposicao = new List<Exposicao>(),
                                            Controles = new List<ControleDeRiscos>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoControle"]?.ToString()))
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
                                        

                                        if(!string.IsNullOrEmpty(row["exUK"]?.ToString()))
                                        {
                                            exp = new Exposicao()
                                            {
                                                UniqueKey = Guid.Parse(row["exUK"].ToString()),
                                                EExposicaoInsalubre = (EExposicaoInsalubre)Enum.Parse(typeof(EExposicaoInsalubre), row["ExpoSeguranca"].ToString(), true),
                                                EExposicaoCalor = (EExposicaoCalor)Enum.Parse(typeof(EExposicaoCalor), row["ExpoCalor"].ToString(), true),
                                                Observacao = row["exObs"].ToString(),
                                                Medicao = new List<Medicoes>()
                                            };
                                       

                                           if (!string.IsNullOrEmpty(row["UKExpo"]?.ToString()))
                                            {
                                               med = new Medicoes()
                                                {
                                                    UniqueKey = Guid.Parse(row["UKExpo"].ToString()),
                                                    TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), row["Tipo"].ToString(), true),
                                                    ValorMedicao = row["Valor"].ToString(),
                                                   MaxExpDiaria = row["MaxExpoDiaria"].ToString()


                                               };

                                            exp.Medicao.Add(med);

                                           }

                                              risk.Exposicao.Add(exp);
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
                                        if (!string.IsNullOrEmpty(row["exUK"]?.ToString()))
                                        {
                                            exp = new Exposicao()
                                            {
                                                UniqueKey = Guid.Parse(row["exUK"].ToString()),
                                                EExposicaoInsalubre = (EExposicaoInsalubre)Enum.Parse(typeof(EExposicaoInsalubre), row["ExpoSeguranca"].ToString(), true),
                                                EExposicaoCalor = (EExposicaoCalor)Enum.Parse(typeof(EExposicaoCalor), row["ExpoCalor"].ToString(), true),
                                                Observacao = row["exObs"].ToString(),
                                                Medicao = new List<Medicoes>()
                                            };

                                            risk.Exposicao.Add(exp);
                                       
                                       
                                            if (!string.IsNullOrEmpty(row["UKExpo"]?.ToString()))
                                            {
                                                med = new Medicoes()
                                                {
                                                    UniqueKey = Guid.Parse(row["UKExpo"].ToString()),
                                                    TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), row["Tipo"].ToString(), true),
                                                    ValorMedicao = row["Valor"].ToString(),
                                                    MaxExpDiaria = row["MaxExpoDiaria"].ToString()


                                                };

                                                exp.Medicao.Add(med);

                                            }

                                        }



                                    }
                                    else
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
                                        if (!string.IsNullOrEmpty(row["exUK"]?.ToString()))
                                        {
                                            exp = new Exposicao()
                                            {
                                                UniqueKey = Guid.Parse(row["exUK"].ToString()),
                                                EExposicaoInsalubre = (EExposicaoInsalubre)Enum.Parse(typeof(EExposicaoInsalubre), row["ExpoSeguranca"].ToString(), true),
                                                EExposicaoCalor = (EExposicaoCalor)Enum.Parse(typeof(EExposicaoCalor), row["ExpoCalor"].ToString(), true),
                                                Observacao = row["exObs"].ToString(),
                                                Medicao = new List<Medicoes>()
                                            };

                                            risk.Exposicao.Add(exp);
                                       
                                            if (!string.IsNullOrEmpty(row["UKExpo"]?.ToString()))
                                            {
                                                med = new Medicoes()
                                                {
                                                    UniqueKey = Guid.Parse(row["UKExpo"].ToString()),
                                                    TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), row["Tipo"].ToString(), true),
                                                    ValorMedicao = row["Valor"].ToString(),
                                                    MaxExpDiaria = row["MaxExpoDiaria"].ToString()


                                                };

                                                exp.Medicao.Add(med);

                                            }

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
                                            Exposicao = new List<Exposicao>(),
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
                                        if (!string.IsNullOrEmpty(row["exUK"]?.ToString()))
                                        {
                                            exp = new Exposicao()
                                            {
                                                UniqueKey = Guid.Parse(row["exUK"].ToString()),
                                                EExposicaoInsalubre = (EExposicaoInsalubre)Enum.Parse(typeof(EExposicaoInsalubre), row["ExpoSeguranca"].ToString(), true),
                                                EExposicaoCalor = (EExposicaoCalor)Enum.Parse(typeof(EExposicaoCalor), row["ExpoCalor"].ToString(), true),
                                                Observacao = row["exObs"].ToString(),
                                                Medicao = new List<Medicoes>()
                                            };

                                            risk.Exposicao.Add(exp);
                                        

                                                if (!string.IsNullOrEmpty(row["UKExpo"]?.ToString()))
                                                        {
                                                            med = new Medicoes()
                                                            {
                                                                UniqueKey = Guid.Parse(row["UKExpo"].ToString()),
                                                                TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), row["Tipo"].ToString(), true),
                                                                ValorMedicao = row["Valor"].ToString(),
                                                                MaxExpDiaria = row["MaxExpoDiaria"].ToString()


                                                            };

                                                            exp.Medicao.Add(med);

                                                }
                                        }



                                    }

                                        per.Riscos.Add(risk);
                                 }

                                    fonte.Perigos.Add(per);

                              
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
                                            Exposicao = new List<Exposicao>(),
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
                                        if (!string.IsNullOrEmpty(row["exUK"]?.ToString()))
                                        {
                                            exp = new Exposicao()
                                            {
                                                UniqueKey = Guid.Parse(row["exUK"].ToString()),
                                                EExposicaoInsalubre = (EExposicaoInsalubre)Enum.Parse(typeof(EExposicaoInsalubre), row["ExpoSeguranca"].ToString(), true),
                                                EExposicaoCalor = (EExposicaoCalor)Enum.Parse(typeof(EExposicaoCalor), row["ExpoCalor"].ToString(), true),
                                                Observacao = row["exObs"].ToString(),
                                                Medicao = new List<Medicoes>()
                                            };

                                            risk.Exposicao.Add(exp);
                                        
                                    

                                            if (!string.IsNullOrEmpty(row["UKExpo"]?.ToString()))
                                                    {
                                                        med = new Medicoes()
                                                        {
                                                            UniqueKey = Guid.Parse(row["UKExpo"].ToString()),
                                                            TipoMedicoes = (ETipoMedicoes)Enum.Parse(typeof(ETipoMedicoes), row["Tipo"].ToString(), true),
                                                            ValorMedicao = row["Valor"].ToString(),
                                                            MaxExpDiaria = row["MaxExpoDiaria"].ToString()


                                                        };

                                                        exp.Medicao.Add(med);
                                                
                                            }
                                        }

                                        per.Riscos.Add(risk);
                                        fonte.Perigos.Add(per);
                                    }
                                }

                                obj.FontesGeradoras.Add(fonte);
                            }
                        }
                    }

                    return View("PPRAReconhecimentoWA", obj);
                }

                return View("PPRAReconhecimentoWA");
            }


            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }

           
        }



    }


}