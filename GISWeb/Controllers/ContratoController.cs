using GISCore.Business.Abstract;
using GISModel.DTO.Contrato;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

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
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }


        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosBusiness { get; set; }


        #endregion
        // GET: TipoDeRisco
        public ActionResult Index()
        {
            ViewBag.Contrato = ContratoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.DescricaoContrato);

            return View();
        }


        public ActionResult Novo()
        {
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "UniqueKey", "Sigla");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(NovoContratoViewModel oContrato)
        {
            
            

            if (ModelState.IsValid)
            {
                try
                {
                    Contrato objContrato = new Contrato()
                    {
                        UniqueKey = Guid.NewGuid(),
                        NumeroContrato = oContrato.NumeroContrato,
                        DescricaoContrato = oContrato.DescricaoContrato,
                        DataInicio = oContrato.DataInicio,
                        DataFim = oContrato.DataFim

                    };
                    ContratoBusiness.Inserir(objContrato);

                    if (oContrato.Departamento !=null)
                    {
                        foreach (string Dep in oContrato.Departamento)
                        {


                            REL_DepartamentoContrato objDepContrato = new REL_DepartamentoContrato()
                            {
                                UniqueKey = Guid.NewGuid(),
                                IDContrato = objContrato.UniqueKey,
                                IDDepartamento = Guid.Parse(Dep), 
                                UsuarioInclusao = objContrato.UsuarioInclusao


                                };
                                REL_DepartamentoContratoBusiness.Inserir(objDepContrato);

                           
                        }
                     
                    }
                    



                    TempData["MensagemSucesso"] = "O Contrato '" + oContrato.DescricaoContrato + "' foi cadastrado com sucesso!";

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

                    TempData["MensagemSucesso"] = "O Contrato '" + oContrato.DescricaoContrato + "' foi atualizado com sucesso.";

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
                    oContrato.UsuarioExclusao = "LoginTeste";
                    ContratoBusiness.Alterar(oContrato);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O Contrato '" + oContrato.DescricaoContrato + "' foi excluído com sucesso." } });
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




        public RetornoJSON TratarRetornoValidacaoToJSON()
        {

            string msgAlerta = string.Empty;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (System.Web.Mvc.ModelError i in item.Errors)
                    {
                        if (!string.IsNullOrEmpty(i.ErrorMessage))
                            msgAlerta += i.ErrorMessage;
                        else
                            msgAlerta += i.Exception.Message;
                    }
                }
            }

            return new RetornoJSON()
            {
                Alerta = msgAlerta,
                Erro = string.Empty,
                Sucesso = string.Empty
            };

        }



    }
}