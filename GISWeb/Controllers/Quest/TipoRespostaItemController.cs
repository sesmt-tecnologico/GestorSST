using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using GISModel.Entidades.Quest;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.Quest
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TipoRespostaItemController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<TipoRespostaItem> TipoRespostaItemBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Novo(string id)
        {
            
            TipoRespostaItem obj = new TipoRespostaItem()
            {
                UKTipoResposta = Guid.Parse(id)
            };

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(TipoRespostaItem entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.Nome))
                        throw new Exception("Informe o item da resposta de múltipla escolha.");

                    if (entidade.UKTipoResposta == Guid.Empty)
                        throw new Exception("Não foi possível localizar a identificação da resposta.");

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    TipoRespostaItemBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "O item da resposta '" + entidade.Nome + "' foi cadastrado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "TipoResposta") } });
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
            try
            {
                Guid UKDep = Guid.Parse(id);
                TipoRespostaItem temp = TipoRespostaItemBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o item da resposta de múltipla resposta, pois a mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                TipoRespostaItemBusiness.Terminar(temp);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O item da resposta de múltipla escolha '" + temp.Nome + "' foi excluído com sucesso." } });

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