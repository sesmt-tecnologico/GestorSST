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
            //return View(ViewBag.Documentos = DocumentosPessoalBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList());
            return View(ViewBag.Documentos = DocumentosPessoalBusiness.Consulta.ToList().OrderByDescending(p=>p.ApartirDe));
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