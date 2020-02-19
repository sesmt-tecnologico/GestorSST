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
            Session["UKEmpregado"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult SaveUpload()
        {
            try
            {
                string nomeArquivo = string.Empty;
                string arquivoEnviados = string.Empty;

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

                        var _arquivo = new Arquivo()
                        {
                            ID = Guid.NewGuid(),
                            UniqueKey = Guid.NewGuid(),
                            UKObjeto = Guid.Parse(Session["UKEmpregado"].ToString()),
                            Conteudo = target.ToArray(),
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            DataInclusao = DateTime.Now,
                            DataExclusao = new DateTime(9999, 12, 31, 23, 59, 59, 997),
                            Extensao = Path.GetExtension(nomeArquivo),
                            NomeLocal = nomeArquivo
                        };

                        this.ArquivosBusiness.Inserir(_arquivo);
                    }
                };

                if (Request.Files.Count == 1)
                    return Json(new { sucesso = "O arquivo '" + arquivoPostado.FileName + "' foi anexado com êxito." });
                else
                    return Json(new { sucesso = "Os arquivos foram anexados com êxito." });

                // var wcf = new GISCore.WCF_Suporte.SuporteClient();

                // wcf.SalvarArquivoNoVault()

                //nomeArquivo = Path.GetFileName(files.FileName);
                //if (arquivo.ContentLength > 0)
                //{
                //    nomeArquivo = Path.GetFileName(arquivo.FileName);
                //    var caminho = Path.Combine(Server.MapPath("~/Imagens"), nomeArquivo);
                //    arquivo.SaveAs(caminho);
                //}

                //arquivoEnviados = arquivoEnviados + " , " + nomeArquivo;
                //arquivoEnviados = nomeArquivo;

                // ViewBag.Mensagem = "Arquivos enviados  : " + arquivoEnviados + " , com sucesso.";
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                if (ex.GetBaseException() == null)
                {
                    //return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });

                    return Content(ex.Message, "text/html");
                }
                else
                {
                    //return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    return Content(ex.GetBaseException().Message, "text/html");
                }
            }
        }
    }
}