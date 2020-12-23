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

namespace GISWeb.Controllers.AnaliseRisco
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ARInterrompidaController : BaseController
    {
        [Inject]
        public IBaseBusiness<ARInterrompida> ARInterrompidaBusiness { get; set; }


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
                    ARInterrompidaBusiness.Inserir(oARinterrompida);

                    Extensions.GravaCookie("MensagemSucesso", "A Atividade '" + oARinterrompida.Descricao + "' foi cadastrada com sucesso!", 10);



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