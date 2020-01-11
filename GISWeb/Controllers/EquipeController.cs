using GISCore.Business.Abstract;
using GISCore.Business.Concrete;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{
    

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EquipeController : BaseController
    {

        [Inject]
        public IEquipeBusiness EquipeBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        // GET: Equipe
        public ActionResult Index()
        {

            ViewBag.Equipe = EquipeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

          
            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(),"ID","NomeFantasia");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Equipe oEquipe)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    EquipeBusiness.Inserir(oEquipe);

                    Extensions.GravaCookie("MensagemSucesso", "A Equipe '" + oEquipe.NomeDaEquipe + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Equipe") } });

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