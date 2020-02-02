using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
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
    public class AtividadeController : BaseController
    {
        #region Inject

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }


        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public IAtividadeFuncaoLiberadaBusiness AtividadeFuncaoLiberadaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }


        [Inject]
        public IPlanoDeAcaoBusiness PlanoDeAcaoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPerigoPotencialBusiness PerigoPotencialBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoAmbienteBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IExposicaoBusiness ExposicaoBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AtividadePerigo> AtividadePerigoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }




        #endregion
        // GET: TipoDeRisco
        public ActionResult Index()
        {
            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d => d.Descricao).ToList();

            return View();
        }

        public ActionResult AtividadePerigo()
        {


            return View();

        }

        public ActionResult ListaPerigoPorAtividade()
        {
            string sql = @"select a.UniqueKey as UK_Atividade, a.Descricao as nome, p.UniqueKey as UK_Perigo, p.Descricao as NomePerigo, ap.UniqueKey as relap,
                             ap.UKAtividade as rel01, ap.UKPerigo as rel02
                             from tbAtividade a
                             left join REL_AtividadePerigo ap on ap.UKAtividade = a.UniqueKey and a.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2)
                             left join tbPerigo p on p.UniqueKey = ap.UKPerigo and a.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2)
                             order by a.Descricao";


            DataTable result = AtividadeBusiness.GetDataTable(sql);

            List<Atividade> lista = new List<Atividade>();

            if (result.Rows.Count > 0)
            {

                Atividade obj = null;
                Perigo oPerigo = null;

                foreach (DataRow row in result.Rows)
                {

                    if (obj == null)
                    {
                        obj = new Atividade()
                        {
                            UniqueKey = Guid.Parse(row["UK_Atividade"].ToString()),
                            Descricao = row["nome"].ToString(),
                            Perigos = new List<Perigo>()
                        };

                        if (!string.IsNullOrEmpty(row["relap"].ToString()))
                        {

                            oPerigo = new Perigo()
                            {
                                UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                Descricao = row["NomePerigo"].ToString(),
                                

                            };

                            obj.Perigos.Add(oPerigo);

                        }

                    }
                    //se a atividade for a mesma, carregar outro documento
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UK_Atividade"].ToString())))
                    {
                        if (!string.IsNullOrEmpty(row["rel02"].ToString()))
                        {
                            if (oPerigo != null)
                            {
                                oPerigo = new Perigo()
                                {
                                    UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                    Descricao = row["NomePerigo"].ToString(),
                                   

                                };
                            }
                        }

                        obj.Perigos.Add(oPerigo);
                    }

                    else
                    {
                        lista.Add(obj);

                        obj = new Atividade()
                        {
                            UniqueKey = Guid.Parse(row["UK_Atividade"].ToString()),
                            Descricao = row["nome"].ToString(),
                            Perigos = new List<Perigo>()
                        };


                        if (!string.IsNullOrEmpty(row["rel02"].ToString()))
                        {
                            oPerigo = new Perigo()
                            {
                                UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                Descricao = row["NomePerigo"].ToString()
                            };



                            obj.Perigos.Add(oPerigo);
                        }
                    }


                }

                if (obj != null)
                    lista.Add(obj);


            }

            return View("_ListaPerigoPorAtividade", lista);

        }

        //recebe parametro de Funcao/index e listaFuncao para listar atividades relacionadas a função
        public ActionResult ListaAtividadePorFuncao(string IDFuncao, string NomeFuncao)
        {

            ViewBag.Funcao = NomeFuncao;

            //ViewBag.ListaAtividadeFuncao = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(IDFuncao)).OrderBy(p=>p.Descricao).ToList();

            try
            {
                // Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(id));

                if (ViewBag.ListaAtividadeFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens4 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes") });
                }
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


        //parametro id da função, nome da função e id da Diretoria, passados de index/função e ListaFunção
        public ActionResult Novo()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Atividade oAtividade)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    AtividadeBusiness.Inserir(oAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oAtividade.Descricao + "' foi cadastrada com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });

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


        public ActionResult ListaDocumentoPessoal()
        {

            string sql = @"select a.UniqueKey as UK_Ativ, a.Descricao as nome, d.UniqueKey, d.NomeDocumento as NomeD, d.DescricaoDocumento as DescricaoD,
                            da.UKAtividade as rel1, da.UKDocumentoPessoal as rel2
                            from tbAtividade a 
                            left join REL_DocumentoPessoalAtividade da on da.UKAtividade = a.uniqueKey and a.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2) 
                            left join tbDocumentosPessoal d on d.UniqueKey = da.UKDocumentoPessoal  and d.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2)
                            order by nome";


            DataTable result = AtividadeBusiness.GetDataTable(sql);

            List<Atividade> lista = new List<Atividade>();

            if (result.Rows.Count > 0)
            {

                Atividade obj = null;
                DocumentosPessoal oDocumento = null;

                foreach (DataRow row in result.Rows)
                {

                    if (obj == null)
                    {
                        obj = new Atividade()
                        {
                            UniqueKey = Guid.Parse(row["UK_Ativ"].ToString()),
                            Descricao = row["nome"].ToString(),
                            DocumentosPessoal = new List<DocumentosPessoal>()
                        };

                        if (!string.IsNullOrEmpty(row["rel2"].ToString()))
                        {

                            oDocumento = new DocumentosPessoal()
                            {
                                UniqueKey = Guid.Parse(row["rel2"].ToString()),
                                DescricaoDocumento = row["DescricaoD"].ToString(),
                                NomeDocumento = row["NomeD"].ToString(),

                            };

                            obj.DocumentosPessoal.Add(oDocumento);

                        }

                    }
                    //se a atividade for a mesma, carregar outro documento
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UK_Ativ"].ToString())))
                            {
                                if (!string.IsNullOrEmpty(row["rel2"].ToString()))
                                {
                                    if (oDocumento != null)
                                    {
                                        oDocumento = new DocumentosPessoal()
                                        {
                                            UniqueKey = Guid.Parse(row["rel2"].ToString()),
                                            DescricaoDocumento = row["DescricaoD"].ToString(),
                                            NomeDocumento = row["NomeD"].ToString(),

                                        };
                                    }
                                }

                                obj.DocumentosPessoal.Add(oDocumento);
                     }

                        else
                        {
                            lista.Add(obj);

                            obj = new Atividade()
                            {
                                UniqueKey = Guid.Parse(row["UK_Ativ"].ToString()),
                                Descricao = row["nome"].ToString(),
                                DocumentosPessoal = new List<DocumentosPessoal>()
                            };


                            if (!string.IsNullOrEmpty(row["rel2"].ToString()))
                            {
                                oDocumento = new DocumentosPessoal()
                                {
                                    UniqueKey = Guid.Parse(row["rel2"].ToString()),
                                    DescricaoDocumento = row["DescricaoD"].ToString(),
                                    NomeDocumento = row["NomeD"].ToString(),
                                };

                           

                                obj.DocumentosPessoal.Add(oDocumento);
                            }
                        }


                }

                if (obj != null)
                    lista.Add(obj);

               
            }

            return View("_ListaDocumento", lista);

        }


        public ActionResult BuscarDetalhesDeMedidasDeControleAtividadeFuncao(string idTipoRisco, string idAtividade)
        {

            List<TipoDeRisco> Riscos = (from Tip in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join ATE in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idAtividade equals ATE.ID
                                        join PD in PossiveisDanosBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPossiveisDanos equals PD.ID
                                        join PP in PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idPerigoPotencial equals PP.ID
                                        join EP in EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        on Tip.idEventoPerigoso equals EP.ID
                                        where Tip.ID.Equals(idTipoRisco) && ATE.ID.Equals(idAtividade)
                                        select new TipoDeRisco()
                                        {
                                            ID = Tip.ID,
                                            EClasseDoRisco = Tip.EClasseDoRisco,
                                            FonteGeradora = Tip.FonteGeradora,
                                            Tragetoria = Tip.Tragetoria,
                                            PossiveisDanos = new PossiveisDanos()
                                            {
                                                DescricaoDanos = PD.DescricaoDanos,

                                            },
                                            PerigoPotencial = new PerigoPotencial()
                                            {
                                                DescricaoEvento = PP.DescricaoEvento,
                                            },
                                            EventoPerigoso = new EventoPerigoso()
                                            {
                                                Descricao = EP.Descricao,
                                            },
                                        }



                                        ).ToList();

            ViewBag.DescricaoRiscos = Riscos;


            var Lista = (from MC in MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                         join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                         on MC.IDTipoDeRisco equals TR.ID
                         where TR.ID.Equals(idTipoRisco)
                         group TR by TR.ID into g
                         select new
                         {
                             IDTipo = g.Key
                         }
                        ).ToList();


            List<Guid> Filtro = new List<Guid>();

            foreach (var lista in Lista)
            {

                Filtro.Add(lista.IDTipo);
            }

            List<MedidasDeControleExistentes> total = MedidasDeControleBusiness.Consulta.Where(p => Filtro.Contains(p.IDTipoDeRisco)).ToList();

            ViewBag.Total = total.Count();



            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(idTipoRisco) && (p.TipoDeRisco.idAtividade.Equals(idAtividade)))).ToList();
            try
            {
                MedidasDeControleExistentes oMedidasDeControleExistentes = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(idTipoRisco));


                if (oMedidasDeControleExistentes == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Medidas de controle não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_ControleRiscoFuncao", oMedidasDeControleExistentes) });
                }
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
        

        public ActionResult Edicao(string id, string Uk)
        {
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            Guid ID_Ativ = Guid.Parse(id);
            Guid UK_Ativ = Guid.Parse(Uk);

            ViewBag.Atividades = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UK_Ativ))).ToList();

            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UK_Ativ))));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Atividade pAtividade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    pAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    AtividadeBusiness.Alterar(pAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + pAtividade.Descricao + "' foi atualizada com sucesso.", 10);

                    //return RedirectToAction("Index");

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });
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
        

        public ActionResult Excluir(string id)
        {
            //ViewBag.Cargo = new SelectList(CargoBusiness.Consulta.ToList(), "IDCargo", "NomeDoCargo");
            return View(AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularPerigoAtividade(string UKAtiv)
        {

            ViewBag.UKAtividade = UKAtiv;

            return PartialView("_VincularPerigoAtividade");
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularPerigo(string UKAtividade, string UKPerigo)
        {

            try
            {
                //Guid UK_Perigo = Guid.Parse(UKPerigo);
                Guid UK_Atividade = Guid.Parse(UKAtividade);
                if (string.IsNullOrEmpty(UKAtividade))
                    throw new Exception("Não foi possível localizar a Atividade.");

                if (string.IsNullOrEmpty(UKPerigo))
                    throw new Exception("Nenhum Perigo para vincular.");


                if (UKPerigo.Contains(","))
                {
                    foreach (string ativ in UKPerigo.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            Perigo pTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (AtividadePerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKPerigo.Equals(pTemp.UniqueKey) && a.UKAtividade.Equals(UK_Atividade)).Count() == 0)
                                {
                                    AtividadePerigoBusiness.Inserir(new REL_AtividadePerigo()
                                    {
                                        UKAtividade = UK_Atividade,
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
                    Perigo pTemp = PerigoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(UKPerigo));

                    if (pTemp != null)
                    {
                        if (AtividadePerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKPerigo.Equals(pTemp.UniqueKey) && a.UKAtividade.Equals(UK_Atividade)).Count() == 0)
                        {
                            AtividadePerigoBusiness.Inserir(new REL_AtividadePerigo()
                            {
                                UKAtividade = UK_Atividade,
                                UKPerigo = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Perigo vinculado a Atividade com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }



        [HttpPost]
        public ActionResult TerminarComRedirect(string ID, string Descricao)
        {
            var ID_Atividade = Guid.Parse(ID);

            try
            {
                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID_Atividade));
                if (oAtividade == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Documento." } });
                }
                else
                {
                    oAtividade.DataExclusao = DateTime.Now;
                    oAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    AtividadeBusiness.Excluir(oAtividade);

                    Extensions.GravaCookie("MensagemSucesso", "Atividade '"+ oAtividade.Descricao +"' foi removida com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Atividade") } });
                }
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

        [RestritoAAjax]
        public ActionResult BuscarAtividadeForAutoComplete(string key)
        {
            try
            {
                List<string> atividadeAsString = new List<string>();
                List<Atividade> lista = AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Atividade com in lista)
                    atividadeAsString.Add(com.Descricao);

                return Json(new { Result = atividadeAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarAtividadeForAutoComplete(string key)
        {
            try
            {
                Atividade item = AtividadeBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }

        [RestritoAAjax]
        public ActionResult BuscarRiscoForAutoComplete(string key)
        {
            try
           {
                List<string> perigoAsString = new List<string>();
                List<Perigo> lista = PerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Perigo com in lista)
                    perigoAsString.Add(com.Descricao);

                return Json(new { Result = perigoAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarRiscoForAutoComplete(string key)
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


        [RestritoAAjax]
        public ActionResult _Upload()
        {
            try
            {
                return PartialView("_Upload");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }
    }
    
}
