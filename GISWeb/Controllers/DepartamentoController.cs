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


namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DepartamentoController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public INivelHierarquicoBusiness NivelHierarquicoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult BuscarDepartamentosPorEmpresa(string id)
        {
            ViewBag.UKEmpresa = id;

            ViewBag.Niveis = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(a => a.Nome).ToList();

            List<Departamento> departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpresa.ToString().Equals(id)).ToList();
            return PartialView("_BuscarDepartamentosPorEmpresa", departamentos);
        }

        public ActionResult BuscarDepartamentosTodasEmpresa()
        {
            ViewBag.Niveis = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(a => a.Nome).ToList();

            List<Departamento> departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            return PartialView("_BuscarDepartamentosPorEmpresa", departamentos);
        }

        public ActionResult ListarDepartamentosPorEmpresa(string idEmpresa)
        {

            Guid UK = Guid.Parse(idEmpresa);

            return Json(new { resultado = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(UK)).ToList().OrderBy(p => p.Sigla) });

        }

        public ActionResult BuscarDiretoriaPorOrgao(string ukDepartamento)
        {
            try
            {
                Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ukDepartamento));
                if (dep == null)
                {
                    throw new Exception("Não foi possível encontrar o departamento a partir da identificação.");
                }
                else
                {
                    NivelHierarquico nh = NivelHierarquicoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(dep.UKNivelHierarquico));
                    if (nh == null)
                    {
                        throw new Exception("Não foi possível recuperar o nível hierarquico do departamento selecionado.");
                    }
                    else
                    {
                        if (nh.Nome.Equals("Diretoria"))
                        {
                            return Json(new { resultado = new RetornoJSON() { Conteudo = dep.UniqueKey + "$" + dep.Sigla } });
                        }
                        else if (nh.Nome.Equals("Superintendência"))
                        {
                            Departamento depDir = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(dep.UKDepartamentoVinculado));
                            if (depDir == null)
                            {
                                throw new Exception("Não foi possível encontrar o órgão vinculado a Superintendência selecionada.");
                            }
                            else
                            {
                                return Json(new { resultado = new RetornoJSON() { Conteudo = depDir.UniqueKey + "$" + depDir.Sigla } });
                            }
                        }
                        else if (nh.Nome.Equals("Gerência"))
                        {
                            Departamento depDir = (from dSup in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(dep.UKDepartamentoVinculado)).ToList()
                                                   join dDir in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on dSup.UKDepartamentoVinculado equals dDir.UniqueKey
                                                   select dDir).ToList().FirstOrDefault();
                            if (depDir == null)
                            {
                                throw new Exception("Não foi possível encontrar o órgão vinculado a Gerência selecionada.");
                            }
                            else
                            {
                                return Json(new { resultado = new RetornoJSON() { Conteudo = depDir.UniqueKey + "$" + depDir.Sigla } });
                            }
                        }
                        else
                        {
                            throw new Exception("Nível hierarquico do departamento selecionado não conhecido.");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        public ActionResult Novo(string ukDepartamento = "")
        {
<
           
            Departamento newDep = new Departamento();

            newDep.UKEmpresa = Guid.Parse(ukEmpresa);

           

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(newDep.UKEmpresa)).ToList();


            Empresa emp = EmpresaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(newDep.UKEmpresa));

            if (emp != null)
                ViewBag.Empresa = emp.NomeFantasia;
            else
                ViewBag.Empresa = string.Empty;

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.Niveis = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Nome);


            Departamento newDep = new Departamento();
            
            if (string.IsNullOrEmpty(ukDepartamento))
                ViewBag.DepartamentoSuperior = string.Empty;
            else
            {
                newDep.UniqueKey = Guid.Parse(ukDepartamento);
                Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(newDep.UniqueKey));



                newDep.UKDepartamentoVinculado = Guid.Parse(ukDepartamento);

                Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey == newDep.UKDepartamentoVinculado);

                if (dep != null)
                {
                    ViewBag.DepartamentoSuperior = dep.Sigla;
                    
                }
                else
                {
                    ViewBag.DepartamentoSuperior = string.Empty;
                }
            }

            return View(newDep);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Departamento Departamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Departamento.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    DepartamentoBusiness.Inserir(Departamento);

                    Extensions.GravaCookie("MensagemSucesso", "O departamento '" + Departamento.Sigla + "' foi cadastrado com sucesso.", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
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

        public ActionResult Edicao(string UKDepartamento)
        {
<
            Departamento newDep = new Departamento();

            newDep.UniqueKey = Guid.Parse(UKDepartamento);

            Guid UKDep = Guid.Parse(UKDepartamento);


            ViewBag.Niveis = NivelHierarquicoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().OrderBy(b => b.Nome);
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();


            List<Departamento> deps = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            deps.RemoveAll(b => b.UniqueKey.Equals(UKDepartamento));
            ViewBag.Departamentos = deps.OrderBy(a => a.Sigla).ToList();

            Empresa emp = emps.FirstOrDefault(a => a.UniqueKey.Equals(UKEmpresa));
            if (emp != null)
                ViewBag.Empresa = emp.NomeFantasia;
            else
                ViewBag.Empresa = string.Empty;

            Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(newDep.UniqueKey));

            deps.RemoveAll(a => a.UniqueKey.Equals(UKDep));
            ViewBag.Departamentos = deps;
            
            Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));

            if (dep != null)
            {
                if (dep.UKDepartamentoVinculado != null)
                {
                    Departamento dep2 = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey == dep.UKDepartamentoVinculado);
                    if (dep2 != null)
                    {
                        ViewBag.DepartamentoSuperior = dep2.UniqueKey;
                    }
                    else
                    {
                        ViewBag.DepartamentoSuperior = string.Empty;
                    }
                }
                else
                {
                    ViewBag.DepartamentoSuperior = string.Empty;
                }
            }
            else
            {
                ViewBag.DepartamentoSuperior = string.Empty;
            }


            return View(dep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Departamento Departamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Departamento.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    Departamento.UsuarioExclusao = Departamento.UsuarioInclusao;
                    DepartamentoBusiness.Alterar(Departamento);

                    Extensions.GravaCookie("MensagemSucesso", "O departamento '" + Departamento.Sigla + "' foi atualizado com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
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


        public ActionResult LocalizarDepartamentoAutoComplete(string q)
        {

            try
            {
                List<Departamento> departamentos = DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && (a.Codigo.ToUpper().Contains(q.ToUpper()) || a.Sigla.ToUpper().Contains(q.ToUpper()) || a.Descricao.ToUpper().Contains(q.ToUpper()))).ToList();

                List<string> lista = new List<string>();
                foreach (Departamento forn in departamentos)
                {
                    if (string.IsNullOrEmpty(forn.Codigo))
                        lista.Add(forn.Sigla);
                    else
                        lista.Add(forn.Codigo + " - " + forn.Sigla);
                }

                return Json(new { Data = lista });
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
        public ActionResult Terminar(string id)
        {

            try
            {
                Guid UKDep = Guid.Parse(id);
                Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (dep == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o departamento, pois a mesmo não foi localizado." } });

                dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                DepartamentoBusiness.Terminar(dep);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O departamento '" + dep.Sigla + "' foi excluído com sucesso." } });

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