using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISHelpers.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Enums;
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
        public IBaseBusiness<ClassificacaoMedida> ClassificacaoMedidaBusiness { get; set; }

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


        public ActionResult AdicionarTipoDeControle()
        {
            ViewBag.TiposDeControles = TipoDeControleBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao));
            
            //var enumData02 = from EClassificacaoDaMedia e in Enum.GetValues(typeof(EClassificacaoDaMedia))
            //                 select new
            //                 {
            //                     ID = (int)e,
            //                     Name = e.GetDisplayName()
            //                 };
            //ViewBag.EClassificacaoDaMedia = new SelectList(enumData02, "ID", "Name");

            var enumData03 = from EControle e in Enum.GetValues(typeof(EControle))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.GetDisplayName()
                             };
            ViewBag.EControle = new SelectList(enumData03, "ID", "Name");


            ViewBag.EClassificacaoDaMedia = ClassificacaoMedidaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderBy(b=>b.Nome).ToList();


            return PartialView("_AdicionarTipoDeControle");
        }

    }
}