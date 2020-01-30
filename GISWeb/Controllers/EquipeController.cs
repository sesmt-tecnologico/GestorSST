using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
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
    public class EquipeController : BaseController
    {

        [Inject]
        public IEquipeBusiness EquipeBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        // GET: Equipe
        public ActionResult Index()
        {

            ViewBag.Equipe = EquipeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();


            return View();
        }

        public ActionResult Novo()
        {

            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "ID", "NomeFantasia");

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

        public ActionResult Edicao(string id)
        {
            var ID_Equipe = Guid.Parse(id);
            return View(EquipeBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID_Equipe)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Equipe oEquipe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EquipeBusiness.Alterar(oEquipe);

                    Extensions.GravaCookie("MensagemSucesso", "A equipe '" + oEquipe.NomeDaEquipe + "' foi atualizada com sucesso.", 10);



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



            [HttpPost]
            public ActionResult Terminar(string id)
            {
                var ID_Equipe = Guid.Parse(id);
                try
                {
                    Equipe oEquipe = EquipeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(ID_Equipe));

                    if (oEquipe == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a equipe, pois a mesmo não foi localizado." } });
                    }
                    else
                    {
                        oEquipe.DataExclusao = DateTime.Now;
                        oEquipe.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        EquipeBusiness.Excluir(oEquipe);

                        return Json(new { resultado = new RetornoJSON() { Sucesso = "A equipe '" + oEquipe.NomeDaEquipe + "' foi excluída com sucesso." } });
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
    }

}