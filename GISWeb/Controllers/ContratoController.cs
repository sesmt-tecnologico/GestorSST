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
using GISHelpers.Utils;

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

            EdicaoContratoViewModel obj = null;

            Contrato oContrato = ContratoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(Guid));
            if (oContrato != null)
            {

                ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
                ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList().OrderBy(a => a.NomeFantasia);

                obj = new EdicaoContratoViewModel()
                {
                    ID = oContrato.ID.ToString(),
                    Numero = oContrato.Numero,
                    Descricao = oContrato.Descricao,
                    DataInicio = oContrato.DataInicio,
                    DataFim = oContrato.DataFim
                };

                REL_ContratoFornecedor rel1 = REL_ContratoFornecedorBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKContrato.Equals(oContrato.UniqueKey) && a.TipoContratoFornecedor == GISModel.Enums.ETipoContratoFornecedor.Contratada);
                obj.UKFornecedor = rel1.UKFornecedor.ToString();

                
                List<REL_DepartamentoContrato> relsDep = REL_DepartamentoContratoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.IDContrato.Equals(oContrato.UniqueKey)).ToList();
                if (relsDep?.Count > 0)
                {
                    obj.Departamento = new List<string>();
                    foreach (REL_DepartamentoContrato item in relsDep)
                    {
                        obj.Departamento.Add(item.IDDepartamento.ToString());
                    }
                }

                List<REL_ContratoFornecedor> relsSub = REL_ContratoFornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && 
                                                                                                          a.UKContrato.Equals(oContrato.UniqueKey) && 
                                                                                                          a.TipoContratoFornecedor == GISModel.Enums.ETipoContratoFornecedor.SubContratada).ToList();
                if (relsSub?.Count > 0)
                {
                    obj.SubContratadas = new List<string>();
                    foreach (REL_ContratoFornecedor item in relsSub)
                    {
                        obj.SubContratadas.Add(item.UKFornecedor.ToString());
                    }
                }

                return View(obj);
            }

            


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(EdicaoContratoViewModel entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Contrato objBanco = ContratoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.ToString().Equals(entidade.ID));
                    if (objBanco == null)
                    {
                        throw new Exception("Não foi possível localizar o contrato na base de dados.");
                    }
                    else
                    {
                        if (!entidade.Numero.Equals(objBanco.Numero) ||
                            !entidade.Descricao.Equals(objBanco.Descricao) ||
                            !entidade.DataInicio.Equals(objBanco.DataInicio) ||
                            !entidade.DataFim.Equals(objBanco.DataFim))
                        {
                            objBanco.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            ContratoBusiness.Terminar(objBanco);


                            Contrato newContrato = new Contrato()
                            {
                                UniqueKey = objBanco.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                Numero = entidade.Numero,
                                Descricao = entidade.Descricao,
                                DataInicio = entidade.DataInicio,
                                DataFim = entidade.DataFim
                            };
                            ContratoBusiness.Inserir(newContrato);
                        }




                        REL_ContratoFornecedor rel1 = REL_ContratoFornecedorBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKContrato.Equals(objBanco.UniqueKey) && a.TipoContratoFornecedor == GISModel.Enums.ETipoContratoFornecedor.Contratada);
                        if (!entidade.UKFornecedor.Equals(rel1.UKFornecedor.ToString())) {

                            rel1.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            REL_ContratoFornecedorBusiness.Terminar(rel1);


                            REL_ContratoFornecedor newRel1 = new REL_ContratoFornecedor()
                            {
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                UKContrato = objBanco.UniqueKey,
                                UKFornecedor = Guid.Parse(entidade.UKFornecedor),
                                TipoContratoFornecedor = GISModel.Enums.ETipoContratoFornecedor.Contratada
                            };
                            REL_ContratoFornecedorBusiness.Inserir(newRel1);
                        }




                        List<REL_DepartamentoContrato> relsDep = REL_DepartamentoContratoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && 
                                                                                                                      a.IDContrato.Equals(objBanco.UniqueKey)).ToList();


                    Extensions.GravaCookie("MensagemSucesso", "O Contrato '" + entidade.Numero + "' foi atualizado com sucesso.", 10);
                    
                    

                        //Primeira verificação, o que veio da web e não está no banco
                        if (entidade?.Departamento?.Count > 0)
                        {
                            foreach (string dep in entidade.Departamento)
                            {
                                if (relsDep.Where(a => a.IDDepartamento.ToString().Equals(dep)).Count() == 0)
                                {
                                    //Dep não está nos departamentos do banco de dados, logo, inserir

                                    REL_DepartamentoContratoBusiness.Inserir(new REL_DepartamentoContrato()
                                    {
                                        IDContrato = objBanco.UniqueKey,
                                        IDDepartamento = Guid.Parse(dep),
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }


                        //Segunda verificação, o que veio do banco que não está na web
                        if (relsDep?.Count > 0)
                        {
                            foreach (REL_DepartamentoContrato item in relsDep)
                            {
                                if (entidade.Departamento.Where(a => a.Equals(item.IDDepartamento.ToString())).Count() == 0)
                                {
                                    //rel do banco não está entre os valores vindos da web, logo, terminar

                                    item.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    REL_DepartamentoContratoBusiness.Terminar(item);
                                }
                            }
                        }



                        List<REL_ContratoFornecedor> relsSub = REL_ContratoFornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                                                                                                  a.UKContrato.Equals(objBanco.UniqueKey) &&
                                                                                                                  a.TipoContratoFornecedor == GISModel.Enums.ETipoContratoFornecedor.SubContratada).ToList();


                        //Primeira verificação, o que veio da web e não está no banco
                        if (entidade?.SubContratadas?.Count > 0)
                        {
                            foreach (string sub in entidade.SubContratadas)
                            {
                                if (relsSub.Where(a => a.UKFornecedor.ToString().Equals(sub)).Count() == 0)
                                {
                                    //Sub não está nas subcontratadas do banco de dados, logo, inserir

                                    REL_ContratoFornecedorBusiness.Inserir(new REL_ContratoFornecedor()
                                    {
                                        UKContrato = objBanco.UniqueKey,
                                        UKFornecedor = Guid.Parse(sub),
                                        TipoContratoFornecedor = GISModel.Enums.ETipoContratoFornecedor.SubContratada,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }

                        if (relsSub?.Count > 0)
                        {
                            foreach (REL_ContratoFornecedor item in relsSub)
                            {
                                if (entidade.SubContratadas.Where(a => a.Equals(item.UKFornecedor.ToString())).Count() == 0)
                                {
                                    //rel do banco não está entre os valores vindos da web, logo, terminar

                                    item.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                                    REL_ContratoFornecedorBusiness.Terminar(item);
                                }
                            }
                        }
                        
                    }

                    Extensions.GravaCookie("MensagemSucesso", "O Contrato '" + entidade.Numero + "' foi atualizado com sucesso.", 10);


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
                Contrato oContrato = ContratoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(Guid));
                if (oContrato == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Contrato, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oContrato.DataExclusao = DateTime.Now;
                    oContrato.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ContratoBusiness.Alterar(oContrato);

                    List<REL_ContratoFornecedor> fornecedores = REL_ContratoFornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKContrato.Equals(Guid)).ToList();
                    if (fornecedores?.Count > 0)
                    {
                        foreach (REL_ContratoFornecedor rel in fornecedores)
                        {
                            rel.DataExclusao = DateTime.Now;
                            rel.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                            REL_ContratoFornecedorBusiness.Alterar(rel);
                        }
                    }

                    REL_DepartamentoContrato dep = REL_DepartamentoContratoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.IDContrato.Equals(Guid));
                    if (dep != null)
                    {
                        dep.DataExclusao = DateTime.Now;
                        dep.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        REL_DepartamentoContratoBusiness.Alterar(dep);
                    }

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
            try
            {

                string sFrom = string.Empty;
                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.DataFim) &&
                    string.IsNullOrEmpty(entidade.DataInicio) &&
                    string.IsNullOrEmpty(entidade.Descricao) &&
                    string.IsNullOrEmpty(entidade.UKDepartamento) &&
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

                if (!string.IsNullOrEmpty(entidade.UKDepartamento))
                {
                    sFrom = ", REL_DepartamentoContrato r2 ";
                    sWhere += " and o.UniqueKey = r2.IDContrato and r2.UsuarioExclusao is null and r2.IDDepartamento = '" + entidade.UKDepartamento + "'";
                }

                if (!string.IsNullOrEmpty(entidade.UKSubContratada))
                {
                    sFrom += ", REL_ContratoFornecedor r3 ";
                    sWhere += " and o.UniqueKey = r3.UKContrato and r3.TipoContratoFornecedor = 1 and r3.UsuarioExclusao is null and r3.UKFornecedor = '" + entidade.UKSubContratada + "'";
                }

                string sql = @"select top 100 o.UniqueKey, o.Numero, o.DataInicio, o.DataFim,
	                                   (select STRING_AGG(d.Sigla, ',') WITHIN GROUP (ORDER BY d.Sigla) 
		                                from REL_DepartamentoContrato r1, tbDepartamento d 
		                                where r1.DataExclusao = '9999-12-31 23:59:59.997' and d.DataExclusao = '9999-12-31 23:59:59.997' and 
			                                  r1.IDContrato = o.UniqueKey and r1.IDDepartamento = d.UniqueKey) as Departamentos, 
		                                f.NomeFantasia,
		                                (select STRING_AGG(f.NomeFantasia, ',') WITHIN GROUP (ORDER BY f.NomeFantasia)   
		                                 from REL_ContratoFornecedor r2, tbFornecedor f
		                                 WHERE r2.UKContrato = o.UniqueKey and r2.DataExclusao = '9999-12-31 23:59:59.997' and
			                                   r2.UKFornecedor = f.UniqueKey and f.DataExclusao = '9999-12-31 23:59:59.997' and
			                                   r2.TipoContratoFornecedor = 1) as SubContratadas
                               from tbcontrato o, REL_ContratoFornecedor r1, tbFornecedor f " + sFrom + @"
                               where o.DataExclusao = '9999-12-31 23:59:59.997' and r1.DataExclusao = '9999-12-31 23:59:59.997' and
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