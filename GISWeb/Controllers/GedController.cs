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
            Guid UK = Guid.Parse(id);
            Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UK));
            var ged = new GedViewModel();
            ged.Empregado = oEmp;

            return View(ged);
        }

        public ActionResult Upload(string id)
        {
            ViewBag.UKEmpregado = id;
            var ukAdmissao = AdmissaoBusiness.GetAdmissao(Guid.Parse(id)).UniqueKey;

            ViewBag.ListaAdmissao = AdmissaoBusiness.BuscarAlocacoes(ukAdmissao.ToString());

            return View();
        }

        [HttpPost]
        public ActionResult SaveUpload(string ukEmpregado)
        {
            try
            {
                string nomeArquivo = string.Empty;
                string arquivoEnviados = string.Empty;
                Guid UKEmpregado = Guid.Parse(ukEmpregado);

                HttpPostedFileBase arquivoPostado = null;
                foreach (string arquivo in Request.Files)
                {
                    //string extension = System.IO.Path.GetExtension(Request.Files[arquivo].FileName);
                    //var webImage = new System.Web.Helpers.WebImage(Request.Files[arquivo].InputStream);
                    //byte[] imgByteArray = webImage.GetBytes();

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
                                UniqueKey = UKEmpregado,
                                UKObjeto = Guid.NewGuid(),
                                Conteudo = target.ToArray(),
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                DataInclusao = DateTime.Now,
                                DataExclusao = new DateTime(9999, 12, 31, 23, 59, 59, 997),
                                Extensao = Path.GetExtension(nomeArquivo),
                                NomeLocal = nomeArquivo
                            };

                            this.ArquivosBusiness.Inserir(_arquivo);

                            //Implementar REL_ArquivoEmpregado
                            //Atraves do UKEmpregado - Locacao, Funcao
                            //Selecionar a locacao onde a uniquekey do empregado e igual UKEmpregado
                        }
                    }
                };

                if (Request.Files.Count == 1)
                    Extensions.GravaCookie("MensagemSucesso", "O arquivo '" + arquivoPostado.FileName + "' foi anexado com êxito.", 10);
                else
                    Extensions.GravaCookie("MensagemSucesso", "Os arquivos foram anexados com êxito.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Upload", "Ged", new { id = UKEmpregado.ToString() }) } });

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