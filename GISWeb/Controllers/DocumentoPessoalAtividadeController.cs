using GISCore.Business.Abstract;
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
    public class DocumentoPessoalAtividadeController : BaseController
    {

        #region Inject

        [Inject]
        public IREL_DocomumentoPessoalAtividadeBusiness DocsPorAtividadeBusiness { get; set; }

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

            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo(string nome, string uk)
        {

            var UK = Guid.Parse(uk);

            ViewBag.Documentos = new SelectList(DocumentosPessoalBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "UniqueKey", "NomeDocumento");
           
            ViewBag.IDAtividade = AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)&&(d.UniqueKey.Equals(UK))).ToList(); ;
            ViewBag.idAtiv = UK;

            ViewBag.Doc = DocsPorAtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.UKAtividade.Equals(UK))).Count();


            string sql = @"select a.UniqueKey, a.Descricao as nome, d.UniqueKey as UniqueKeyD, d.NomeDocumento as NomeD, d.DescricaoDocumento as DescricaoD,
                            da.UKAtividade as rel1, da.UKDocumentoPessoal
                            from tbAtividade a 
                            left join REL_DocumentoPessoalAtividade da on da.UKAtividade = a.uniqueKey  
                            left join tbDocumentosPessoal d on d.UniqueKey = da.UKDocumentoPessoal  
                             where da.UKAtividade = '" + UK.ToString() + @"' order by d.NomeDocumento ";
                            


            DataTable result = DocsPorAtividadeBusiness.GetDataTable(sql);

            List<DocumentosPessoal> lista = new List<DocumentosPessoal>();


            if (result.Rows.Count > 0)
            {
                DocumentosPessoal obj = null;

                foreach (DataRow row in result.Rows)
                {
                    if (result.Rows.Count > 0)
                    {
                        obj = new DocumentosPessoal()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKeyD"].ToString()),
                            NomeDocumento = row["NomeD"].ToString()

                        };
                    }

                }

                if (obj != null)
                    lista.Add(obj);
            }

            ViewBag.lista = lista;




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(REL_DocomumentoPessoalAtividade oDocAtividade, string idAtiv)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    oDocAtividade.UKAtividade = Guid.Parse(idAtiv);
                    DocsPorAtividadeBusiness.Inserir(oDocAtividade);

                    TempData["MensagemSucesso"] = "O Documento foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "DocumentoPessoalAtividade") } });
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