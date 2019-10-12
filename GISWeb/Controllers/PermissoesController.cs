using GISCore.Business.Abstract;
using GISModel.DTO.Permissoes;
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

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PermissoesController : BaseController
    {

        #region Inject

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(a => a.Sigla);

            return View();
        }

        [HttpPost]
        public ActionResult SalvarPermissoes(bool Acao, string Perfil, string UIDsUsuarios, string Config)
        {
            try
            {
                if (Acao)
                {
                    //Incluir permissão
                    if (UIDsUsuarios.Contains("|"))
                    {
                        foreach (string IDUsuario in UIDsUsuarios.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(IDUsuario))
                            {
                                UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() {
                                    UKUsuario = Guid.Parse(IDUsuario),
                                    UKPerfil = Guid.Parse(Perfil),
                                    UKConfig = Guid.Parse(Config), UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                });
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() {
                            UKUsuario = Guid.Parse(UIDsUsuarios),
                            UKPerfil = Guid.Parse(Perfil),
                            UKConfig = Guid.Parse(Config),
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        });
                    }
                }
                else
                {
                    //Remover permissão
                    if (UIDsUsuarios.Contains("|"))
                    {
                        foreach (string IDUsuario in UIDsUsuarios.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(IDUsuario))
                            {
                                UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() {
                                    UKUsuario = Guid.Parse(IDUsuario),
                                    UKPerfil = Guid.Parse(Perfil),
                                    UKConfig = Guid.Parse(Config),
                                    DataExclusao = DateTime.Now,
                                    UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                });
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() {
                            UKUsuario = Guid.Parse(UIDsUsuarios),
                            UKPerfil = Guid.Parse(Perfil),
                            UKConfig = Guid.Parse(Config),
                            DataExclusao = DateTime.Now,
                            UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        });
                    }
                }

                return Json(new { resultado = new RetornoJSON() { } });
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
        public ActionResult BuscarUsuariosPorEmpresa(string id)
        {
            try
            {

                ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                Guid UKEmpresa = Guid.Parse(id);
                List<Usuario> lUsuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(UKEmpresa)).ToList();

                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in lUsuarios)
                {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel()
                    {
                        IDUsuario = iUsr.UniqueKey,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join emp in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKConfig equals emp.UniqueKey
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                  where usuarioperfil.UKUsuario.Equals(iUsr.UniqueKey)
                                  select new Perfil { Nome = perfil.Nome, UniqueKey = perfil.UniqueKey };

                    oUsrPerfViewModel.Perfis = lPerfis.ToList();

                    lUsuariosPerfis.Add(oUsrPerfViewModel);

                }

                return Json(new { data = RenderRazorViewToString("_UsuariosPerfis", lUsuariosPerfis), usuarios = lUsuariosPerfis.Count, colunas = ViewBag.Perfis.Count + 2 });
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
        public ActionResult BuscarUsuariosPorDepartamento(string id)
        {
            try
            {
                ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                Guid UKDepartamento = Guid.Parse(id);

                List<Usuario> lUsuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKDepartamento.Equals(UKDepartamento)).ToList();

                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in lUsuarios)
                {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel()
                    {
                        IDUsuario = iUsr.UniqueKey,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join dep in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKConfig equals dep.UniqueKey
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                  where usuarioperfil.UKUsuario.Equals(iUsr.UniqueKey)
                                  select new Perfil { Nome = perfil.Nome, UniqueKey = perfil.UniqueKey };

                    oUsrPerfViewModel.Perfis = lPerfis.ToList();

                    lUsuariosPerfis.Add(oUsrPerfViewModel);

                }

                return Json(new { data = RenderRazorViewToString("_UsuariosPerfis", lUsuariosPerfis), usuarios = lUsuariosPerfis.Count, colunas = ViewBag.Perfis.Count + 2 });
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

    }
}