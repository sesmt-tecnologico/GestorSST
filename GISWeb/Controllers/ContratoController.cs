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
            ViewBag.Departametnos = DepartamentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Sigla);

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

                    Extensions.GravaCookie("MensagemSucesso", "O Contrato '" + oContrato.Numero + "' foi atualizado com sucesso.", 10);
                    
                    
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

    }
}