using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades.Veiculo;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;


namespace GISWeb.Controllers.Veiculo
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MovimentacaoVeicularController : BaseController
    {

        #region Inject

        [Inject]
        public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

        [Inject]
        public IBaseBusiness<MovimentacaoVeicular> MovimentacaoVeicularBusiness { get; set; }

        #endregion
        
        // GET: MovimentacaoVeicular
        public ActionResult Index()
        {
           // var Movimentacao = MovimentacaoVeicularBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
           // ViewBag.movimentacao = Movimentacao;

            return View();
        }

        public ActionResult Novo()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(MovimentacaoVeicular oMovimentacao)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var reg = Guid.NewGuid();

                    oMovimentacao.RegistroVeicular = reg;
                    MovimentacaoVeicularBusiness.Inserir(oMovimentacao);

                    Extensions.GravaCookie("MensagemSucesso", "O inicio de movimentação do veículo '" + oMovimentacao.Veiculo + "' foi cadastrada com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaMovimentacao", "MovimentacaoVeicular") } });

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


        public ActionResult ListaMovimentacao()
        {
             var Movimentacao = MovimentacaoVeicularBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
             ViewBag.movimentacao = Movimentacao;


            return View();
        }
    }
}