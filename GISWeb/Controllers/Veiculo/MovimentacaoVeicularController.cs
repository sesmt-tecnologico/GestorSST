using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
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

        [Inject]
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

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
            var data = DateTime.Now.Date;

            

            ViewBag.login = AutorizacaoProvider.UsuarioAutenticado.Login;

            Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
            && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(AutorizacaoProvider.UsuarioAutenticado.Login.ToUpper().Trim())
            || a.UsuarioInclusao.Equals(AutorizacaoProvider.UsuarioAutenticado.Login));

            var movimentacao = MovimentacaoVeicularBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
            && a.UsuarioInclusao.Equals(AutorizacaoProvider.UsuarioAutenticado.Login)).OrderByDescending(a=>a.DataInclusao).ToList();


            MovimentacaoVeicular mov = movimentacao.FirstOrDefault();
            if(mov != null)
            {
                ViewBag.kmSaida = mov.KMChegada;
            


            var data2 = mov.DataInclusao.Date;

            var movimentacao2 = MovimentacaoVeicularBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
            && a.UsuarioInclusao.Equals(AutorizacaoProvider.UsuarioAutenticado.Login) && a.DataInclusao.Equals(data2)).OrderByDescending(a => a.DataInclusao).ToList();


            MovimentacaoVeicular mov2 = movimentacao.FirstOrDefault();


                 if (mov2 == null )
                 {
                    ViewBag.mov = "1";


                    return View();
                 }

                if (mov2.DataInclusao.Date == data && mov2.KMChegada == null)
                {

                ViewBag.mov = "2";

                ViewBag.veiculo = mov2.Veiculo;
                ViewBag.frota = mov2.frota;
                ViewBag.usuario = AutorizacaoProvider.UsuarioAutenticado.Nome;

                Extensions.GravaCookie("MensagemAlerta", "Você deve encerrar a última movimentação!", 10);


                return RedirectToAction("ListaMovimentacao", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login });
                }
                if (mov2.DataInclusao.Date == data && mov2.KMChegada != null)
                {

                    ViewBag.mov = "2";

                    ViewBag.veiculo = mov2.Veiculo;
                    ViewBag.frota = mov2.frota;
                    ViewBag.usuario = AutorizacaoProvider.UsuarioAutenticado.Nome;

                    return View();
                }
            if (mov2.DataInclusao.Date != data && mov2.KMChegada == null)
            {


                Extensions.GravaCookie("MensagemErro", "Você não encerrou a última movimentação do dia Anterior!", 10);

                return RedirectToAction("ListaMovimentacao", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login });

            }
            else
            {
                ViewBag.mov = "3";

                return View();


                    //return RedirectToAction("ListaMovimentacao", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login });
             }

            }
            else
            {
                ViewBag.kmSaida = 0;
                ViewBag.mov = "3";

                return View();
            }





        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(MovimentacaoVeicular oMovimentacao, string Veiculo, string frota)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = AutorizacaoProvider.UsuarioAutenticado.Login;

                    MovimentacaoVeicular movimentacao = MovimentacaoVeicularBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UsuarioInclusao.Equals(user)));

                    

                    var reg = Guid.NewGuid();


                    oMovimentacao.Veiculo = Veiculo;
                    oMovimentacao.frota = frota;
                    oMovimentacao.RegistroVeicular = reg;
                    //oMovimentacao.KMSaida = Convert.ToString(kmChegada);
                    oMovimentacao.UsuarioInclusao = AutorizacaoProvider.UsuarioAutenticado.Login;

                    MovimentacaoVeicularBusiness.Inserir(oMovimentacao);

                    Extensions.GravaCookie("MensagemSucesso", "O inicio de movimentação do veículo '" + oMovimentacao.Veiculo + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaMovimentacao", "MovimentacaoVeicular", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login}) } });
                    
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


        public ActionResult ListaMovimentacao(string usuario)
        {


             var Movimentacao = MovimentacaoVeicularBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
             && a.UsuarioInclusao.Equals(usuario)).ToList();

             ViewBag.movimentacao = Movimentacao.OrderByDescending(a=>a.DataInclusao);

            MovimentacaoVeicular mov = Movimentacao.FirstOrDefault();

            ViewBag.veiculo = mov.Veiculo;
            ViewBag.frota = mov.frota;

            return View();
        }


        public ActionResult Edicao(string UniqueKey)
        {


            Guid UKMov = Guid.Parse(UniqueKey);

            MovimentacaoVeicular movimentacao = MovimentacaoVeicularBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(UKMov)));

            return View("_KMChegada", movimentacao);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(MovimentacaoVeicular pMov)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MovimentacaoVeicular movimentacao = MovimentacaoVeicularBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                    && a.UniqueKey.Equals(pMov.UniqueKey));

                    
                        var kmSaida = Convert.ToInt32(movimentacao.KMSaida);
                        var pMovKMChegado = Convert.ToInt32(pMov.KMChegada);

                    

                    if(pMovKMChegado > kmSaida )
                    {
                        movimentacao.KMChegada = pMov.KMChegada;

                        movimentacao.UsuarioInclusao = AutorizacaoProvider.UsuarioAutenticado.Login;

                        MovimentacaoVeicularBusiness.Alterar(movimentacao);


                        Extensions.GravaCookie("MensagemSucesso", "KM final  '" + pMov.KMChegada + "' foi inserido com sucesso.", 10);

                        //return RedirectToAction("Index");

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaMovimentacao", "MovimentacaoVeicular", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login }) } });


                    }
                    else
                    {
                        Extensions.GravaCookie("MensagemErro", "O KM final '" + pMov.KMChegada + "' deve ser maior que o KM inicial! '" + movimentacao.KMSaida + "' ", 10);

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaMovimentacao", "MovimentacaoVeicular", new { usuario = AutorizacaoProvider.UsuarioAutenticado.Login }) } });

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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }


    }
}