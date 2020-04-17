using GISCore.Business.Abstract;
using GISCore.Repository.Configuration;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Admissao;
using System.Collections.Generic;
using System.Data;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISHelpers.Utils;
using System.Globalization;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EmpregadoController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        #endregion


        public ActionResult Desktop(string id) {

            if (string.IsNullOrEmpty(id))
            {
                if (CustomAuthorizationProvider.UsuarioAutenticado.Permissoes.Where(a => a.Perfil.Equals("Empregado")).Count() > 0)
                {
                    Empregado emp = EmpregadoBusiness.Consulta.FirstOrDefault(a =>
                                            string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                            a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim()));
                    if (emp != null)
                    {
                        id = emp.UniqueKey.ToString();
                    }
                }
            }

            if (string.IsNullOrEmpty(id))
            {
                return View();
            }

            Guid UK = Guid.Parse(id);

            ViewBag.UKEmp = id;

            Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UK));

            return View(oEmp);
        }



        public ActionResult Novo()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    empregado.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    empregado.Status = "Atualmente sem admissão";
                    EmpregadoBusiness.Inserir(empregado);

                    Extensions.GravaCookie("MensagemSucesso", "O empregado '" + empregado.Nome + "' foi cadastrado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() {  URL = Url.Action("Novo", "Empregado") } });
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


        public ActionResult Pesquisa()
        {

            //ViewBag.Status = new List<string> { "Atualmente admitido", "Já admitido alguma vez", "Atualmente sem admissão" };
            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.Cargos = CargoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            ViewBag.Contratos = ContratoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisaAvancada(PesquisaEmpregadoViewModel entidade)
        {
            try
            {
                if (string.IsNullOrEmpty(entidade.Nome) &&
                    string.IsNullOrEmpty(entidade.CPF) &&
                    entidade.Status == null &&
                    string.IsNullOrEmpty(entidade.Empresa) &&
                    string.IsNullOrEmpty(entidade.Contrato) &&
                    string.IsNullOrEmpty(entidade.DataAdmissao) &&
                    string.IsNullOrEmpty(entidade.Cargo) &&
                    string.IsNullOrEmpty(entidade.Funcao) &&
                    string.IsNullOrEmpty(entidade.Atividade))
                {
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa de empregado.");
                }


                string sWhere = string.Empty;
                string sFrom = string.Empty;

                if (!string.IsNullOrEmpty(entidade.Nome))
                    sWhere += " and upper(e.Nome) like '%" + entidade.Nome.ToUpper() + "%'";

                if (!string.IsNullOrEmpty(entidade.CPF))
                    sWhere += " and e.CPF = '" + entidade.CPF + "'";

                if (entidade.Status != null)
                    sWhere += " and e.Status = '" + entidade.Status.GetDisplayName() + "'";


                if (!string.IsNullOrEmpty(entidade.DataAdmissao) ||
                    !string.IsNullOrEmpty(entidade.Empresa) ||
                    !string.IsNullOrEmpty(entidade.Contrato) ||
                    !string.IsNullOrEmpty(entidade.Cargo) ||
                    !string.IsNullOrEmpty(entidade.Funcao))
                {

                    sFrom += ", tbAdmissao a ";
                    sWhere += " and e.UniqueKey = a.UKEmpregado and a.DataExclusao = '9999-12-31 23:59:59.997' ";

                    if (!string.IsNullOrEmpty(entidade.DataAdmissao))
                    {
                        string data1 = entidade.DataAdmissao.Substring(0, entidade.DataAdmissao.IndexOf(" - "));
                        string data2 = entidade.DataAdmissao.Substring(entidade.DataAdmissao.IndexOf(" - ") + 3);
                        
                        sWhere += @" and a.DataAdmissao between '" + data1 + "' and '" + data2 + "'";
                    }

                    if (!string.IsNullOrEmpty(entidade.Empresa))
                    {
                        sWhere += " and a.UKEmpresa = '" + entidade.Empresa + "' ";
                    }

                    if (!string.IsNullOrEmpty(entidade.Contrato) ||
                        !string.IsNullOrEmpty(entidade.Cargo) ||
                        !string.IsNullOrEmpty(entidade.Funcao))
                    {

                        sFrom += ", tbAlocacao al ";
                        sWhere += " and a.UniqueKey = al.UKAdmissao and al.DataExclusao = '9999-12-31 23:59:59.997' ";

                        if (!string.IsNullOrEmpty(entidade.Contrato))
                        {
                            sWhere += @" and al.UKContrato = '" + entidade.Contrato + "'";
                        }

                        if (!string.IsNullOrEmpty(entidade.Cargo))
                        {
                            sWhere += @" and al.UKCargo = '" + entidade.Cargo + "'";
                        }

                        if (!string.IsNullOrEmpty(entidade.Funcao))
                        {
                            sWhere += @" and al.UKFuncao = '" + entidade.Funcao + "'";
                        }

                    }



                }

                

                string sql = @"select top 100 e.UniqueKey, e.Nome, e.CPF, e.DataNascimento, e.Email, e.Status
                           from tbEmpregado e " + sFrom + @"
                           where e.DataExclusao = '9999-12-31 23:59:59.997' " + sWhere + @"
                           order by e.Nome";

                List<Empregado> lista = new List<Empregado>();
                DataTable result = ContratoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new Empregado()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            Nome = row["Nome"].ToString(),
                            CPF = row["CPF"].ToString(),
                            DataNascimento = row["DataNascimento"].ToString(),
                            Email = row["Email"].ToString(),
                            Status = row["Status"].ToString()
                        });
                    }
                }

                return PartialView("_PesquisaAvancada", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }





        //public ActionResult ListaEmpregadoNaoAdmitido()
        //{
        //   // var ID = Guid.Parse(id);

        //    var listEmp = from e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                  join a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
        //                  on e.UniqueKey equals a.UKEmpregado
        //                  into g
        //                  from NaoAdm in g.DefaultIfEmpty()
        //                  select new AdmissaoViewModel()
        //                  {
        //                      UK_empregado = e.UniqueKey,
        //                      ID = e.ID,
        //                      NomeEmpregado = e.Nome,
        //                      CPF = e.CPF
        //                  };

            
        //    List<AdmissaoViewModel> lista = new List<AdmissaoViewModel>();

        //    lista = listEmp.ToList();

        //    ViewBag.Empregado = lista;

        //    //ViewBag.Empregado = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

        //    return View();
        //}

        //public ActionResult ListaEmpregadoAdmitidoPorEmpresa()
        //{


        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ListaEmpregadoAdmitidoPorEmpresa(PesquisaEmpregadoViewModel entidade)
        //{
        //    try
        //    {

        //        string sWhere = string.Empty;

        //        if (string.IsNullOrEmpty(entidade.NomeEmpregado) &&
        //           string.IsNullOrEmpty(entidade.CPF) &&
        //           string.IsNullOrEmpty(entidade.NomeEmpresa)
        //           )
        //            throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

        //        if (!string.IsNullOrEmpty(entidade.NomeEmpregado))
        //            sWhere += " and e.Nome like '%" + entidade.NomeEmpregado.Replace("*", "%") + "%'";

        //        if (!string.IsNullOrEmpty(entidade.CPF))
        //            sWhere += " and e.CPF = '" + entidade.CPF + "'";

        //        if (!string.IsNullOrEmpty(entidade.NomeEmpresa))
        //            sWhere += " and b.NomeFantasia like '%" + entidade.NomeEmpresa.Replace("*", "%") + "%'";



        //        string sql = @" select e.UniqueKey, e.ID, e.Nome, e.CPF, e.ID, a.IDEmpregado, a.IDEmpresa, b.ID, b.NomeFantasia, a.MaisAdmin from tbEmpregado e, tbAdmissao a, tbEmpresa b
        //                    where e.UniqueKey = a.IDEmpregado and b.ID = a.IDEmpresa" + sWhere +
        //                    @" order by a.IDEmpregado";

                               
                

        //        List<PesquisaEmpregadoViewModel> lista = new List<PesquisaEmpregadoViewModel>();

        //        DataTable result = EmpregadoBusiness.GetDataTable(sql);
        //        if (result.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in result.Rows)
        //            {
        //                lista.Add(new PesquisaEmpregadoViewModel()
        //                {
                            
        //                    UniqueKey = row["UniqueKey"].ToString(),
        //                    idEmpregado = row["IDEmpregado"].ToString(),
        //                    NomeEmpregado = row["Nome"].ToString(),
        //                    CPF = row["CPF"].ToString(),
        //                    NomeEmpresa = row["NomeFantasia"].ToString(),
        //                    justificativa = row["MaisAdmin"].ToString()

        //                }); ;
        //            }
        //        }




        //        return PartialView("_ListaEmpregadoPorEmpresa", lista);
        //    }
        //    catch (Exception ex)
        //    {

        //        if (ex.GetBaseException() == null)
        //        {
        //            return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //        }
        //        else
        //        {
        //            return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //        }
        //    }
                                            
        //}






        public ActionResult Edicao(string id)
        {
            return View(EmpregadoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoBusiness.Alterar(empregado);

                    Extensions.GravaCookie("MensagemSucesso", "O empregado '" + empregado.Nome + "' foi atualizado com sucesso.", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empregado") } });
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



        [HttpPost]
        public ActionResult Terminar(string IDEmpregado)
        {

            try
            {
                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDEmpregado));
                if (oEmpregado == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o empregado, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oEmpregado.DataExclusao = DateTime.Now;
                    //oEmpregado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EmpregadoBusiness.Excluir(oEmpregado);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O empregado '" + oEmpregado.Nome + "' foi excluído com sucesso." } });
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


        public ActionResult Perfil(string id)
        {
            Guid UK = Guid.Parse(id);

            Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UK));

            return View(oEmp);
        }

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

        [HttpPost]
        [Autorizador]
        [DadosUsuario]
        public ActionResult AtualizarFotoEmpregado(string imagemStringBase64, string login)
        {
            try
            {
                login = login.Replace(".", "").Replace("-", "");

                UsuarioBusiness.SalvarAvatar(login, imagemStringBase64, "jpg");
            }
            catch (Exception ex)
            {
                Extensions.GravaCookie("MensagemErro", ex.Message, 2);
            }

            return Json(new { url = Url.Action("Perfil") });
        }

    }
}