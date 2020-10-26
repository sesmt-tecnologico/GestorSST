using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.DTO.Admissao;
using GISModel.DTO.AtividadeGeradoraRisco;
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
using System.Web.SessionState;


namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AtividadeGeradoraRiscoController : BaseController
    {
        #region
        [Inject]        
        public IBaseBusiness<Atividade> AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Perigo> PerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Risco> RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeControle> TipoDeControleBusiness { get; set; }

        [Inject]
        public IBaseBusiness<ControleDeRiscos> ControleDeRiscosBusiness { get; set; }


        [Inject]
        public IBaseBusiness<ReconhecimentoDoRisco> ReconhecimentoBusiness { get; set; }


        [Inject]
        public IBaseBusiness<Alocacao> AlocacaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Arquivo> ArquivoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        #endregion


        public ActionResult Index()
        {
            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }
        
        public ActionResult PesquisaAtividade(string UniqueKey)
        {

            string query01 = @"select  a.Uniquekey as ukAtiv, a.Descricao as NomeAtividade, p.Uniquekey as ukPer, p.Descricao as DescPerigo, r.UniqueKey as ukrisc,
                r.Nome as NomeRisco,  pd.Uniquekey as UKdanos, pd.DescricaoDanos,re.UniqueKey as UKReconhecimento, re.UKRisco as rUKrisco                
                from tbAtividade a
				left join REL_AtividadePerigo rel02
                on a.Uniquekey = rel02.UKAtividade and a.DataExclusao = '9999-12-31 23:59:59.997'
                left join tbPerigo p
                on rel02.UKPerigo = p.Uniquekey and p.DataExclusao = '9999-12-31 23:59:59.997'
                left join REL_PerigoRisco rel03
                on p.Uniquekey = rel03.UKPerigo and p.DataExclusao = '9999-12-31 23:59:59.997'
                left join tbRisco r
                on rel03.UKRisco = r.Uniquekey and p.DataExclusao = '9999-12-31 23:59:59.997'
                left join REL_RiscoDanosASaude rel04
                on rel03.UKRisco = rel04.UKRiscos 
                left join tbPossiveisDanos pd
                on rel04.UKDanosSaude = pd.Uniquekey and pd.DataExclusao = '9999-12-31 23:59:59.997'
                left join tbReconhecimentoDoRisco re 
                on r.UniqueKey = re.UKRisco and re.UKAtividade = '" + UniqueKey + @"' and re.DataExclusao = '9999-12-31 23:59:59.997' 
                where a.Uniquekey =   '" + UniqueKey + @"'
                order by p.Uniquekey";


            VMAtividadesRiscos obj1 = null;
            Atividade atividade = null;
            Perigo perigo = null;
            Risco risc = null;
            PossiveisDanos danos = null;
            List<VMAtividadesRiscos> ListRiscos = new List<VMAtividadesRiscos>();

            DataTable result01 = AlocacaoBusiness.GetDataTable(query01);
            if (result01.Rows.Count > 0)
            {
                foreach (DataRow row in result01.Rows)
                {
                    if (obj1 == null)
                    {

                        obj1 = new VMAtividadesRiscos()
                        {
                            
                            UKAtividade = row["ukAtiv"].ToString(),
                            UKPerigo = row["ukPer"].ToString(),
                            UKRisco = row["ukrisc"].ToString(),
                            UKDanos = row["UKdanos"].ToString(),
                            NomeAtividade = new List<Atividade>(),
                           

                        };
                        if (!string.IsNullOrEmpty(row["ukAtiv"]?.ToString()))
                        {
                            atividade = new Atividade()
                            {
                                UniqueKey = Guid.Parse(row["ukAtiv"].ToString()),
                                Descricao = row["NomeAtividade"].ToString(),
                                Perigos = new List<Perigo>(),
                            };

                            if (!string.IsNullOrEmpty(row["ukPer"]?.ToString()))
                            {
                                perigo = new Perigo()
                                {
                                    UniqueKey = Guid.Parse(row["ukPer"].ToString()),
                                    Descricao = row["DescPerigo"].ToString(),
                                    Riscos = new List<Risco>()
                                };

                                atividade.Perigos.Add(perigo);
                            }

                            if (!string.IsNullOrEmpty(row["ukrisc"]?.ToString()))
                            {
                                risc = new Risco()
                                {
                                    UniqueKey = Guid.Parse(row["ukrisc"].ToString()),
                                    Nome = row["NomeRisco"].ToString(),
                                    Danos = new List<PossiveisDanos>(),

                                };

                                if (!string.IsNullOrEmpty(row["UKReconhecimento"].ToString()))
                                {
                                    risc.Reconhecimento = new ReconhecimentoDoRisco()
                                    {
                                        UniqueKey = Guid.Parse(row["UKReconhecimento"].ToString()),
                                        UKRisco = Guid.Parse(row["rUKRisco"].ToString()),
                                        UKAtividade = Guid.Parse(row["ukAtiv"].ToString()),
                                    };
                                }

                                perigo.Riscos.Add(risc);

                            }
                            if (!string.IsNullOrEmpty(row["UKdanos"]?.ToString()))
                            {
                                danos = new PossiveisDanos()
                                {
                                    UniqueKey = Guid.Parse(row["UKdanos"].ToString()),
                                    DescricaoDanos = row["DescricaoDanos"].ToString()
                                };

                                risc.Danos.Add(danos);

                            }

                            

                            obj1.NomeAtividade.Add(atividade);

                            ListRiscos.Add(obj1);
                        }

                    }
                    else
                    {
                        
                            if (obj1.UKAtividade.Equals(row["ukAtiv"].ToString()))
                            {

                                if (obj1.UKPerigo.Equals(row["ukPer"].ToString()))
                                {

                                    if (obj1.UKRisco.Equals(row["ukrisc"].ToString()))
                                    {

                                        if (!string.IsNullOrEmpty(row["UKdanos"]?.ToString()))
                                        {
                                            danos = new PossiveisDanos()
                                            {
                                                DescricaoDanos = row["DescricaoDanos"].ToString()
                                            };

                                            risc.Danos.Add(danos);

                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(row["ukrisc"]?.ToString()))
                                        {
                                            risc = new Risco()
                                            {
                                                UniqueKey = Guid.Parse(row["ukrisc"].ToString()),
                                                Nome = row["NomeRisco"].ToString(),
                                                Danos = new List<PossiveisDanos>()
                                            };

                                        if (!string.IsNullOrEmpty(row["UKReconhecimento"].ToString()))
                                        {
                                            risc.Reconhecimento = new ReconhecimentoDoRisco()
                                            {
                                                UniqueKey = Guid.Parse(row["UKReconhecimento"].ToString()),
                                                UKRisco = Guid.Parse(row["rUKRisco"].ToString()),
                                                UKAtividade = Guid.Parse(row["ukAtiv"].ToString()),
                                            };
                                        }

                                        perigo.Riscos.Add(risc);

                                        }
                                        if (!string.IsNullOrEmpty(row["UKdanos"]?.ToString()))
                                        {
                                            danos = new PossiveisDanos()
                                            {
                                                DescricaoDanos = row["DescricaoDanos"].ToString()
                                            };

                                            risc.Danos.Add(danos);

                                        }

                                    }

                                }
                                else
                                {
                                    obj1 = new VMAtividadesRiscos()
                                    {

                                        UKAtividade = row["ukAtiv"].ToString(),
                                        UKPerigo = row["ukPer"].ToString(),
                                        UKRisco = row["ukrisc"].ToString(),
                                        UKDanos = row["UKdanos"].ToString(),
                                        NomeAtividade = new List<Atividade>(),


                                    };

                                   if (!string.IsNullOrEmpty(row["ukPer"]?.ToString()))
                                    {
                                        perigo = new Perigo()
                                        {
                                            UniqueKey = Guid.Parse(row["ukPer"].ToString()),
                                            Descricao = row["DescPerigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        };

                                        atividade.Perigos.Add(perigo);
                                    }

                                    if (!string.IsNullOrEmpty(row["ukrisc"]?.ToString()))
                                    {
                                        risc = new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["ukrisc"].ToString()),
                                            Nome = row["NomeRisco"].ToString(),
                                            Danos = new List<PossiveisDanos>()
                                        };
                                    if (!string.IsNullOrEmpty(row["UKReconhecimento"].ToString()))
                                    {
                                        risc.Reconhecimento = new ReconhecimentoDoRisco()
                                        {
                                            UniqueKey = Guid.Parse(row["UKReconhecimento"].ToString()),
                                            UKRisco = Guid.Parse(row["rUKRisco"].ToString()),
                                            UKAtividade = Guid.Parse(row["ukAtiv"].ToString()),
                                        };
                                    }

                                    perigo.Riscos.Add(risc);

                                    }
                                    if (!string.IsNullOrEmpty(row["UKdanos"]?.ToString()))
                                    {
                                        danos = new PossiveisDanos()
                                        {
                                            DescricaoDanos = row["DescricaoDanos"].ToString()
                                        };

                                        risc.Danos.Add(danos);

                                    }

                                ListRiscos.Add(obj1);


                                 }
                           

                               
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(row["ukAtiv"].ToString()))
                                {
                                obj1 = new VMAtividadesRiscos()
                                {
                                    UKAtividade = row["ukAtiv"].ToString(),
                                    UKPerigo = row["ukPer"].ToString(),
                                    UKRisco = row["ukrisc"].ToString(),
                                    UKDanos = row["UKdanos"].ToString(),
                                    NomeAtividade = new List<Atividade>(),

                                };

                                atividade = new Atividade()
                                    {
                                        UniqueKey = Guid.Parse(row["ukAtiv"].ToString()),
                                        Descricao = row["NomeAtividade"].ToString(),
                                        Perigos = new List<Perigo>(),
                                    };

                                    if (!string.IsNullOrEmpty(row["ukPer"].ToString()))
                                    {
                                        perigo = new Perigo()
                                        {
                                            UniqueKey = Guid.Parse(row["ukPer"].ToString()),
                                            Descricao = row["DescPerigo"].ToString(),
                                            Riscos = new List<Risco>()
                                        };

                                        atividade.Perigos.Add(perigo);
                                    }

                                    if (!string.IsNullOrEmpty(row["ukrisc"]?.ToString()))
                                    {
                                        risc = new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["ukrisc"].ToString()),
                                            Nome = row["NomeRisco"].ToString(),
                                            Danos = new List<PossiveisDanos>()
                                        };
                                    if (!string.IsNullOrEmpty(row["UKReconhecimento"].ToString()))
                                    {
                                        risc.Reconhecimento = new ReconhecimentoDoRisco()
                                        {
                                            UniqueKey = Guid.Parse(row["UKReconhecimento"].ToString()),
                                            UKRisco = Guid.Parse(row["rUKRisco"].ToString()),
                                            UKAtividade = Guid.Parse(row["ukAtiv"].ToString()),
                                        };
                                    }

                                    perigo.Riscos.Add(risc);

                                    }
                                    if (!string.IsNullOrEmpty(row["UKdanos"]?.ToString()))
                                    {
                                        danos = new PossiveisDanos()
                                        {
                                            DescricaoDanos = row["DescricaoDanos"].ToString()
                                        };

                                        risc.Danos.Add(danos);

                                    }

                                    obj1.NomeAtividade.Add(atividade);

                                 ListRiscos.Add(obj1);


                                }
                            }

                            // ListRiscos.Add(obj);
                        

                    }

                }

            }

            //ListRiscos.Add(obj1);

            ViewBag.Atividade = ListRiscos;

            return PartialView("_PesquisaAtividade", ListRiscos);
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


        public ActionResult CriarControle(string UKAtividade,  string UKPerigo, string UKRisco)
        {

            ViewBag.UKAtividade = UKAtividade;            
            ViewBag.UKPerigo = UKPerigo;
            ViewBag.UKRisco = UKRisco;

            var UKAtiv = Guid.Parse(UKAtividade);           
            var UKPerig = Guid.Parse(UKPerigo);
            var UKRisc = Guid.Parse(UKRisco);

            var objAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKAtiv));
            if (objAtividade == null)
                throw new Exception("Não foi possível recuperar a Atividade na base de dados. Tente novamente ou acione o administrador do sitema.");

            ViewBag.Atividade = objAtividade.Descricao;



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


            var enumData02 = from EProbabilidadeSeg e in Enum.GetValues(typeof(EProbabilidadeSeg))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.GetDisplayName()
                             };
            ViewBag.EProbabilidadeSeg = new SelectList(enumData02, "ID", "Name");


            var enumData03 = from EGravidade e in Enum.GetValues(typeof(EGravidade))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.GetDisplayName()
                             };
            ViewBag.EGravidade = new SelectList(enumData03, "ID", "Name");


            return PartialView("_CadastrarControleDeRisco");

        }

        [HttpPost]
        [RestritoAAjax]
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
                                
                                p.UKAtividade.Equals(entidade.UKAtividade) &&
                                p.UKPerigo.Equals(entidade.UKPerigo) &&
                                p.UKRisco.Equals(entidade.UKRisco)
                    );

                    if (oReconhecimento == null)
                    {
                        oReconhecimento = new ReconhecimentoDoRisco()
                        {
                            
                            UKAtividade = entidade.UKAtividade,
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





    }
}