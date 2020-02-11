using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AlocacaoController : BaseController
    {

        #region
        
        [Inject]
        public IEquipeBusiness EquipeBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }
        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_ContratoFornecedor> ContratoFornecedorBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Novo(string id)
        {
            try
            {
                Guid UKAdmissao = Guid.Parse(id);
                Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKAdmissao));

                Alocacao obj = new Alocacao();
                obj.UKAdmissao = oAdmissao.UniqueKey;

                ViewBag.Contratos = (from contForn in ContratoFornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKFornecedor.Equals(oAdmissao.UKEmpresa)).ToList()
                                     join cont in ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on contForn.UKContrato equals cont.UniqueKey
                                     select cont).ToList();

                ViewBag.Cargos = CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
                ViewBag.Estabelecimentos = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
                ViewBag.Equipes = EquipeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(oAdmissao.UKEmpresa)).ToList();

                return PartialView("_Novo", obj);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Alocacao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(entidade.UKAdmissao));
                    if (oAdmissao == null)
                        throw new Exception("Não foi possível encontrar a admissão na base de dados.");

                    if (AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && 
                                                             a.UKAdmissao.Equals(entidade.UKAdmissao) && 
                                                             a.UKCargo.Equals(entidade.UKCargo) &&
                                                             a.UKFuncao.Equals(entidade.UKFuncao)).Count() > 0) {
                        throw new Exception("Já existe uma alocação deste empregado neste cargo e função selecionado.");
                    }
                    else
                    {
                        entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        AlocacaoBusiness.Inserir(entidade);

                        Extensions.GravaCookie("MensagemSucesso", "O empregado foi alocado com sucesso.", 10);

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = oAdmissao.UKEmpregado.ToString() }) } });
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

        [HttpPost]
        [RestritoAAjax]
        public ActionResult Desalocar(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Não foi possível localizar a identificação da alocação para prosseguir com a operação.");

                Guid UKAlocacao = Guid.Parse(id);
                Alocacao al = AlocacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKAlocacao));
                if (al == null)
                    throw new Exception("Não foi possível encontrar a alocação na base de dados.");

                Admissao ad = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(al.UKAdmissao));
                if (ad == null)
                    throw new Exception("Não foi possível encontrar a admissão onde o empregado está alocado na base de dados.");

                al.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                AlocacaoBusiness.Terminar(al);

                Extensions.GravaCookie("MensagemSucesso", "O empregado foi desalocado com sucesso.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = ad.UKEmpregado.ToString() }) } });
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
