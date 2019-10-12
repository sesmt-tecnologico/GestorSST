using GISCore.Business.Abstract;
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
    public class REL_DepartamentoContratoController : BaseController
    {

        #region Inject
        [Inject]
        public IBaseBusiness<Fornecedor> FornecedorBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DepartamentoContrato> REL_DepartamentoContrato { get; set; }

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
            ViewBag.Depatamento = DepartamentoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Descricao);

            return View();
        }


        public ActionResult Novo(string id)
        {

            Guid Guid = Guid.Parse(id);

            ViewBag.Guid = Guid;

            ViewBag.Fornecedor = new SelectList(FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "NomeFantasia");

            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "Sigla");


            ViewBag.Contrato = new SelectList(ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "NumeroContrato");



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(REL_DepartamentoContrato oContrato)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    REL_DepartamentoContrato.Inserir(oContrato);

                    TempData["MensagemSucesso"] = "O Contrato foi cadastrado com sucesso!";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "REL_DepartamentoContrato") } });

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


        public ActionResult Detalhes(string ID)
        {




            return View();
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


        public ActionResult PesquisaDepartamento()
        {






            return View();
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