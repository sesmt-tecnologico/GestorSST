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

            var equipes = (from equipe in EquipeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          join emp in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on equipe.UKEmpresa equals emp.UniqueKey
                          select new Equipe { UniqueKey = equipe.UniqueKey,
                                              NomeDaEquipe = equipe.NomeDaEquipe,
                                              Empresa = new Empresa() { 
                                                    NomeFantasia = emp.NomeFantasia
                                              },
                                              DataInclusao = equipe.DataInclusao,
                                              UsuarioInclusao = equipe.UsuarioInclusao,
                                              ResumoAtividade = equipe.ResumoAtividade
                          }).OrderBy(a => a.NomeDaEquipe).ToList();

            ViewBag.Equipe = equipes;

            return View();
        }

        public ActionResult Novo()
        {
            ViewBag.Empresa = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

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
                    oEquipe.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
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
            var UK = Guid.Parse(id);
            return View(EquipeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UK)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Equipe entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Equipe obj = EquipeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (obj == null)
                        throw new Exception("A equipe a ser atualizada não foi encontrada na base de dados.");

                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EquipeBusiness.Terminar(obj);

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EquipeBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "A equipe '" + entidade.NomeDaEquipe + "' foi atualizada com sucesso.", 10);


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
            var UK = Guid.Parse(id);
            try
            {
                Equipe oEquipe = EquipeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UK));
                if (oEquipe == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a equipe, pois a mesmo não foi localizado." } });

                oEquipe.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                EquipeBusiness.Terminar(oEquipe);

                Extensions.GravaCookie("MensagemSucesso", "A equipe '" + oEquipe.NomeDaEquipe + "' foi excluída com sucesso.", 10);

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


        
    }
}