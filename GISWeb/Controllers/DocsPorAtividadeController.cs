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

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocsPorAtividadeController : BaseController
    {

        #region Inject

        [Inject]
        public IDocsPorAtividadeBusiness DocsPorAtividadeBusiness { get; set; }

        [Inject]
        public IDocumentosPessoalBusiness DocumentosPessoalBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IFuncCargoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }
        
        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {

            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo(string id)
        {


            ViewBag.Documentos = new SelectList(DocumentosPessoalBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDocumentosEmpregado", "NomeDocumento");
           
            ViewBag.IDAtividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)&&(d.ID.Equals(id))).ToList(); ;
            ViewBag.idAtiv = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(DocsPorAtividade oDocAtividade, string idAtiv)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    oDocAtividade.idAtividade = Guid.Parse(idAtiv);
                    DocsPorAtividadeBusiness.Inserir(oDocAtividade);

                    TempData["MensagemSucesso"] = "O Documento foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "DocsPorAtividade") } });
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

            return View(DocumentosPessoalBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(DocumentosPessoal oDocumentosPessoalBusiness)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DocumentosPessoalBusiness.Alterar(oDocumentosPessoalBusiness);

                    TempData["MensagemSucesso"] = "O Documento '" + oDocumentosPessoalBusiness.NomeDocumento + "' foi atualizado com sucesso.";

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
        public ActionResult TerminarComRedirect(string IDDocumentosEmpregado, string NomeDocumento)
        {

            try
            {
                DocumentosPessoal oDocumentosPessoal = DocumentosPessoalBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDDocumentosEmpregado));
                if (oDocumentosPessoal == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Documento." } });
                }
                else
                {
                    oDocumentosPessoal.DataExclusao = DateTime.Now;
                    oDocumentosPessoal.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    DocumentosPessoalBusiness.Alterar(oDocumentosPessoal);

                    TempData["MensagemSucesso"] = "O Documento foi excluido com sucesso.";

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