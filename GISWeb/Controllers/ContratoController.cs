using GISCore.Business.Abstract;
using GISModel.DTO.Contrato;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using System.Collections.Generic;
using System.Data;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ContratoController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<REL_DepartamentoContrato> REL_DepartamentoContratoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_ContratoFornecedor> REL_ContratoFornecedorBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Fornecedor> FornecedorBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Sigla);
            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList().OrderBy(a => a.NomeFantasia);
            return View();
        }

        public ActionResult Novo()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList().OrderBy(a => a.NomeFantasia);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(NovoContratoViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (entidade?.Departamento?.Count > 0)
                    {


                        if (entidade?.SubContratadas?.Count > 0 && !string.IsNullOrEmpty(entidade.UKFornecedor))
                        {
                            if (entidade.SubContratadas.Where(a => a.Equals(entidade.UKFornecedor)).Count() > 0)
                            {
                                throw new Exception("Não é possível selecionar o mesmo fornecedor no campo sub-contratada");
                            }
                        }



                        Contrato obj = new Contrato()
                        {
                            UniqueKey = Guid.NewGuid(),
                            Numero = entidade.Numero,
                            Descricao = entidade.Descricao,
                            DataInicio = entidade.DataInicio,
                            DataFim = entidade.DataFim,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };
                        ContratoBusiness.Inserir(obj);

                        if (!string.IsNullOrEmpty(entidade.UKFornecedor))
                        {
                            REL_ContratoFornecedor rel = new REL_ContratoFornecedor()
                            {
                                UKContrato = obj.UniqueKey,
                                UKFornecedor = Guid.Parse(entidade.UKFornecedor),
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                TipoContratoFornecedor = GISModel.Enums.ETipoContratoFornecedor.Contratada
                            };
                            REL_ContratoFornecedorBusiness.Inserir(rel);
                        }

                        if (entidade?.SubContratadas?.Count > 0)
                        {
                            foreach (string sub in entidade.SubContratadas)
                            {
                                REL_ContratoFornecedor rel = new REL_ContratoFornecedor()
                                {
                                    UKContrato = obj.UniqueKey,
                                    UKFornecedor = Guid.Parse(sub),
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                    TipoContratoFornecedor = GISModel.Enums.ETipoContratoFornecedor.SubContratada
                                };
                                REL_ContratoFornecedorBusiness.Inserir(rel);
                            }
                        }
                    
                        foreach (string Dep in entidade.Departamento)
                        {
                            REL_DepartamentoContrato objDepContrato = new REL_DepartamentoContrato()
                            {
                                IDContrato = obj.UniqueKey,
                                IDDepartamento = Guid.Parse(Dep), 
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            };

                            REL_DepartamentoContratoBusiness.Inserir(objDepContrato);
                        }


                    }
                    else
                    {
                        throw new Exception("É necessário informar pelo menos um departamento para prosseguir com o cadastro do contrato.");
                    }

                    Extensions.GravaCookie("MensagemSucesso", "O Contrato '" + entidade.Numero + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Contrato") } });

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

        public ActionResult Edicao(string id)
        {
            Guid Guid = Guid.Parse(id);
             
            return View(ContratoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Contrato oContrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContratoBusiness.Alterar(oContrato);

                    TempData["MensagemSucesso"] = "O Contrato '" + oContrato.Numero + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Contrato") } });
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

        public ActionResult Excluir(string id)
        {
            ViewBag.Contrato = new SelectList(ContratoBusiness.Consulta.ToList(), "IDContrato", "DescricaoContrato");
            return View(ContratoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        public ActionResult Terminar(string IDContrato)
        {
            Guid Guid = Guid.Parse(IDContrato);
            try
            {
                Contrato oContrato = ContratoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid));
                if (oContrato == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Contrato, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oContrato.DataExclusao = DateTime.Now;
                    oContrato.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ContratoBusiness.Alterar(oContrato);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Contrato '" + oContrato.Numero + "' foi excluído com sucesso." } });
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
        public ActionResult Pesquisar(VMPesquisaContrato entidade) {
            try {

                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.DataFim) &&
                    string.IsNullOrEmpty(entidade.DataInicio) &&
                    string.IsNullOrEmpty(entidade.Descricao) &&
                    (entidade.Departamento == null || entidade.Departamento.Count == 0) &&
                    string.IsNullOrEmpty(entidade.Numero) &&
                    string.IsNullOrEmpty(entidade.UKFornecedor) &&
                    string.IsNullOrEmpty(entidade.UKSubContratada))
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

                if (!string.IsNullOrEmpty(entidade.Numero))
                    sWhere += " and o.Numero like '" + entidade.Numero.Replace("*", "%") + "'";

                if (!string.IsNullOrEmpty(entidade.DataInicio))
                    sWhere += " and o.DataInicio = '" + entidade.DataInicio + "'";

                if (!string.IsNullOrEmpty(entidade.DataFim))
                    sWhere += " and o.DataFim = '" + entidade.DataFim + "'";

                if (!string.IsNullOrEmpty(entidade.UKFornecedor))
                    sWhere += " AND r1.UKFornecedor = '" + entidade.UKFornecedor + "'"; 

                string sql = @"select top 100 o.UniqueKey, o.Numero, o.DataInicio, o.DataFim,
	                                   (select STRING_AGG(d.Sigla, ',') WITHIN GROUP (ORDER BY d.Sigla) 
		                                from REL_DepartamentoContrato r1, tbDepartamento d 
		                                where r1.UsuarioExclusao is null and d.UsuarioExclusao is null and 
			                                  r1.IDContrato = o.UniqueKey and r1.IDDepartamento = d.UniqueKey) as Departamentos, 
		                                f.NomeFantasia,
		                                (select STRING_AGG(f.NomeFantasia, ',') WITHIN GROUP (ORDER BY f.NomeFantasia)   
		                                 from REL_ContratoFornecedor r2, tbFornecedor f
		                                 WHERE r2.UKContrato = o.UniqueKey and r2.UsuarioExclusao is null and
			                                   r2.UKFornecedor = f.UniqueKey and f.UsuarioExclusao is null and
			                                   r2.TipoContratoFornecedor = 1) as SubContratadas
                               from tbcontrato o, REL_ContratoFornecedor r1, tbFornecedor f
                               where o.UsuarioExclusao is null and r1.UsuarioExclusao is null and
	                                 o.UniqueKey = r1.UKContrato and r1.TipoContratoFornecedor = 0 and
	                                 r1.UKFornecedor = f.UniqueKey and f.UsuarioExclusao is null " + sWhere + @"
                               order by o.Numero";

                List<VMPesquisaContrato> lista = new List<VMPesquisaContrato>();
                DataTable result = ContratoBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        lista.Add(new VMPesquisaContrato()
                        {
                            UniqueKey = row["UniqueKey"].ToString(),
                            Numero = row["Numero"].ToString(),
                            DataInicio = row["DataInicio"].ToString(),
                            DataFim = row["DataFim"].ToString(),
                            UKFornecedor = row["NomeFantasia"].ToString(),
                            UKDepartamento = row["Departamentos"].ToString(),
                            UKSubContratada = row["SubContratadas"].ToString()
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