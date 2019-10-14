using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EmpresaController : BaseController
    {

        #region

        [Inject]
        public ICNAEBusiness CNAEBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p=> string.IsNullOrEmpty(p.UsuarioExclusao) ).ToList();

            return View();
        }

        public ActionResult EmpresaCriacoes(string id)
        {

            Guid ID = Guid.Parse(id);

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(ID)).ToList();

            var Lista = from Dep in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        join Est in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        on Dep.ID equals Est.ID
                        join Dir in DiretoriaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        on Dep.ID equals Dir.ID
                        where Dir.IDEmpresa.Equals(id)
                        select new Estabelecimento
                        {
                            ID = Est.ID,
                            NomeCompleto = Est.NomeCompleto,
                            //Departamento = new Departamento
                            //{
                            //    ID = Dep.ID,
                            //    Sigla = Dep.Sigla,                            
                            //    //Diretoria = new Diretoria
                            //    //{
                            //    //    ID = Dir.ID,
                            //    //    Sigla = Dir.Sigla
                            //    //}
                            //},

                        };

            List<Estabelecimento> lista01 = Lista.ToList();

            ViewBag.Lista = lista01;




            return View();
        }

        public ActionResult BuscarEmpresaPorID(string IDEmpresa) {

            try
            {
                Empresa oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDEmpresa));
                if (oEmpresa == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Empresa com o ID '" + IDEmpresa + "' não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oEmpresa) });
                }
            }
            catch (Exception ex) {
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

        public ActionResult Novo() 
        {
            //ViewBag.CNAE = new SelectList(CNAEBusiness.Consulta.ToList(), "IDCNAE", "Codigo");
            
            return View();
        }

        public ActionResult Edicao(string id)
        {
            Guid UKEmpresa = Guid.Parse(id);
            Empresa obj = EmpresaBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(UKEmpresa));
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Empresa Empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Empresa.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EmpresaBusiness.Inserir(Empresa);

                    Extensions.GravaCookie("MensagemSucesso", "A empresa '" + Empresa.NomeFantasia + "' foi cadastrada com sucesso.", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empresa") } });
                }
                catch (Exception ex) {
                    if (ex.GetBaseException() == null) {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else {
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Empresa Empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpresaBusiness.Alterar(Empresa);

                    Extensions.GravaCookie("MensagemSucesso", "A empresa '" + Empresa.NomeFantasia + "' foi atualizada com sucesso.", 10);
                    
                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empresa") } });
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
        public ActionResult Terminar(string IDEmpresa)
        {

            try {
                Empresa oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDEmpresa));
                if (oEmpresa == null) {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else {

                    //oEmpresa.DataExclusao = DateTime.Now;
                    oEmpresa.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EmpresaBusiness.Alterar(oEmpresa);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "A empresa '" + oEmpresa.NomeFantasia + "' foi excluída com sucesso." } });
                }
            }
            catch (Exception ex) {
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
        public ActionResult TerminarComRedirect(string IDEmpresa)
        {

            try
            {
                Empresa oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDEmpresa));
                if (oEmpresa == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else
                {
                    //oEmpresa.DataExclusao = DateTime.Now;
                    oEmpresa.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    EmpresaBusiness.Alterar(oEmpresa);

                    TempData["MensagemSucesso"] = "A empresa '" + oEmpresa.NomeFantasia + "' foi excluída com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empresa") } });
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

        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult Upload()
        {
            try
            {
                string fName = string.Empty;
                string msgErro = string.Empty;
                foreach (string fileName in Request.Files.AllKeys)
                {
                    HttpPostedFileBase oFile = Request.Files[fileName];
                    fName = oFile.FileName;
                    if (oFile != null)
                    {
                        string sExtensao = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
                        if (sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("GIF")) {
                            //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                            
                        }
                        else
                        {
                            throw new Exception("Extensão do arquivo não permitida.");
                        }
                        
                    }
                }
                if (string.IsNullOrEmpty(msgErro))
                    return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", arquivo = fName, erro = msgErro });
                else
                    return Json(new { erro = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

    }
}