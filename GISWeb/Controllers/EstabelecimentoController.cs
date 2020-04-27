using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO;
using GISWeb.Infraestrutura.Provider.Concrete;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.DTO.Estabelecimento;
using System.Collections.Generic;
using System.Data;
using GISModel.Enums;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EstabelecimentoController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<REL_EstabelecimentoDepartamento> REL_EstabelecimentoDepartamentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion


        public ActionResult Index()
        {

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }



        public ActionResult Novo()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(EstabelecimentoViewModel entidade)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (entidade?.Departamento?.Count > 0)
                    {

                        Estabelecimento obj = new Estabelecimento()
                        {
                            UniqueKey = Guid.NewGuid(),
                            NomeCompleto = entidade.NomeCompleto,
                            Codigo = entidade.Codigo,
                            Descricao = entidade.Descricao,
                            TipoDeEstabelecimento = entidade.TipoDeEstabelecimento,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login

                        };
                        EstabelecimentoBusiness.Inserir(obj);

                        if (entidade?.Departamento?.Count > 0)
                        {
                            foreach (string dep in entidade.Departamento)
                            {
                                REL_EstabelecimentoDepartamento rel = new REL_EstabelecimentoDepartamento()
                                {

                                    UKEstabelecimento = obj.UniqueKey,
                                    UKDepartamento = Guid.Parse(dep),
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login


                                };
                                REL_EstabelecimentoDepartamentoBusiness.Inserir(rel);
                            }

                        }
                    }
                    else
                    {
                        throw new Exception("É necessário informar pelo menos um departamento para prosseguir com o cadastro do contrato.");
                    }

                    Extensions.GravaCookie("MensagemSucesso", "O Estabelecimento '" + entidade.NomeCompleto + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });

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
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarEstabelecimento(PesquisaEstabelecimentoViewModel entidade)
        {
            try
            {

                string sWhere = string.Empty;

                if (entidade.TipoDeEstabelecimento == null &&
                    string.IsNullOrEmpty(entidade.Codigo) &&
                    string.IsNullOrEmpty(entidade.NomeCompleto) &&
                    string.IsNullOrEmpty(entidade.Descricao) &&
                    entidade.UKDepartamento == Guid.Empty)
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

                if (entidade.TipoDeEstabelecimento != null)
                    sWhere += " and e.TipoDeEstabelecimento = " + ((int)entidade.TipoDeEstabelecimento).ToString() + "";

                if (!string.IsNullOrEmpty(entidade.Codigo))
                    sWhere += " and e.Codigo = '" + entidade.Codigo + "'";

                if (!string.IsNullOrEmpty(entidade.NomeCompleto))
                    sWhere += " and e.NomeCompleto like '" + entidade.NomeCompleto.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.Descricao))
                    sWhere += " AND e.Descricao = '" + entidade.Descricao + "'";

                if (entidade.UKDepartamento != Guid.Empty)
                {
                    sWhere += " and d.UniqueKey = '" + entidade.UKDepartamento + "'";
                }

                string sql = @"select top 100 e.UniqueKey, e.Codigo, e.NomeCompleto, d.UniqueKey as ukDep, d.Sigla as SiglaDep, d.Codigo as CodDep, e.TipoDeEstabelecimento
                               from tbestabelecimento e, REL_EstabelecimentoDepartamento rel, tbDepartamento d
                               where e.UniqueKey = rel.UKEstabelecimento and e.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 rel.UKDepartamento = d.UniqueKey and rel.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 d.DataExclusao = '9999-12-31 23:59:59.997' "  + sWhere + @"
                               order by e.NomeCompleto";

                List<PesquisaEstabelecimentoViewModel> lista = new List<PesquisaEstabelecimentoViewModel>();
                DataTable result = EstabelecimentoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new PesquisaEstabelecimentoViewModel()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            Codigo = row["Codigo"].ToString(),
                            NomeCompleto = row["NomeCompleto"].ToString(),
                            TipoDeEstabelecimento = (TipoEstabelecimento)Enum.Parse(typeof(TipoEstabelecimento), row["TipoDeEstabelecimento"].ToString(), true),
                            UKDepartamento = Guid.Parse(row["ukDep"].ToString()),
                            Departamento = row["SiglaDep"].ToString() + " [" + row["CodDep"].ToString() + "]"
                        });
                    }
                }

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }



        public ActionResult Edicao(Guid Uk)
        {

           //Guid Uk = Guid.Parse(Uk);
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.ToList(), "IDDepartamento", "Sigla");
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(Uk)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Estabelecimento oEstabelecimento)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Estabelecimento dep = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(oEstabelecimento.UniqueKey));
                    if (dep == null)
                    {


                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Estabelecimento, pois a mesmo não foi localizado." } });
                    }

                    else
                    {
                        dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        EstabelecimentoBusiness.Terminar(dep);


                        Estabelecimento objEstab = new Estabelecimento()
                        {
                            UniqueKey = oEstabelecimento.UniqueKey,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                            TipoDeEstabelecimento = oEstabelecimento.TipoDeEstabelecimento,
                            Codigo = oEstabelecimento.Codigo,
                            Descricao = oEstabelecimento.Descricao,
                            NomeCompleto = oEstabelecimento.NomeCompleto
                        };
                        EstabelecimentoBusiness.Inserir(objEstab);


                        Extensions.GravaCookie("MensagemSucesso", "O Estbelecimento '" + oEstabelecimento.NomeCompleto + "' foi atualizado com sucesso.", 10);


                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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

        

        [HttpPost]
        public ActionResult Terminar(string id)
        {
            Guid ID = Guid.Parse(id);
            try
            {
                Estabelecimento dep = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(ID));
                if (dep == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o estabelecimento, pois o mesmo não foi localizado." } });

                dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                EstabelecimentoBusiness.Terminar(dep);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O estabelecimento '" + dep.NomeCompleto + "' foi excluído com sucesso." } });

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














        public ActionResult Lista()
        {
            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult ListarEstabelecimentoPorDepartamento(string idDepartamento)
        {

            return Json(new { resultado = EstabelecimentoBusiness.Consulta.Where(p => p.ID.Equals(idDepartamento)).ToList().OrderBy(p => p.ID) });

        }


    }
}