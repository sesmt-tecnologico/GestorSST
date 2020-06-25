using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.PCMSO;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.PCMSO;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.PCMSO
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class REL_RiscosExamesController: BaseController
    {
        #region
        [Inject]
        public IBaseBusiness<REL_RiscosExames> REL_RiscosExamesBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Perigo> PerigoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Exames> ExamesBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: REL_RiscosExames
        public ActionResult Index()
        {

            List<VMLExamesRiscos> ListExRiscos = (from rel in REL_RiscosExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                              join p in PerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                              on rel.ukPerigo equals p.UniqueKey
                                              join e in ExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                              on rel.ukExame equals e.UniqueKey
                                              select new VMLExamesRiscos()
                                              {
                                                  TipoExame = rel.TipoExame,
                                                  Perigo = p.Descricao,
                                                  Exame = e.Nome,
                                                  Obrigatoriedade = rel.Obrigariedade


                                              }).ToList();


            ViewBag.ExRiscos = ListExRiscos;

            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.Perigo = PerigoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Template.Equals(true)).ToList().OrderBy(p=>p.Riscos);

            ViewBag.Exame = ExamesBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(p => p.Nome).ToList();

            return View();
        }

        public ActionResult Cadastrar( REL_RiscosExames oRel_Exames)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    oRel_Exames.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    REL_RiscosExamesBusiness.Inserir(oRel_Exames);
                    Extensions.GravaCookie("MensagemSucesso", "Exame e Risco registrado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "REL_RiscosExames") } });

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


    }
}