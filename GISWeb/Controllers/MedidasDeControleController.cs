using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MedidasDeControleController : BaseController
    {

        #region

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness RiscosDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index(string id)
        {

            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(id))).ToList();
            return View();
        }

        public ActionResult BuscarDetalhesDeMedidasDeControleEstabelecimento(string IDTipoDeRisco)
        {
            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(IDTipoDeRisco))).ToList();
            try
            {
                MedidasDeControleExistentes oMedidasDeControleExistentes = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(IDTipoDeRisco));
                if (oMedidasDeControleExistentes == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Favor cadastrar uma medida de controle ou criar um Plano de Ação!!! ." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oMedidasDeControleExistentes) });
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

        public ActionResult NovoControleRiscoFuncao(string id, string idAtivRisco)
        {
            ViewBag.EstabID = id;
            ViewBag.AtivRisco = idAtivRisco;
            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(id))).ToList();
            ViewBag.TipoDeRisco = TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(id))).ToList();
            ViewBag.RegistroID = new SelectList(MedidasDeControleBusiness.Consulta, "RegistroID", "Diretoria");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarControleRiscoFuncao(MedidasDeControleExistentes oMedidasDeControleExistentes, string RegistroID, string AtivRiscoID)
        {

            oMedidasDeControleExistentes.IDTipoDeRisco = Guid.Parse(RegistroID);
            //oMedidasDeControleExistentes.IDAtividadeRiscos = AtivRiscoID;

            if (ModelState.IsValid)
            {
                try
                {

                    MedidasDeControleBusiness.Inserir(oMedidasDeControleExistentes);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oMedidasDeControleExistentes.NomeDaImagem + "'foi cadastrada com sucesso.", 10);
                                                            

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("NovoControleRiscoFuncao", "MedidasDeControle", new { id = oMedidasDeControleExistentes.IDTipoDeRisco }) } });
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

        public ActionResult Novo(string id, string idAtivRisco)
        {

            ViewBag.EstabID = id;
            ViewBag.AtivRisco = idAtivRisco;
            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(id))).ToList();
            ViewBag.TipoDeRisco = TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(id))).ToList();
            ViewBag.RegistroID = new SelectList(MedidasDeControleBusiness.Consulta, "RegistroID", "Diretoria");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(MedidasDeControleExistentes oMedidasDeControleExistentes, string RegistroID,string AtivRiscoID)
        {

            oMedidasDeControleExistentes.IDTipoDeRisco = Guid.Parse(RegistroID);
            //oMedidasDeControleExistentes.IDAtividadeRiscos = AtivRiscoID;

            if (ModelState.IsValid)
            {
                try
                {

                    MedidasDeControleBusiness.Inserir(oMedidasDeControleExistentes);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oMedidasDeControleExistentes.NomeDaImagem + "'foi cadastrada com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "MedidasDeControle", new { id = oMedidasDeControleExistentes.IDTipoDeRisco }) } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(MedidasDeControleExistentes oMedidasDeControleExistentes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MedidasDeControleBusiness.Alterar(oMedidasDeControleExistentes);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oMedidasDeControleExistentes.NomeDaImagem + "' foi atualizada com sucesso.", 10);

                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "MedidasDeControle") } });
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
        public ActionResult Terminar(string IDMedidasDeControle)
        {

            try
            {
                MedidasDeControleExistentes oIDMedidasDeControle = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDMedidasDeControle));
                if (oIDMedidasDeControle == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta medida de controle, pois a mesma não foi localizada." } });
                }
                else
                {

                    //oEmpresa.DataExclusao = DateTime.Now;
                    oIDMedidasDeControle.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    MedidasDeControleBusiness.Alterar(oIDMedidasDeControle);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "A imagem '" + oIDMedidasDeControle.NomeDaImagem + "' foi excluída com sucesso." } });
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

        [HttpPost]
        public ActionResult TerminarComRedirect(string IDMedidasDeControle)
        {

            try
            {
                MedidasDeControleExistentes oEstabelecimentoImagens = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDMedidasDeControle));
                if (oEstabelecimentoImagens == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a imagem  '" + oEstabelecimentoImagens.NomeDaImagem + "', pois a mesma não foi localizada." } });
                }
                else
                {
                    //oEmpresa.DataExclusao = DateTime.Now;
                    oEstabelecimentoImagens.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    MedidasDeControleBusiness.Alterar(oEstabelecimentoImagens);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi excluída com sucesso.", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "MedidasDeControle") } });
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
                        if (sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("GIF"))
                        {
                            

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
