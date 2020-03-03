using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class ArquivoController : BaseController
    {

        #region

        [Inject]
        public IArquivoBusiness ArquivoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Novo(string id)
        {
            ViewBag.RegistroID = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Arquivo pArquivo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    ArquivoBusiness.Inserir(pArquivo);

                    TempData["MensagemSucesso"] = "O Arquivo '" + pArquivo.NomeLocal + "' foi cadastrada com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "Arquivo") } });
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


        [RestritoAAjax]
        public ActionResult Upload(string ukObjeto)
        {
            try
            {

                ViewBag.UKObjeto = ukObjeto;

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
        public ActionResult Upload(Arquivo arquivo)
        {
            try
            {
                HttpPostedFileBase arquivoPostado = null;
                foreach (string fileInputName in Request.Files)
                {
                    arquivoPostado = Request.Files[fileInputName];
                    if (arquivoPostado != null && arquivoPostado.ContentLength > 0)
                    {
                        if (ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(arquivo.UKObjeto) && a.NomeLocal.ToUpper().Equals(arquivoPostado.FileName.ToUpper())).Count() > 0)
                        {
                            throw new Exception("Já existe um arquivo com este nome anexado ao incidente.");
                        }
                        else
                        {
                            var target = new MemoryStream();
                            arquivoPostado.InputStream.CopyTo(target);

                            arquivo.Conteudo = target.ToArray();
                            arquivo.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            arquivo.DataInclusao = DateTime.Now;
                            arquivo.Extensao = Path.GetExtension(arquivoPostado.FileName);
                            arquivo.NomeLocal = arquivoPostado.FileName;

                            ArquivoBusiness.Inserir(arquivo);
                        }
                    }
                }

                if (Request.Files.Count == 1)
                    return Json(new { sucesso = "O arquivo '" + arquivoPostado.FileName + "' foi anexado com êxito." });
                else
                    return Json(new { sucesso = "Os arquivos foram anexados com êxito." });

            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }


        public ActionResult Visualizar_Antigo(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        Guid uk = Guid.Parse(id);

                        Arquivo arq = ArquivoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                        if (arq == null)
                        {
                            throw new Exception("Arquivo não encontrado através da identificação recebida como parâmetro.");
                        }
                        else
                        {
                            byte[] conteudo = ArquivoBusiness.Download(arq.NomeRemoto);

                            var mimeType = string.Empty;
                            try
                            {
                                mimeType = MimeMapping.GetMimeMapping(arq.NomeLocal);
                            }
                            catch { }

                            if (string.IsNullOrWhiteSpace(mimeType))
                                mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;

                            Response.AddHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", arq.NomeLocal));

                            return File(conteudo, mimeType);
                        }


                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 500;
                        return Content(ex.Message, "text/html");
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    return Content("Parâmetros para visualização de arquivo inválido.", "text/html");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult Excluir(string ukArquivo)
        {
            try
            {
                //Apenas exclusão lógica
                if (string.IsNullOrEmpty(ukArquivo))
                    throw new Exception("A identificação do arquivo não foi localizada. Por favor, acione o administrador.");

                Guid uk = Guid.Parse(ukArquivo);

                Arquivo arquivoPersistido = ArquivoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));
                if (arquivoPersistido == null)
                    throw new Exception("As informações para exclusão do arquivo não são válidas.");

                arquivoPersistido.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                arquivoPersistido.DataExclusao = DateTime.Now;

                ArquivoBusiness.Alterar(arquivoPersistido);

                return Json(new { });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        public byte[] Download(string remoteFileName)
        {
            string vault = ConfigurationManager.AppSettings["Vault"];

            GISCore.WCF_Suporte.SuporteClient WCFSuporte = new GISCore.WCF_Suporte.SuporteClient();
            return WCFSuporte.BuscarArquivoDoVault(vault, remoteFileName);
        }

        public ActionResult Visualizar(string id)
        {
            try
            {
                if (id != null)
                {
                    try
                    {
                        Guid ukarquivo = Guid.Parse(id);
                        Arquivo arq = ArquivoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey == ukarquivo);
                        if (arq == null)
                        {
                            throw new Exception("Arquivo não encontrado através da identificação recebida como parâmetro.");
                        }
                        else
                        {
                            byte[] conteudo = ArquivoBusiness.Download(arq.NomeRemoto);

                            var mimeType = string.Empty;
                            try
                            {
                                mimeType = MimeMapping.GetMimeMapping(arq.NomeLocal);
                            }
                            catch { }

                            if (string.IsNullOrWhiteSpace(mimeType))
                                mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;

                            Response.AddHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", arq.NomeLocal));

                            return File(conteudo, mimeType);
                        }


                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 500;
                        return Content(ex.Message, "text/html");
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    return Content("Parâmetros para visualização de arquivo inválido.", "text/html");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }
        public ActionResult Download_Antigo(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        //Incidente incidentePersistido = IncidenteBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(id));
                        //if (incidentePersistido == null)
                        //    throw new Exception("As informações para geração do pacote zip de arquivos não são válidas.");

                        //List<Arquivo> arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKObjeto.Equals(id)).ToList();
                        //if (arquivos == null || arquivos.Count == 0)
                        //{
                        //    throw new Exception("Nenhum arquivo encontrado para o documento.");
                        //}

                        //byte[] conteudo = ArquivoBusiness.BaixarTodosArquivos(incidentePersistido.Codigo, arquivos);

                        //var mimeType = string.Empty;
                        //try
                        //{
                        //    mimeType = MimeMapping.GetMimeMapping(incidentePersistido.Codigo + ".zip");
                        //}
                        //catch { }

                        //if (string.IsNullOrWhiteSpace(mimeType))
                        //    mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;

                        //Response.AddHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", incidentePersistido.Codigo + ".zip"));

                        //return File(conteudo, mimeType);

                        return Content("Não implementado.", "text/html");

                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 500;
                        return Content(ex.Message, "text/html");
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    return Content("Parâmetros para visualização de arquivo inválido.", "text/html");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }


        }

    }
}