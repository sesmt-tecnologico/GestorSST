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

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EstabelecimentoAmbienteController : BaseController
    {

        #region

        [Inject]
        public IPlanoDeAcaoBusiness PlanoDeAcaoBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoImagensBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index(string id)
        {
           
            ViewBag.Imagens = EstabelecimentoImagensBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)&&(p.IDEstabelecimento.Equals(id))).ToList();
            return View();
        }

        public ActionResult Lista(string id, string nome)
        {

            ViewBag.nome = nome;
            ViewBag.Imagens = EstabelecimentoImagensBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(id))).ToList();


            var ExistePlanoAcao = from PA in PlanoDeAcaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  on PA.Identificador equals Guid.Parse(id)
                                  select new PlanoDeAcao
                                  {
                                      Identificador = PA.Identificador
                                  };

            List<PlanoDeAcao> PlanoAcao = ExistePlanoAcao.ToList();

            var total = PlanoAcao.Count();
            ViewBag.total = total;


            return View();
        }

        public ActionResult BuscarDetalhesDosRiscos(string idEstabelecimento)
        {
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimentoImagens.Equals(idEstabelecimento))).ToList();
            try
            {
                AtividadesDoEstabelecimento oRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimentoImagens.Equals(idEstabelecimento));
                if (oRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens4 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oRiscosDoEstabelecimento) });
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

        public ActionResult BuscarDetalhesEstabelecimentoImagens(string IDEstabelecimento)
        {
            ViewBag.Imagens = EstabelecimentoImagensBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(IDEstabelecimento))).ToList();
            try
            {
                EstabelecimentoAmbiente oEstabelecimentoImagens = EstabelecimentoImagensBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimento.Equals(IDEstabelecimento));
                if (oEstabelecimentoImagens == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Imagens3 não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oEstabelecimentoImagens) });
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

        public ActionResult Novo(string id)
        {
            
            ViewBag.EstabID = id;

            ViewBag.Imagens = EstabelecimentoImagensBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(id))).ToList();
            //ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(id))).ToList();

           List<Estabelecimento>  EstabAmbiente = (from Estab in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                join Dep in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                on Estab.ID equals Dep.ID
                                join Dir in DiretoriaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                on Dep.ID equals Dir.ID
                                join Emp in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                on Dir.IDEmpresa equals Emp.ID
                                where Estab.ID.Equals(id)
                                select new Estabelecimento()
                                {
                                    ID = Estab.ID,
                                    TipoDeEstabelecimento = Estab.TipoDeEstabelecimento,
                                    Descricao = Estab.Descricao,
                                    NomeCompleto = Estab.NomeCompleto,
                                    //ID = Estab.ID,

                                    //Departamento = new Departamento()
                                    //{
                                    //    ID = Dep.ID,
                                    //    Sigla = Dep.Sigla,
                                    //    Descricao = Dep.Descricao
                                    //    //Diretoria = new Diretoria()
                                    //    //{
                                    //    //    ID = Dir.ID,
                                    //    //    Empresa = new Empresa()
                                    //    //    {
                                    //    //        ID = Emp.ID
                                    //    //    }
                                    //    //}
                                    //}

                                  }
                                ).ToList();

            ViewBag.Estabelecimento = EstabAmbiente;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(EstabelecimentoAmbiente oEstabelecimentoImagens,string RegistroID)
        {

            //id do Estabelecimento recebido por parametro
            oEstabelecimentoImagens.IDEstabelecimento = Guid.Parse(RegistroID);

            if (ModelState.IsValid)
            {
                try
                {

                    EstabelecimentoImagensBusiness.Inserir(oEstabelecimentoImagens);

                    TempData["MensagemSucesso"] = "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "'foi cadastrada com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "EstabelecimentoAmbiente", new {id = oEstabelecimentoImagens.IDEstabelecimento }) } });
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

                    TempData["MensagemSucesso"] = "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi atualizada com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EstabelecimentoAmbiente") } });
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

                    TempData["MensagemSucesso"] = "A imagem '" + oEstabelecimentoImagens.NomeDaImagem + "' foi excluída com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "EstabelecimentoAmbiente") } });
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
