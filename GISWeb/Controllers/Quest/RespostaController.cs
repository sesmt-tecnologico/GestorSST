using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.Quest;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.Quest
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class RespostaController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Resposta> RespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {

            ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(a => a.NomeFantasia).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pesquisar(Resposta entidade)
        {
            try
            {

                string sFrom = string.Empty;
                string sWhere = string.Empty;

                if ((entidade.UKEmpresa == null || entidade.UKEmpresa == Guid.Empty) &&
                    entidade.DataInclusao == null)
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

                //if (!string.IsNullOrEmpty(entidade.Numero))
                //    sWhere += " and o.Numero like '" + entidade.Numero.Replace("*", "%") + "'";

                //if (!string.IsNullOrEmpty(entidade.DataInicio))
                //    sWhere += " and o.DataInicio = '" + entidade.DataInicio + "'";

                //if (!string.IsNullOrEmpty(entidade.DataFim))
                //    sWhere += " and o.DataFim = '" + entidade.DataFim + "'";

                //if (!string.IsNullOrEmpty(entidade.UKFornecedor))
                //    sWhere += " AND r1.UKFornecedor = '" + entidade.UKFornecedor + "'";

                //if (!string.IsNullOrEmpty(entidade.UKDepartamento))
                //{
                //    sFrom = ", REL_DepartamentoContrato r2 ";
                //    sWhere += " and o.UniqueKey = r2.UKContrato and r2.DataExclusao = '9999-12-31 23:59:59.997' and r2.UKDepartamento = '" + entidade.UKDepartamento + "'";
                //}


                string sql = @"select top 100 o.UniqueKey, o.Numero, o.DataInicio, o.DataFim,
	                                  (     select STRING_AGG(d.Sigla, ',') WITHIN GROUP (ORDER BY d.Sigla) 
		                                    from REL_DepartamentoContrato r1, tbDepartamento d 
		                                    where r1.DataExclusao = '9999-12-31 23:59:59.997' and d.DataExclusao = '9999-12-31 23:59:59.997' and 
			                                      r1.UKContrato = o.UniqueKey and r1.UKDepartamento = d.UniqueKey
                                      ) as Departamentos, 
		                              f.NomeFantasia,
		                              (     select STRING_AGG(f.NomeFantasia, ',') WITHIN GROUP (ORDER BY f.NomeFantasia)   
		                                    from REL_ContratoFornecedor r2, tbFornecedor f
		                                    WHERE r2.UKContrato = o.UniqueKey and r2.DataExclusao = '9999-12-31 23:59:59.997' and
			                                      r2.UKFornecedor = f.UniqueKey and f.DataExclusao = '9999-12-31 23:59:59.997' and
			                                      r2.TipoContratoFornecedor = 1
                                      ) as SubContratadas
                               from tbcontrato o, REL_ContratoFornecedor r1, tbEmpresa f " + sFrom + @"
                               where o.DataExclusao = '9999-12-31 23:59:59.997' and r1.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 o.UniqueKey = r1.UKContrato and r1.TipoContratoFornecedor = 0 and
	                                 r1.UKFornecedor = f.UniqueKey and f.DataExclusao = '9999-12-31 23:59:59.997' " + sWhere + @"
                               order by o.Numero";

                List<Resposta> lista = new List<Resposta>();
                DataTable result = RespostaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        //lista.Add(new VMPesquisaContrato()
                        //{
                        //    UniqueKey = row["UniqueKey"].ToString(),
                        //    Numero = row["Numero"].ToString(),
                        //    DataInicio = row["DataInicio"].ToString(),
                        //    DataFim = row["DataFim"].ToString(),
                        //    UKFornecedor = row["NomeFantasia"].ToString(),
                        //    UKDepartamento = row["Departamentos"].ToString(),
                        //    UKSubContratada = row["SubContratadas"].ToString()
                        //});
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