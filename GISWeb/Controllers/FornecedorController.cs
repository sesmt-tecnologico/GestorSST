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

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FornecedorController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<Fornecedor> FornecedorBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.Fornecedor = FornecedorBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Novo()
        {

           
            return View();
        }


        public ActionResult Edicao(string id)
        {
            Guid Guid = Guid.Parse(id);
            return View(FornecedorBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Fornecedor entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FornecedorBusiness.Inserir(entidade);

                    TempData["MensagemSucesso"] = "O fornecedor '" + entidade.NomeFantasia + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Fornecedor") } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Fornecedor entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Fornecedor temp = FornecedorBusiness.Consulta.FirstOrDefault(p=>string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(entidade.UniqueKey));
                    if (temp == null)
                    {
                        throw new Exception("Não foi possível encontrar o Fornecedor através da identificação fornecida.");
                    }
                    else
                    {
                        temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        temp.DataExclusao = DateTime.Now;
                        FornecedorBusiness.Alterar(temp);

                        entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        entidade.DataInclusao = temp.DataInclusao;
                        FornecedorBusiness.Inserir(entidade);
                    }

                   

                    TempData["MensagemSucesso"] = "O forncedor '" + entidade.NomeFantasia + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Fornecedor") } });
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
            Guid Guid = Guid.Parse(id);
            try
            {
                Fornecedor oFornecedor = FornecedorBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(Guid));
                if (oFornecedor == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o fornecedor, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oFornecedor.DataExclusao = DateTime.Now;
                    oFornecedor.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FornecedorBusiness.Alterar(oFornecedor);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O fornecedor '" + oFornecedor.NomeFantasia + "' foi excluído com sucesso." } });
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

        [HttpPost]
        public ActionResult TerminarComRedirect(string IDEmpresa)
        {

            try
            {
                Fornecedor oFornecedor = FornecedorBusiness.Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IDEmpresa));
                if (oFornecedor == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o fornecedor, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oFornecedor.DataExclusao = DateTime.Now;
                    oFornecedor.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FornecedorBusiness.Alterar(oFornecedor);

                    TempData["MensagemSucesso"] = "O fornecedor '" + oFornecedor.NomeFantasia + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empresa") } });
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