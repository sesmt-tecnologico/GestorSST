using GISCore.Business.Abstract;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{

    public class ControleDeRiscosController : BaseController
    {

        #region inject

        [Inject]
        public IControleDeRiscoBusiness ControleBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion


        // GET: ControleDeRiscos
        public ActionResult Index()
        {

            ViewBag.Controles = ControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            ViewBag.TotalControles = ControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).Count();

            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.Controle = ControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Cadastrar(ControleDeRiscos entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ControleBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Controle cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "ControleDeRiscos") } });
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