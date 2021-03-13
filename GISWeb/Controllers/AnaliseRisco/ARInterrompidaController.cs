using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades.AnaliseDeRisco;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.Entidades;

namespace GISWeb.Controllers.AnaliseRisco
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ARInterrompidaController : BaseController
    {
        #region
        [Inject]
        public IBaseBusiness<ARInterrompida> ARInterrompidaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AnaliseDeRiscoEmpregados> REL_AnaliseDeRiscoEmpregadosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<PlanoDeAcao> PlanoDeAcaoBusiness { get; set; }


        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        #endregion

        // GET: ARInterrompida
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]        
        public ActionResult Cadastrar(ARInterrompida oARinterrompida)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    oARinterrompida.Status = "Aberta";
                    oARinterrompida.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ARInterrompidaBusiness.Inserir(oARinterrompida);

                    REL_AnaliseDeRiscoEmpregados rARE = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Registro.Equals(oARinterrompida.Registro));

                    rARE.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Nome;
                    REL_AnaliseDeRiscoEmpregadosBusiness.Alterar(rARE);


                    var oRegis = oARinterrompida.Registro;
                    var oItem = oARinterrompida.Item;
                    var ofato = oARinterrompida.Descricao;

                    PlanoDeAcao oPlanoDeAcao = null;

                    
                        if(oPlanoDeAcao == null)
                        {
                           PlanoDeAcao planoDeAcao = new PlanoDeAcao()
                            {
                                Identificador = Guid.Parse(oRegis),
                                item = oItem,
                                fato = ofato,
                                status = "Aberto",

                            };
                            PlanoDeAcaoBusiness.Inserir(planoDeAcao);
                        }
                    


                    Extensions.GravaCookie("MensagemSucesso", "O Evento '" + oARinterrompida.Descricao + "' gerou um Plano de Ação e a atividade deverá ser interrompida!", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AnaliseDeRisco") } });

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