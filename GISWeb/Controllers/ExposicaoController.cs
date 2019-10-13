using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
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
    public class ExposicaoController : BaseController
    {

        #region

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }
        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoImagensBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeDeRiscoBusiness { get; set; }

        [Inject]
        public IExposicaoBusiness ExposicaoBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Novo(Exposicao oExposicao, string IDAtividadeAlocada, string idAlocacao, string idTipoDeRisco, string idEmpregado)
        {
            if(ExposicaoBusiness.Consulta.Any(p=>p.idAtividadeAlocada.Equals(IDAtividadeAlocada) && p.idTipoDeRisco.Equals(idTipoDeRisco)))
            {
                return Json(new { resultado = new RetornoJSON() { Alerta = "Já existe uma exposição para esta Alocação!" } });
                
            }
            else { 
           
            ViewBag.AtivAloc = IDAtividadeAlocada;
            ViewBag.IDaloc = idAlocacao;
            ViewBag.IDRisc= idTipoDeRisco;
            ViewBag.IdEmpregado = idEmpregado;

            ViewBag.Imagens = AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(IDAtividadeAlocada))).ToList();

            var EXPO = (from TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                            
                        join ATE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        on TR.idAtividadesDoEstabelecimento equals ATE.ID
                        where TR.ID.Equals(idTipoDeRisco)
                        select new TipoDeRisco()
                        {
                            ID = TR.ID,
                            idPossiveisDanos = TR.idPossiveisDanos,
                            idAtividadesDoEstabelecimento = TR.idAtividadesDoEstabelecimento,
                            idEventoPerigoso = TR.idEventoPerigoso,
                            idPerigoPotencial = TR.idPerigoPotencial,
                            EClasseDoRisco = TR.EClasseDoRisco,
                            FonteGeradora = TR.FonteGeradora,
                            Tragetoria = TR.Tragetoria

                        }).ToList();


            ViewBag.Riscos = EXPO;

            var Aloc = (from a in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(idAlocacao))).ToList()
                        select new
                        {
                            id = a.ID
                        }
                        ).ToList();


            List<string> Filtro = new List<string>();

            Guid? filtro = null;

            foreach (var item in Aloc)
            {
                filtro = item.id;
            }
            
            List<string> model = Filtro;

            ViewBag.IDaloc = filtro;


            try
            {
                
                if (oExposicao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Novo", oExposicao) });
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

            //return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Exposicao oExposicao, string idAtividadeAlocada, string idAlocacao, string idTipoDeRisco, string idEmpregado)
        {
            
            if (ModelState.IsValid)
            {
                try
                {

                    oExposicao.idAtividadeAlocada = Guid.Parse(idAtividadeAlocada);
                    oExposicao.idAlocacao = Guid.Parse(idAlocacao);
                    oExposicao.idTipoDeRisco = Guid.Parse(idTipoDeRisco);
                    ExposicaoBusiness.Inserir(oExposicao);

                    Extensions.GravaCookie("MensagemSucesso", "A Exposição foi registrada com sucesso.", 10);


                    
                    //return Json(new { data = RenderRazorViewToString("_DetalhesAmbienteAlocado", oExposicao) }); 

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("PerfilEmpregado", "Admissao", new { id = idEmpregado}) } });
                    
                    //return Json(new { resultado = new RetornoJSON() { Sucesso = "Exposição Cadastrada com sucesso!" } });


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
            return View(EstabelecimentoImagensBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(EstabelecimentoAmbiente oEstabelecimentoImagens)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EstabelecimentoImagensBusiness.Alterar(oEstabelecimentoImagens);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi atualizada com sucesso.", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EstabelecimentoImagens") } });
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
        public ActionResult Terminar(string IDEstebelecimentoImagens)
        {

            try
            {
                EstabelecimentoAmbiente oEstabelecimentoImagens = EstabelecimentoImagensBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDEstebelecimentoImagens));
                if (oEstabelecimentoImagens == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else
                {

                    //oEmpresa.DataExclusao = DateTime.Now;
                    oEstabelecimentoImagens.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EstabelecimentoImagensBusiness.Alterar(oEstabelecimentoImagens);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi excluída com sucesso." } });
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
        public ActionResult TerminarComRedirect(string IDEstebelecimentoImagens)
        {

            try
            {
                EstabelecimentoAmbiente oEstabelecimentoImagens = EstabelecimentoImagensBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDEstebelecimentoImagens));
                if (oEstabelecimentoImagens == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a imagem  '" + oEstabelecimentoImagens.NomeDaImagem + "', pois a mesma não foi localizada." } });
                }
                else
                {
                    //oEmpresa.DataExclusao = DateTime.Now;
                    oEstabelecimentoImagens.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    EstabelecimentoImagensBusiness.Alterar(oEstabelecimentoImagens);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi excluída com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EstabelecimentoImagens") } });
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
