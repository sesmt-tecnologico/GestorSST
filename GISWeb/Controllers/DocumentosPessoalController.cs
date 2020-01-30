using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using System.Collections.Generic;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocumentosPessoalController : BaseController
    {

        #region Inject

        [Inject]
        public IDocumentosPessoalBusiness DocumentosPessoalBusiness { get; set; }

        [Inject]
        public IREL_DocomumentoPessoalAtividadeBusiness REL_DocomumentoPessoalAtividadeBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {

            ViewBag.Documentos = DocumentosPessoalBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderByDescending(p => p.ApartirDe);


            


            List<DocumentosPessoal> total = new List<DocumentosPessoal>();

            foreach (var item in ViewBag.Documentos)
            {
                if (item != null)
                {
                    total.Add(item);
                }
            }

            ViewBag.Conta = total.Count();


            return View();
          

            
            
            // return View(ViewBag.Documentos = DocumentosPessoalBusiness.Consulta.ToList().OrderByDescending(p=>p.ApartirDe));
        }

        public ActionResult Novo()
        {
                       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(DocumentosPessoal oDocumento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    oDocumento.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    DocumentosPessoalBusiness.Inserir(oDocumento);

                    Extensions.GravaCookie("MensagemSucesso", "O documento  '" + oDocumento.NomeDocumento + "' foi salvo com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "DocumentosPessoal") } });
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
            var ID_Documento = Guid.Parse(id);
            return View(DocumentosPessoalBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID_Documento)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(DocumentosPessoal oDocumentosPessoal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    oDocumentosPessoal.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    DocumentosPessoalBusiness.Alterar(oDocumentosPessoal);

                    Extensions.GravaCookie("MensagemSucesso", "O documento  '"+ oDocumentosPessoal.NomeDocumento+ "' foi atualizado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "DocumentosPessoal") } });
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
        public ActionResult VincularDocumento(string UKAtividade)
        {

            ViewBag.UKAtividade = UKAtividade;

            return PartialView("_VincularDocs");
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularDocumentoAtividade(string UKAtividade, string UkDoc)
        {
            
            try
            {
                Guid UK_Atividade = Guid.Parse(UKAtividade);

                if (string.IsNullOrEmpty(UkDoc))
                    throw new Exception("Não foi possível localizar a função.");

                if (string.IsNullOrEmpty(UKAtividade))
                    throw new Exception("Nenhuma  atividade recebida como parâmetro para vincular a função.");

                
                if (UkDoc.Contains(","))
                {
                    foreach (string ativ in UkDoc.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_DocomumentoPessoalAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumentoPessoal.Equals(pTemp.UniqueKey) && a.UKAtividade.Equals(UK_Atividade)).Count() == 0)
                                {
                                    REL_DocomumentoPessoalAtividadeBusiness.Inserir(new REL_DocomumentoPessoalAtividade()
                                    {
                                         UKAtividade = UK_Atividade,
                                         UKDocumentoPessoal = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(UkDoc.Trim()));

                    if (pTemp != null)
                    {
                        if (REL_DocomumentoPessoalAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumentoPessoal.Equals(pTemp.UniqueKey) && a.UKAtividade.Equals(UK_Atividade)).Count() == 0)
                        {
                            REL_DocomumentoPessoalAtividadeBusiness.Inserir(new REL_DocomumentoPessoalAtividade()
                            {
                                UKAtividade = UK_Atividade,
                                UKDocumentoPessoal = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Documento vinculado a Atividade com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }




        public ActionResult BuscarDocumentoForAutoComplete(string key)
        {
            try
            {
                List<string> documentoAsString = new List<string>();
                List<DocumentosPessoal> lista = DocumentosPessoalBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (DocumentosPessoal com in lista)
                    documentoAsString.Add(com.NomeDocumento);

                return Json(new { Result = documentoAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }


        [RestritoAAjax]
        public ActionResult ConfirmarDocumentoForAutoComplete(string key)
        {
            try
            {
                DocumentosPessoal item = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => a.NomeDocumento.ToUpper().Equals(key.ToUpper()));

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
        public ActionResult TerminarComRedirect(string ID, string NomeDocumento)
        {
            var ID_Documento = Guid.Parse(ID);

            try
            {
                DocumentosPessoal oDocumentosPessoal = DocumentosPessoalBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID_Documento));
                if (oDocumentosPessoal == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Documento." } });
                }
                else
                {
                    oDocumentosPessoal.DataExclusao = DateTime.Now;
                    oDocumentosPessoal.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    DocumentosPessoalBusiness.Alterar(oDocumentosPessoal);

                    Extensions.GravaCookie("MensagemSucesso", "O documento foi removido com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "DocumentosPessoal") } });
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

    }
}