using GISCore.Business.Abstract;
using GISCore.Business.Concrete;
using GISModel.DTO.Ged;
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
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class GedController : BaseController
    {
        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IArquivoBusiness ArquivosBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IREL_ArquivoEmpregadoBusiness REL_ArquivoEmpregadoBusiness { get; set; }

        // GET: Ged

        public ActionResult Index(string id)
        {
            Guid ukEmpregado = Guid.Parse(id);
            Guid ukAdmissao = this.AdmissaoBusiness.GetAdmissao(ukEmpregado).UniqueKey;

            Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ukEmpregado));
            var ged = new GedViewModel();
            ged.Empregado = oEmp;

            return View(ged);
        }

        public ActionResult Upload(string ukemp, string ukalocado, string ukfuncao)
        {
            //ViewBag.UKEmpregado = ukemp;
            var ukAdmissao = AdmissaoBusiness.GetAdmissao(Guid.Parse(ukemp)).UniqueKey;

            ViewBag.ListaAdmissao = AdmissaoBusiness.BuscarAlocacoes(ukAdmissao.ToString());

            ViewBag.UKEmpregado = ukemp;
            ViewBag.UKAlocado = ukalocado;
            ViewBag.UKFuncao = ukfuncao;

            return View();
        }

        public ActionResult BuscarAdmissoesAtuaisGed(string UKEmpregado)
        {
            var lista = this.AdmissaoBusiness.BuscarAdmissoesAtuais(UKEmpregado);

            return PartialView("_BuscarAdmissoesAtuaisGed", lista);
        }

        [HttpPost]
        public ActionResult SaveUpload(string ukEmpregado, string ukAlocado, string ukFuncao)
        {
            try
            {
                string nomeArquivo = string.Empty;
                string arquivoEnviados = string.Empty;
                Guid UKEmpregado = Guid.Parse(ukEmpregado);

                HttpPostedFileBase arquivoPostado = null;
                foreach (string arquivo in Request.Files)
                {
                    arquivoPostado = Request.Files[arquivo];
                    if (arquivoPostado != null && arquivoPostado.ContentLength > 0)
                    {
                        var target = new MemoryStream();
                        arquivoPostado.InputStream.CopyTo(target);

                        nomeArquivo = Request.Files[arquivo].FileName;
                        if (ArquivosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && 
                        a.UniqueKey.Equals(UKEmpregado) && a.NomeLocal.ToUpper().Equals(arquivoPostado.FileName.ToUpper())).Count() > 0)
                        {
                            throw new Exception("Já existe um arquivo com este nome.");
                        }
                        else
                        {
                            var _arquivo = new Arquivo()
                            {
                                UKObjeto = UKEmpregado,
                                Conteudo = target.ToArray(),
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                DataInclusao = DateTime.Now,
                                DataExclusao = new DateTime(9999, 12, 31, 23, 59, 59, 997),
                                Extensao = Path.GetExtension(nomeArquivo),
                                NomeLocal = nomeArquivo
                            };

                            this.ArquivosBusiness.Inserir(_arquivo);

                            var _RELarquivoEmpregado = new REL_ArquivoEmpregado()
                            {
                                UKEmpregado = UKEmpregado, 
                                UKObjetoArquivo = _arquivo.UniqueKey,
                                UKLocacao = Guid.Parse(ukAlocado),
                                UKFuncao = Guid.Parse(ukFuncao),
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            };

                            this.REL_ArquivoEmpregadoBusiness.Inserir(_RELarquivoEmpregado);


                        }
                    }
                };
                
                if (Request.Files.Count == 1)
                    Extensions.GravaCookie("MensagemSucesso", "O arquivo '" + arquivoPostado.FileName + "' foi anexado com êxito.", 1);
                else
                    Extensions.GravaCookie("MensagemSucesso", "Os arquivos foram anexados com êxito.", 1);
                Response.StatusCode = 200;
                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Ged", new { id = UKEmpregado.ToString() }) } });

                //return RedirectToAction(nameof(Upload), new { id = UKEmpregado.ToString() });
                //if (Request.Files.Count == 1)
                //    return Json(new { sucesso = "O arquivo '" + arquivoPostado.FileName + "' foi anexado com êxito." });
                //else
                //    return Json(new { sucesso = "Os arquivos foram anexados com êxito." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                if (ex.GetBaseException() == null)
                {
                    Extensions.GravaCookie("MensagemErro",ex.Message, 1);
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    Extensions.GravaCookie("MensagemErro", ex.GetBaseException().Message, 1);
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }
        }


    }
}