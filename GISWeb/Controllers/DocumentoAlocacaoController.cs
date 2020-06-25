using GISCore.Business.Abstract;
using GISModel.DTO.DocumentosAlocacao;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Filters;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocumentoAlocacaoController : BaseController
    {
        #region

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public IDocumentosPessoalBusiness DocumentosPessoalBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }
        
        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_FuncaoAtividade> REL_FuncaoAtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DocomumentoPessoalAtividade> REL_DocomumentoPessoalAtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DocumentosAlocados> REL_DocumentosAlocadoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: DocumentoAlocacao
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Novo(string UKAlocacao)
           {

            Guid Alock = Guid.Parse(UKAlocacao);

            ViewBag.Aloc = Alock;

            List<Alocacao> lista = new List<Alocacao>();


            var sql = @"select al.UniqueKey as UKal, fa.UniqueKey as UKfa, d.UniqueKey as UKd, d.NomeDocumento as Nome,
                        da.UniqueKey as UKda
                        from tbAlocacao al
                        join REL_FuncaoAtividade fa
                        on al.UKFuncao = fa.UKFuncao
                        join REL_DocumentoPessoalAtividade da
                        on fa.UKAtividade = da.UKAtividade
                        join tbDocumentosPessoal d
                        on da.UKDocumentoPessoal = d.UniqueKey and d.DataExclusao = '9999-12-31 23:59:59.997'
                        where al.UniqueKey = '" + Alock + @"'
                        order by al.UniqueKey ";

            DataTable result = DocumentosPessoalBusiness.GetDataTable(sql);

            if (result.Rows.Count > 0)
            {

                Alocacao obj = null;
                DocumentosPessoal oDoc = null;

                foreach (DataRow row in result.Rows)
                {

                    if (obj == null)
                    {
                        obj = new Alocacao()
                        {
                            UniqueKey = Guid.Parse(row["UKal"].ToString()),
                            DocumentosPessoal = new List<DocumentosPessoal>()
                        };

                        if (!string.IsNullOrEmpty(row["UKd"].ToString()))
                        {
                            oDoc = new DocumentosPessoal()
                            {
                                UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                NomeDocumento = row["nome"].ToString(),

                            };


                            obj.DocumentosPessoal.Add(oDoc);
                        }
                    }
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UKal"].ToString())))
                    {
                        if (!string.IsNullOrEmpty(row["UKda"].ToString()))
                        {
                            if (oDoc == null)
                            {
                                oDoc = new DocumentosPessoal()
                                {
                                    UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                    NomeDocumento = row["nome"].ToString(),

                                };

                                obj.DocumentosPessoal.Add(oDoc);
                            }


                            else
                            {
                                oDoc = new DocumentosPessoal()
                                {
                                    UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                    NomeDocumento = row["nome"].ToString(),

                                };

                                obj.DocumentosPessoal.Add(oDoc);
                            }


                        }
                    }
                    else
                    {
                        lista.Add(obj);

                        obj = new Alocacao()
                        {
                            UniqueKey = Guid.Parse(row["UKal"].ToString()),
                            DocumentosPessoal = new List<DocumentosPessoal>()
                        };

                        if (!string.IsNullOrEmpty(row["UKd"].ToString()))
                        {
                            oDoc = new DocumentosPessoal()
                            {
                                UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                NomeDocumento = row["nome"].ToString(),

                            };


                            obj.DocumentosPessoal.Add(oDoc);
                        }
                    }
                }

                if (obj != null)
                    lista.Add(obj);
            }




            return View(lista);


        }

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(string UKAlocacao, string UKDoc)
        {
            var documento = UKDoc;
             
            try
            {
                Guid UK_Alocacao = Guid.Parse(UKAlocacao);

                if (string.IsNullOrEmpty(UKAlocacao))
                    throw new Exception("Não foi possível localizar a Alocação.");

                if (string.IsNullOrEmpty(documento))
                    throw new Exception("Nenhum Documento para vincular.");


                if (documento.Contains(","))
                {
                     documento = documento.Remove(documento.Length - 1);

                    foreach (string ativ in documento.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumento.Equals(pTemp.UniqueKey) && a.UKAlocacao.Equals(UK_Alocacao)).Count() == 0)
                                {
                                    REL_DocumentosAlocadoBusiness.Inserir(new REL_DocumentosAlocados()
                                    {
                                        Posicao = 0,
                                        UKAlocacao = UK_Alocacao,
                                        UKDocumento = pTemp.UniqueKey,
                                        DataDocumento = DateTime.MaxValue,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    }) ;
                                    
                                }
                                else
                                {
                                    return Json(new { resultado = new RetornoJSON() { Erro = "Este documento já está cadastrado para esta alocação!." } });
                                }
                            }
                        }
                    }
                }
                else
                {
                    DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(documento.Trim()));
                    if (pTemp != null)
                    {
                        if (REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumento.Equals(pTemp.UniqueKey) && a.UKAlocacao.Equals(UK_Alocacao)).Count() == 0)
                        {
                            REL_DocumentosAlocadoBusiness.Inserir(new REL_DocumentosAlocados()
                            {
                                UKAlocacao = UK_Alocacao,
                                UKDocumento = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                

                Extensions.GravaCookie("MensagemSucesso", "Os documentos foram alocados com sucesso!", 10);

               //return RedirectToAction("ConfirmaData", "DocumentoAlocacao", UK_Alocacao) ;

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ConfirmaData", "DocumentoAlocacao", UK_Alocacao) } });

            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }

        }


        public ActionResult ConfirmaData(string UK_Alocacao)
       {

            ViewBag.aloc = UK_Alocacao;

            var docs = from dal in REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                       join d in DocumentosPessoalBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                       on dal.UKDocumento equals d.UniqueKey
                       where dal.UKAlocacao.Equals(UK_Alocacao) || d.Validade != 0
                       select new DocumentosAlocacaoViewModel()
                       {
                           Nomedocumento = d.NomeDocumento,
                           UKDocumento = dal.UKDocumento,
                           UniqueKeyREL = dal.UniqueKey,
                           Data = dal.DataDocumento
                       };

            List<DocumentosAlocacaoViewModel> lista = docs.ToList();


            return View(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarData(string UKdate,string Unique, string UKDoc)
        {
            Guid UKdocs = Guid.Parse(UKDoc);
            Guid Uniques = Guid.Parse(Unique);
           

            REL_DocumentosAlocados oTemp = REL_DocumentosAlocadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(Uniques) && a.UKDocumento.Equals(UKdocs));
            if (ModelState.IsValid)
            {
                try
                {

                    oTemp.UKDocumento = UKdocs;
                    oTemp.DataDocumento =Convert.ToDateTime(UKdate);

                    REL_DocumentosAlocadoBusiness.Alterar(oTemp);

                    Extensions.GravaCookie("MensagemSucesso", "As datas foram atualizado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ConfirmaData", "DocumentoAlocacao") } });
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

        public ActionResult Editar(string UK)
        {
            Guid UKrel = Guid.Parse(UK);
           var documento = REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKrel));
            ViewBag.uk = UK;
            ViewBag.UKDocalocado = documento;


            return View("_EditarData");
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarData(string UK, string data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid UKp = Guid.Parse(UK);

                    var documento = REL_DocumentosAlocadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKp));

                    documento.DataDocumento = Convert.ToDateTime(data);

                    REL_DocumentosAlocadoBusiness.Alterar(documento);

                    TempData["MensagemSucesso"] = "Data alterada para: '" + documento.DataDocumento + "' com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Ged") } });
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
        public ActionResult DesalocarDocs(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Não foi possível localizar a identificação da alocação para prosseguir com a operação.");

                Guid UKAlocacao = Guid.Parse(id);

                var al = (from r in REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKAlocacao.Equals(UKAlocacao)).ToList()
                         select new REL_DocumentosAlocados()
                         {
                             ID = r.ID,
                             UKAlocacao = r.UKAlocacao,

                         }).ToList();

                List<REL_DocumentosAlocados> REL = new List<REL_DocumentosAlocados>();

                REL = al;


                if (REL == null)
                    throw new Exception("Não foi possível encontrar os Documentos na base de dados.");

                //Admissao ad = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(al.UKAdmissao));
                //if (ad == null)
                //    throw new Exception("Não foi possível encontrar a admissão onde o empregado está alocado na base de dados.");
                foreach(var item in REL)
                {
                    if (item.UKAlocacao.Equals(UKAlocacao) && item.UsuarioExclusao == null)
                        {

                        REL_DocumentosAlocados docs = REL_DocumentosAlocadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKAlocacao.Equals(UKAlocacao));
                        if (docs == null)
                            throw new Exception("Não foi possível encontrar a alocação na base de dados.");

                        docs.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        
                        REL_DocumentosAlocadoBusiness.Terminar(docs);
                       
                    }

                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Documentos desalocados!" } });


                //Extensions.GravaCookie("MensagemSucesso", "Docuemntos desalocado com sucesso.", 10);


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




    }
}