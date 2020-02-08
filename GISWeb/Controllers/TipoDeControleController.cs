using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GISWeb.Controllers
{

    public class TipoDeControleController : BaseController
    {

        #region inject

        [Inject]
        public IBaseBusiness<TipoDeControle> TipoDeControleBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion


        // GET: ControleDeRiscos
        public ActionResult Index()
        {

            ViewBag.TipoControles = TipoDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            ViewBag.TotalControles = TipoDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).Count();

            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.TipoControle = TipoDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Cadastrar(TipoDeControle entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    TipoDeControleBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Tipo de Controle '" + entidade.Descricao + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "TipoDeControle") } });
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