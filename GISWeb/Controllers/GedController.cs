using GISCore.Business.Abstract;
using GISCore.Business.Concrete;
using GISModel.DTO.Ged;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class GedController : Controller
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
        public string SaveUpload(IEnumerable<HttpPostedFileBase> file)
        {
            try
            {
                string nomeArquivo = string.Empty;
                string arquivoEnviados = string.Empty;

                foreach (string arquivo in Request.Files)
                {
                    HttpPostedFileBase files = Request.Files[arquivo];
                    string extension = System.IO.Path.GetExtension(Request.Files[arquivo].FileName);
                    var webImage = new System.Web.Helpers.WebImage(Request.Files[0].InputStream);
                    byte[] imgByteArray = webImage.GetBytes();

                    var _arquivo = new Arquivo()
                    {
                        ID = Guid.NewGuid(),
                        UniqueKey = Guid.NewGuid(),
                        UKObjeto = Guid.Parse(Session["UKEmpregado"].ToString()),
                        Conteudo = imgByteArray.ToArray(),
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        DataInclusao = DateTime.Now,
                        DataExclusao = new DateTime(9999,12,31,23,59,59,997),
                        Extensao = Path.GetExtension(Request.Files[arquivo].FileName),
                        NomeLocal = Request.Files[arquivo].FileName
                    };

                    this.ArquivosBusiness.Inserir(_arquivo);
                };

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
                ViewBag.Mensagem = "Erro : " + ex.Message;
                return "Erro : " + ex.Message;
            }

            return "Arquivo(s) enviados com sucesso.";
        }
    }
}