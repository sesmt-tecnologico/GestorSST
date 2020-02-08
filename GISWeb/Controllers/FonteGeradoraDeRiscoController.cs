using GISCore.Business.Abstract;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{
    public class FonteGeradoraDeRiscoController : BaseController
    {


        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        // GET: FonteGeradoraDeRisco
        public ActionResult Index()
        {
            ViewBag.Fonte = FonteGeradoraDeRiscoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList();

            ViewBag.ContaFonte = FonteGeradoraDeRiscoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).Count();
            return View();
        }

        public ActionResult Novo()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(FonteGeradoraDeRisco oFonteGeradoraDeRisco)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    FonteGeradoraDeRiscoBusiness.Inserir(oFonteGeradoraDeRisco);

                    Extensions.GravaCookie("MensagemSucesso", "a Fonte Geradora de riscos '" + oFonteGeradoraDeRisco.FonteGeradora + "' foi cadastrado com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FonteGeradoraDeRisco", new { id = oFonteGeradoraDeRisco.ID }) } });

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