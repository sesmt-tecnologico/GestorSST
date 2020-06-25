using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISModel.DTO.Workflow;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using GISModel.DTO.Shared;
using System.Collections.Generic;
using GISWeb.Infraestrutura.Filters;
using System.Web.SessionState;
using System.Data;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class WorkflowController : BaseController
    {
        #region inject

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDocumentosPessoalBusiness DocumentoPessoalBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DocumentosAlocados> REL_DocumentosAlocadosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Workflow> WorkflowBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        // GET: Workflow
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult WorkfolwAnalise(string UKAlocacao, string UKREL_DocAloc, string status)
        {

            Guid UKaloc = Guid.Parse(UKAlocacao);

            ViewBag.UKAlocacao = UKAlocacao;

            ViewBag.UKREL = UKREL_DocAloc;

            return View("_VincularAnalise");
        }

        public ActionResult ConfirmaAnalise(string UKAlocacao, string UKREL_DocAloc, string Coment, string status)
        {
            Guid Aloc = Guid.Parse(UKAlocacao);
            Guid Rel = Guid.Parse(UKREL_DocAloc);


           var oSql = @" select al.UKAdmissao, adm.UniqueKey, adm.UKempregado as ukemp from tbAlocacao al
            join tbAdmissao adm on al.UkAdmissao = adm.UniqueKey
            where al.UniqueKey = '" + UKAlocacao + @"'";

            DataTable result = AdmissaoBusiness.GetDataTable(oSql);

            string ukEmp = "";

            if (result.Rows.Count > 0)
            {
               
                foreach (DataRow row in result.Rows)
                {
                    if (ukEmp == "")
                    {
                        ukEmp = row["ukemp"].ToString();
                    }

                }

            }

            Guid ukempreg = Guid.Parse(ukEmp);

                if (ModelState.IsValid)
            {
                try
                {
                    REL_DocumentosAlocados oRelDoc = REL_DocumentosAlocadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(Rel));

                    oRelDoc.Posicao = Convert.ToInt32(status);
                    REL_DocumentosAlocadosBusiness.Alterar(oRelDoc);
                         

                    Workflow wor = new Workflow();

                    wor.UKAlocacao = Aloc;
                    wor.UKREL_DocAloc = Rel;
                    wor.Comentarios = Coment;
                    wor.Status = Convert.ToInt32(status);
                    wor.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    WorkflowBusiness.Inserir(wor);

                    Extensions.GravaCookie("MensagemSucesso", "Solicitação enviada com sucesso", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Ged", new { id = ukEmp.ToString() }) } }) ;
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
               
           public ActionResult PesquisaWorkflow(string UKAlocacao, string UKREL_DocAloc)
            {

            Guid aloc = Guid.Parse(UKAlocacao);
            Guid rel = Guid.Parse(UKREL_DocAloc);

            //var oWork = WorkflowBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) &&
            //            (a.UKAlocacao.Equals(aloc)) && (a.UKREL_DocAloc.Equals(rel))).OrderByDescending(a => a.DataInclusao).ToList();


        var workdoc = from w in WorkflowBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          join r in REL_DocumentosAlocadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on w.UKREL_DocAloc equals r.UniqueKey
                          join d in DocumentoPessoalBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on r.UKDocumento equals d.UniqueKey
                          where w.UKAlocacao.Equals(aloc)
                          select new PesquisaWorkflowViewModel()
                          {
                              UsuarioInclusao = w.UsuarioInclusao,
                              Data = w.DataInclusao,
                              Comentarios = w.Comentarios,
                              status = w.Status,
                              Documento = d.NomeDocumento,
                              IDWorkflow = w.ID

                          };


            List<PesquisaWorkflowViewModel> oWork = workdoc.ToList();

            ViewBag.oWork = oWork.OrderByDescending(a=>a.Data);
            



            var emp = from a in AdmissaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      join al in AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      on a.UniqueKey equals al.UKAdmissao  
                      join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      on a.UKEmpregado equals e.UniqueKey
                      join em in EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      on a.UKEmpresa equals em.UniqueKey
                      where al.UniqueKey.Equals(aloc)
                      select new Admissao()
                      {
                           DataAdmissao = a.DataAdmissao,

                          Empregado = new Empregado
                          {
                              Nome = e.Nome
                          },
                          Empresa = new Empresa
                          {
                              NomeFantasia = em.NomeFantasia
                          }

                      };

            List<string> lista = new List<string>();

            ViewBag.Lista = emp.ToList();


                return View("_ExibirWorkflow");
            }

        public ActionResult ListaDocsAnalise()
        {


            ViewBag.Total = "154";


            return View("_Totais");
        }



    }

    
}