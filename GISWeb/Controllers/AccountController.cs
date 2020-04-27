using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Conta;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;

namespace GISWeb.Controllers
{

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AccountController : BaseController
    {

        #region Inject

            [Inject]
            public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IEmpregadoBusiness EmpregadoBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

        #endregion

        public ActionResult Login(string path)
        {
            ViewBag.OcultarMenus = true;
            //ViewBag.IncluirCaptcha = Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]);
            ViewBag.UrlAnterior = path;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AutenticacaoModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string msgErro = string.Empty;
                    AutorizacaoProvider.LogIn(usuario, out msgErro);

                    if (AutorizacaoProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Empregado")).Count() > 0)
                    {
                        Empregado emp = EmpregadoBusiness.Consulta.FirstOrDefault(a => 
                                                string.IsNullOrEmpty(a.UsuarioExclusao) && 
                                                a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(usuario.Login.ToUpper().Trim()));

                        if (emp != null)
                        {
                            return Json(new { url = Url.Action("Desktop", "Empregado", new { id = emp.UniqueKey }) });
                        }
                    }

                    return Json(new { url = Url.Action(ConfigurationManager.AppSettings["Web:DefaultAction"], ConfigurationManager.AppSettings["Web:DefaultController"]) });
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
            }
        }

        public ActionResult Logout()
        {
            try
            {
                ReiniciarCache(AutorizacaoProvider.UsuarioAutenticado.Login);
            }
            catch { }


            AutorizacaoProvider.LogOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [Autorizador]
        [DadosUsuario]
        public ActionResult Perfil()
        {

            Usuario usr = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(AutorizacaoProvider.UsuarioAutenticado.UniqueKey));

            AutenticacaoModel aut = AutorizacaoProvider.UsuarioAutenticado;
            aut.Telefone = usr.Telefone;

            try
            {
                Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(usr.UKDepartamento));
                aut.Departamento = dep.Sigla + " [" + dep.Codigo + "]";
            }
            catch { }

            try
            {
                Empresa emp = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(usr.UKEmpresa));
                aut.Empresa = emp.NomeFantasia;
            }
            catch { }

            return View(aut);
        }

        [HttpPost]
        [Autorizador]
        [DadosUsuario]
        public ActionResult AtualizarFoto(string imagemStringBase64)
        {
            try
            {
                UsuarioBusiness.SalvarAvatar(AutorizacaoProvider.UsuarioAutenticado.Login, imagemStringBase64, "jpg");
            }
            catch (Exception ex)
            {
                Extensions.GravaCookie("MensagemErro", ex.Message, 2);
            }

            return Json(new { url = Url.Action("Perfil") });
        }

        

        [OutputCache(Duration = 604800, Location = OutputCacheLocation.Client, VaryByParam = "login")]
        public ActionResult FotoPerfil(string login)
        {
            byte[] avatar = null;

            try
            {
                login = login.Replace(".", "").Replace("-", "");

                avatar = UsuarioBusiness.RecuperarAvatar(login);
            }
            catch { }

            if (avatar == null || avatar.Length == 0)
                avatar = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Ace/avatars/unknown.png"));

            return File(avatar, "image/jpeg");
        }



        public ActionResult DefinirNovaSenha(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {

                    Extensions.GravaCookie("MensagemSucesso", "Não foi possível recuperar a identificação do usuário.", 10);


                    //TempData["MensagemErro"] = "Não foi possível recuperar a identificação do usuário.";
                }
                else
                {
                    id = GISHelpers.Utils.Criptografador.Descriptografar(WebUtility.UrlDecode(id.Replace("_@", "%")), 1);

                    string numDiasExpiracao = ConfigurationManager.AppSettings["Web:ExpirarLinkAcesso"];
                    if (string.IsNullOrEmpty(numDiasExpiracao))
                        numDiasExpiracao = "1";

                    if (DateTime.Now.Subtract(DateTime.ParseExact(id.Substring(id.IndexOf("#") + 1), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)).Days > int.Parse(numDiasExpiracao))
                    {

                        Extensions.GravaCookie("MensagemSucesso", "Este link já expirou, solicite um outro link na opção abaixo.", 10);

                        //TempData["MensagemErro"] = "Este link já expirou, solicite um outro link na opção abaixo.";
                    }
                    else
                    {
                        NovaSenhaViewModel oNovaSenhaViewModel = new NovaSenhaViewModel();
                        //oNovaSenhaViewModel.UKUsuario = id.Substring(0, id.IndexOf("#"));
                        return View(oNovaSenhaViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
                else
                {
                    TempData["MensagemErro"] = ex.GetBaseException().Message;
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefinirSenha(NovaSenhaViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                if (entidade.NovaSenha.Equals(entidade.ConfirmarNovaSenha))
                {
                    try
                    {
                        Usuario user = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Login.Equals(AutorizacaoProvider.UsuarioAutenticado.Login));

                        if (user == null)
                            return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível localizar o usuário logado na base de dados. Favor acionar o administrador." } });

                        if (!user.Senha.Equals(UsuarioBusiness.CreateHashFromPassword(entidade.SenhaAtual)))
                            return Json(new { resultado = new RetornoJSON() { Alerta = "A senha atual não confere com a senha da base de dados." } });

                        entidade.UKUsuario = user.UniqueKey;

                        UsuarioBusiness.DefinirSenha(entidade);

                        return Json(new { resultado = new RetornoJSON() { Sucesso = "Senha alterada com sucesso." } });
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
                    return Json(new { resultado = new RetornoJSON() { Erro = "As duas senhas devem ser identicas." } });
                }
            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarAcesso(NovaSenhaViewModel novaSenhaViewModel)
        {
            if (!string.IsNullOrEmpty(novaSenhaViewModel.Email))
            {
                try
                {
                    UsuarioBusiness.SolicitarAcesso(novaSenhaViewModel.Email);
                    Extensions.GravaCookie("MensagemSucesso", "Solicitação de acesso realizada com sucesso.", 10);


                    //TempData["MensagemSucesso"] = "Solicitação de acesso realizada com sucesso.";
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Login", "Account") } });
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
                return Json(new { resultado = new RetornoJSON() { Erro = "Informe o e-mail cadastrado em sua conta." } });
            }
        }


    }
}