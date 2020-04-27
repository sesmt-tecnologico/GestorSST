using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISHelpers.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ClassificacaoMedidaController : BaseController
    {

        #region inject

        [Inject]
        public IBaseBusiness<ClassificacaoMedida> ClassificacaoMedidaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        #endregion


        // GET: ControleDeRiscos
        public ActionResult Index()
        {

            ViewBag.Classificacao = ClassificacaoMedidaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(a => a.Nome).ToList();

            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.Classificacao = ClassificacaoMedidaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Cadastrar(ClassificacaoMedida entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ClassificacaoMedidaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "Classifição da Medida '" + entidade.Nome + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "ClassificacaoMedida") } });
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

        [HttpPost]
        public ActionResult Terminar(string IDclassificacao)
        {
            Guid Guid = Guid.Parse(IDclassificacao);
            try
            {
                ClassificacaoMedida oClassificacao = ClassificacaoMedidaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid));
                if (oClassificacao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a classificação, pois a mesma não foi localizada." } });
                }
                else
                {

                    oClassificacao.DataExclusao = DateTime.Now;
                    oClassificacao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ClassificacaoMedidaBusiness.Alterar(oClassificacao);

                   
                    Extensions.GravaCookie("MensagemSucesso", "A classificação '" + oClassificacao.Nome + "' foi excluída com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "ClassificacaoMedida") } });
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




        //public ActionResult AdicionarClassificacaoMedida()
        //{
        //    ViewBag.Classificacao = ClassificacaoMedidaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao));

        //    var enumData02 = from EClassificacaoDaMedia e in Enum.GetValues(typeof(EClassificacaoDaMedia))
        //                     select new
        //                     {
        //                         ID = (int)e,
        //                         Name = e.GetDisplayName()
        //                     };
        //    ViewBag.EClassificacaoDaMedia = new SelectList(enumData02, "ID", "Name");

        //    var enumData03 = from EControle e in Enum.GetValues(typeof(EControle))
        //                     select new
        //                     {
        //                         ID = (int)e,
        //                         Name = e.GetDisplayName()
        //                     };
        //    ViewBag.EControle = new SelectList(enumData03, "ID", "Name");

        //    return PartialView("_AdicionarTipoDeControle");
        //}

    }
}