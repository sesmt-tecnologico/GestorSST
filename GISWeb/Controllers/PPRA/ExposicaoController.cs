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
using GISModel.Enums;
using GISHelpers.Utils;
using GISModel.DTO.PPRA;

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
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }

        [Inject]
        public IReconhecimentoBusiness ReconhecimentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_PerigoRisco> REL_PerigoRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_FontePerigo> REL_FontePerigoBusiness { get; set; }




        [Inject]
        public IWorkAreaBusiness WorkareaBusiness { get; set; }




        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Novo(Exposicao oExposicao,  string UKRisco, string UKEstabelecimento, string UKWorkarea)
        {

            Guid GuidRisc = Guid.Parse(UKRisco);
            Guid GuidUkEstab = Guid.Parse(UKEstabelecimento);
            Guid GuidUKWork = Guid.Parse(UKWorkarea);

           


            if (ExposicaoBusiness.Consulta.Any(p=>p.UKRisco.Equals(GuidRisc) && p.UKWorkArea.Equals(GuidUKWork)))
            {
                var UltimoReconhecimento = ExposicaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKWorkArea.Equals(GuidUKWork) && p.UKRisco.Equals(GuidRisc));

                var enumData = from EProbabilidadeSeg e in Enum.GetValues(typeof(EProbabilidadeSeg))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.GetDisplayName()
                               };
                var EProb = new SelectList(enumData, "ID", "Name", UltimoReconhecimento.EProbabilidadeSeg);


                List<VMLListaExposicao> ListExp = (from ex in ExposicaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   join e in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on ex.UKEstabelecimento equals e.UniqueKey
                                                   join wa in WorkareaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on e.UniqueKey equals wa.UKEstabelecimento 
                                                   join rp in REL_PerigoRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) ).ToList()
                                                   on ex.UKRisco equals rp.UKRisco
                                                   join r in RiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on ex.UKRisco equals r.UniqueKey
                                                   join p in PerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on rp.UKPerigo equals p.UniqueKey
                                                   join fp in REL_FontePerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on p.UniqueKey equals fp.UKPerigo
                                                   join f in FonteGeradoraDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                   on fp.UKFonteGeradora equals f.UniqueKey
                                                   where ex.UKWorkArea.Equals(GuidUKWork) && ex.UKRisco.Equals(GuidRisc)
                                                   select new VMLListaExposicao()
                                                   {
                                                       EExposicaoInsalubre = ex.EExposicaoInsalubre,
                                                       EExposicaoCalor = ex.EExposicaoCalor,
                                                       EExposicaoSeg = ex.EExposicaoSeg,
                                                       Estabelecimento = e.NomeCompleto,
                                                       Workarea = wa.Nome,
                                                       FonteGeradora = f.FonteGeradora,
                                                       Risco = r.Nome,
                                                       UsuarioInclusao = ex.UsuarioInclusao,
                                                       DataInclusao = ex.DataInclusao


                                                   }).ToList();



                ViewBag.lista = ListExp.ToList();





                ViewBag.Prob =  UltimoReconhecimento.EProbabilidadeSeg.GetDisplayName();

                ViewBag.ukExposi = UltimoReconhecimento.UniqueKey;

                ViewBag.ukWorkarea = UltimoReconhecimento.UKWorkArea;



                ViewBag.ultimo = UltimoReconhecimento;
                
            }

            ViewBag.ukworkarea = GuidUKWork;
            ViewBag.IDRisc= GuidRisc;
            ViewBag.Estab = GuidUkEstab;

            ViewBag.Imagens = AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(GuidRisc))).ToList();

            var Reconhecimento = ReconhecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKWorkarea.Equals(GuidUKWork) && p.UKRisco.Equals(GuidRisc));

            ViewBag.classeRisco = Reconhecimento.EClasseDoRisco;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Exposicao oExposicao, string ukWorkarea, string ukRisc, string ukEstab)
        {

            Guid Ukestabelecimento = Guid.Parse(ukEstab);
            Guid UKRisco = Guid.Parse(ukRisc);
            Guid UKWorkarea = Guid.Parse(ukWorkarea);

            if (ModelState.IsValid)
            {
                try
                {


                    oExposicao.UKEstabelecimento = Ukestabelecimento;
                    oExposicao.UKRisco = UKRisco;
                    oExposicao.UKWorkArea = UKWorkarea;
                    oExposicao.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ExposicaoBusiness.Inserir(oExposicao);

                    Extensions.GravaCookie("MensagemSucesso", "A Exposição foi registrada com sucesso.", 10);


                    
                    

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FonteGeradoraDeRisco") } });
                    
                  
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
