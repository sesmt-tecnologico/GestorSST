using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {

        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }



        public ActionResult Index()
        {
            if (AutorizacaoProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Empregado")).Count() > 0)
            {
                Empregado emp = EmpregadoBusiness.Consulta.FirstOrDefault(a =>
                                        string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                        a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(AutorizacaoProvider.UsuarioAutenticado.Login.ToUpper().Trim()));
                if (emp != null)
                {
                    return RedirectToAction("Desktop", "Empregado", new { id = emp.UniqueKey });
                }
            }

            return View();
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult AlterarPropriedade(string obid, string propriedade, string valor)
        {
            try
            {

                if (propriedade.Equals("Telefone"))
                {
                    Guid uk = Guid.Parse(obid);

                    Usuario usr = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(uk));

                    if (usr == null)
                        throw new Exception("Não foi possível localizar o usuário na base de dados.");

                    usr.UsuarioExclusao = AutorizacaoProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Terminar(usr);

                    UsuarioBusiness.InserirSemEmailESenha(new Usuario()
                    {
                        UniqueKey = usr.UniqueKey,
                        Login = usr.Login,
                        Senha = usr.Senha,
                        CPF = usr.CPF,
                        Email = usr.Email,
                        Telefone = valor,
                        TipoDeAcesso = usr.TipoDeAcesso,
                        Nome = usr.Nome,
                        UKDepartamento = usr.UKDepartamento,
                        UKEmpresa = usr.UKEmpresa,
                        UsuarioInclusao = AutorizacaoProvider.UsuarioAutenticado.Login
                    });
                }

                //Ficha fichaPersistida = FichaBusiness.RecuperarPorOBID(obid, "gedexs");

                //if (fichaPersistida == null)
                //    throw new Exception("Os dados para alteração deste documento são inválidos.");

                //if (fichaPersistida.RevState != SPFRevState.WORKING)
                //    throw new Exception("Este documento não está no status apropriado para ser alterado; esta ação só pode ser realizada em documentos em andamento.");

                //fichaPersistida.CreationUser = AutorizacaoProvider.UsuarioAutenticado;

                //if (propriedade.ToUpper().Trim() == "TIPOLOGIA")
                //{
                //    TipoDocumento tipoPersistido = TipoDocumentoBusiness.RecuperarPorUID(valor, "GEDEXS");

                //    if (!string.IsNullOrWhiteSpace(tipoPersistido.Mascara))
                //        throw new Exception("O novo tipo de documento selecionado é controlado por uma máscara de identificação secundária e, portanto, somente pode ser escolhido no ato de criação de um novo documento.");

                //    if (fichaPersistida.Tipo.Equals(tipoPersistido.UID))
                //        throw new Exception("Selecione um tipo de documento diferente do atual para continuar.");

                //    Ficha objAlteracao = FichaBusiness.RecuperarPorOBID(obid, "gedexs");
                //    objAlteracao.CreationUser = AutorizacaoProvider.UsuarioAutenticado;
                //    objAlteracao.Tipo = tipoPersistido.UID;
                //    objAlteracao.Grupo = "$" + objAlteracao.Configuration.UID + "$" + objAlteracao.Grupo;

                //    FichaBusiness.AlterarEscopo(objAlteracao, fichaPersistida);
                //}
                //else
                //    FichaBusiness.AlterarPropriedades(fichaPersistida, new List<Tuple<string, string>>() { new Tuple<string, string>(propriedade, valor) }, AutorizacaoProvider.UsuarioAutenticado.Login);

                Extensions.GravaCookie("MensagemSucesso", "Informação alterada com sucesso.", 10);

                return Json(new { });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        public ActionResult Sobre()
        {
            return View();
        }
    }
}