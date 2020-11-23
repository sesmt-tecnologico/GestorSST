using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AtividadeEquipeController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Atividade> AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Equipe> EquipeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Admissao> AdmissaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AtividadeEquipe> REL_AtividadeEquipeBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: AtividadeEquipe
        public ActionResult Index()
        {

           
            var oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(a=>string.IsNullOrEmpty(a.UsuarioExclusao)
                                  && a.Empregado.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

            var oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                && a.UniqueKey.Equals(oAdmissao.UKEmpresa));

            ViewBag.Atividade = (from a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                join ra in REL_AtividadeEquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                on a.UniqueKey equals ra.UKAtividade
                                where ra.UKEmpresa.Equals(oEmpresa.UniqueKey)
                                select a
                                 ).OrderBy(a=>a.Descricao);

           return View();
        }


        public ActionResult Novo(string UKEmpresa)
        {
                      

            Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                  && a.Empregado.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

            Empresa oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                && a.UniqueKey.Equals(oAdmissao.UKEmpresa));

            ViewBag.Equipe = (from e in EquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                && a.UKEmpresa.Equals(oEmpresa.UniqueKey))
                                select e
                                );

            ViewBag.UkEmpresa = oEmpresa.UniqueKey;






            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Atividade oAtividade, string EmpID, string oEquipe)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    oAtividade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    AtividadeBusiness.Inserir(oAtividade);                   

                    var aAtividade = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(oAtividade.Descricao));
                                       

                    REL_AtividadeEquipe oAtividadeEquipe = new REL_AtividadeEquipe()
                    {

                    UKEmpresa = Guid.Parse(EmpID),
                    UKEquipe = Guid.Parse(oEquipe),
                    UKAtividade = aAtividade.UniqueKey
                    };                   

                    REL_AtividadeEquipeBusiness.Inserir(oAtividadeEquipe);


                    Extensions.GravaCookie("MensagemSucesso", "A atividade '" + oAtividade.Descricao + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "AtividadeEquipe") } });

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