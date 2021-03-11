using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Usuario;
using System.Data;
using System.Globalization;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class UsuarioController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        [MenuAtivo(MenuAtivo = "Administracao/Usuarios")]
        public ActionResult Index()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Sigla);
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList().OrderBy(a => a.NomeFantasia);

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Usuarios")]
        public ActionResult Novo()
        {
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool bRedirect = false;
                    if (Usuario.Senha != null && Usuario.Senha.Equals("redirect"))
                        bRedirect = true;



                    if (Usuario.TipoDeAcesso == GISModel.Enums.TipoDeAcesso.Sistema)
                    {
                        string senha = GISHelpers.Utils.Severino.GeneratePassword();
                        // Usuario.Senha = senha;
                        Usuario.Senha = "caete123";
                    }


                    
                    Usuario.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Inserir(Usuario);

                    if (bRedirect)
                    {
                        Extensions.GravaCookie("MensagemSucesso", "O usuário '" + Usuario.Nome + "' foi cadastrado com sucesso.", 10);

                        
                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Sucesso = "O usuário '" + Usuario.Nome + "' foi cadastrado com sucesso." } });
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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [MenuAtivo(MenuAtivo = "Administracao/Usuarios")]
        public ActionResult Edicao(string id)
        {
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            List<Usuario> usuarios = (from usr in UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(id)).ToList()
                                      join emp in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usr.ID equals emp.ID
                                      join dep in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usr.ID equals dep.ID
                                      select new Usuario()
                                      {
                                          ID = usr.ID,
                                          Nome = usr.Nome,
                                          Login = usr.Login,
                                          CPF = usr.CPF,
                                          Email = usr.Email,
                                          TipoDeAcesso = usr.TipoDeAcesso,
                                          DataInclusao = usr.DataInclusao
                                      }).ToList();

            if (usuarios.Count > 0)
            {
                Usuario oUsuario = usuarios[0];
                ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(oUsuario.ID)).ToList();
                return PartialView(oUsuario);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Alterar(Usuario);

                    Extensions.GravaCookie("MensagemSucesso", "O usuário '" + Usuario.Nome + "' foi atualizado com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = "#" + Url.Action("Index", "Usuario").Substring(1) } });
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

        public ActionResult BuscarUsuarioPorID(string IDUsuario)
        {
            try
            {
                List<Usuario> usuarios = (from usr in UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDUsuario)).ToList()
                                          join emp in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usr.ID equals emp.ID
                                          join dep in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usr.ID equals dep.ID
                                          select new Usuario()
                                          {
                                              ID = usr.ID,
                                              Nome = usr.Nome,
                                              Login = usr.Login,
                                              CPF = usr.CPF,
                                              Email = usr.Email,
                                              TipoDeAcesso = usr.TipoDeAcesso,
                                              DataInclusao = usr.DataInclusao,
                                          }).ToList();

                if (usuarios.Count < 1)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Usuário com o ID '" + IDUsuario + "' não encontrado." } });
                }
                else
                {
                    Usuario oUsuario = usuarios[0];
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oUsuario) });
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
        public ActionResult Terminar(string IDUsuario)
        {

            try
            {

                Guid uk = Guid.Parse(IDUsuario);

                Usuario oUsuario = UsuarioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(uk));
                if (oUsuario == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o usuário, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oUsuario.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    UsuarioBusiness.Terminar(oUsuario);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O usuário '" + oUsuario.Nome + "' foi excluído com sucesso." } });
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
        public ActionResult TerminarComRedirect(string IDUsuario)
        {

            try
            {
                Usuario oUsuario = UsuarioBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDUsuario));
                if (oUsuario == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o usuário, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oUsuario.DataExclusao = DateTime.Now;
                    oUsuario.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    UsuarioBusiness.Alterar(oUsuario);

                    Extensions.GravaCookie("MensagemSucesso", "O usuário '" + oUsuario.Nome + "' foi excluído com sucesso.", 10);

                                       
                    return Json(new { resultado = new RetornoJSON() { URL = "#" + Url.Action("Index", "Empresa").Substring(1) } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Pesquisar(VMPesquisaUsuario entidade)
        {
            try
            {

                string sFrom = string.Empty;
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.CPF) &&
                    string.IsNullOrEmpty(entidade.Nome) &&
                    string.IsNullOrEmpty(entidade.Email) &&
                    string.IsNullOrEmpty(entidade.DataCriacao) &&
                    string.IsNullOrEmpty(entidade.UKDepartamento) &&
                    string.IsNullOrEmpty(entidade.UKEmpresa))
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

                if (!string.IsNullOrEmpty(entidade.CPF))
                    sWhere += " and o.CPF = '" + entidade.CPF + "'";

                if (!string.IsNullOrEmpty(entidade.Nome))
                    sWhere += " and Upper(o.Nome) like '" + entidade.Nome.ToUpper().Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.Email))
                    sWhere += " and Upper(o.Email) like '" + entidade.Email.ToUpper().Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.UKDepartamento))
                    sWhere += " AND o.UKDepartamento = '" + entidade.UKDepartamento + "'";

                if (!string.IsNullOrEmpty(entidade.UKEmpresa))
                    sWhere += " and o.UKEmpresa = '" + entidade.UKEmpresa + "'";

                if (!string.IsNullOrEmpty(entidade.DataCriacao) && entidade.DataCriacao.Contains(" - "))
                {
                    string data1 = entidade.DataCriacao.Substring(0, entidade.DataCriacao.IndexOf(" - "));
                    DateTime dt1 = DateTime.ParseExact(data1, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string data2 = entidade.DataCriacao.Substring(entidade.DataCriacao.IndexOf(" - ") + 3);
                    DateTime dt2 = DateTime.ParseExact(data2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    sWhere += " and o.DataInclusao between '" + dt1.ToString("yyyy-MM-dd") + " 00:00:00.001' and '" + dt2.ToString("yyyy-MM-dd") + " 23:59:59.999'";
                }

                string sql = @"select top 100 o.UniqueKey, o.CPF, o.Nome, o.Email, e.NomeFantasia, d.Sigla, d.Codigo, o.TipoDeAcesso, o.DataInclusao
                               from tbusuario o, tbempresa e, tbdepartamento d
                               where o.DataExclusao = '9999-12-31 23:59:59.997' and 
                                     o.UKEmpresa = e.Uniquekey and e.DataExclusao = '9999-12-31 23:59:59.997' and 
                                     o.UKDepartamento = d.UniqueKey and d.DataExclusao = '9999-12-31 23:59:59.997' 
	                                 " + sWhere + @"
                               order by o.Nome";

                List<VMPesquisaUsuario> lista = new List<VMPesquisaUsuario>();
                DataTable result = UsuarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMPesquisaUsuario()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            CPF = row["CPF"].ToString(),
                            Nome = row["Nome"].ToString(),
                            Email = row["Email"].ToString(),
                            DataCriacao = row["DataInclusao"].ToString(),
                            UKDepartamento = row["Sigla"].ToString() + " [" + row["Codigo"].ToString() + "]",
                            UKEmpresa = row["NomeFantasia"].ToString()
                        });
                    }
                }

                return PartialView("_Pesquisar", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }


    }
}