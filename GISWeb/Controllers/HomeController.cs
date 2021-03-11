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
using System.Data;
using System.Collections.Generic;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        #region

        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DocumentosAlocados> REL_DocumentosAlocados { get; set; }

        #endregion

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

        public ActionResult Suporte() 
        {

            List<Perfil> perfis = new List<Perfil>();

            string sql = @"select p.Nome as Perfil, u.Nome as Usuario, d.Sigla, d.Codigo, u.Telefone, u.Email
                           from tbUsuarioPerfil up, tbUsuario u, tbPerfil p, tbDepartamento d
                           where up.DataExclusao = '9999-12-31 23:59:59.997' and
	                             up.UKUsuario = u.UniqueKey and u.DataExclusao = '9999-12-31 23:59:59.997' and
	                             up.UKConfig = d.UniqueKey and d.DataExclusao = '9999-12-31 23:59:59.997' and 
	                             up.UKPerfil = p.UniqueKey and p.DataExclusao = '9999-12-31 23:59:59.997' and
	                             p.Nome in ('Administrador', 'Medicina', 'Técnico', 'Gestor Técnico')
                           order by p.Nome, u.Nome";

            DataTable result = UsuarioBusiness.GetDataTable(sql);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Perfil temp = perfis.FirstOrDefault(a => a.Nome.Equals(row["Perfil"].ToString()));
                    if (temp == null)
                    {
                        perfis.Add(new Perfil()
                        {
                            Nome = row["Perfil"].ToString(),
                            Usuarios = new List<Usuario>() {
                               new Usuario() {
                                    Nome = row["Usuario"].ToString(),
                                    Email = row["Email"].ToString(),
                                    Telefone = row["Telefone"].ToString(),
                                    Departamento = new Departamento()
                                    {
                                        Sigla = row["Sigla"].ToString() + " [" + row["Codigo"].ToString() + "]"
                                    }
                               }
                            }
                        });
                    }
                    else 
                    {
                        temp.Usuarios.Add(new Usuario()
                        {
                            Nome = row["Usuario"].ToString(),
                            Email = row["Email"].ToString(),
                            Telefone = row["Telefone"].ToString(),
                            Departamento = new Departamento()
                            {
                                Sigla = row["Sigla"].ToString() + " [" + row["Codigo"].ToString() + "]"
                            }
                        });
                    }
                }
            }

            ViewBag.Perfis = perfis;

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

        [HttpPost]
        [RestritoAAjax]
        public ActionResult AlterarDataDoc( string uk, string propriedade, string valor)
        {
            try
            {

                if (propriedade.Equals("Realizado"))
                {
                    Guid UKDocAloc = Guid.Parse(uk);

                    var usr = AutorizacaoProvider.UsuarioAutenticado.Login;

                    var doc = REL_DocumentosAlocados.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                    && a.UniqueKey.Equals(UKDocAloc));

                    doc.DataDocumento =Convert.ToDateTime(valor);

                    REL_DocumentosAlocados.Alterar(doc);


                    //REL_DocumentosAlocados.Alterar(new REL_DocumentosAlocados()
                    //{
                    //    ID = doc.ID

                    //});

                    //UsuarioBusiness.Terminar(usr);

                    //UsuarioBusiness.InserirSemEmailESenha(new Usuario()
                    //{
                    //    UniqueKey = usr.UniqueKey,
                    //    Login = usr.Login,
                    //    Senha = usr.Senha,
                    //    CPF = usr.CPF,
                    //    Email = usr.Email,
                    //    Telefone = valor,
                    //    TipoDeAcesso = usr.TipoDeAcesso,
                    //    Nome = usr.Nome,
                    //    UKDepartamento = usr.UKDepartamento,
                    //    UKEmpresa = usr.UKEmpresa,
                    //    UsuarioInclusao = AutorizacaoProvider.UsuarioAutenticado.Login
                    //});
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