using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AtividadeFuncaoLiberadaController : BaseController
    {
        #region

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IAtividadeFuncaoLiberadaBusiness AtividadeFuncaoLiberadaBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        public ActionResult Novo(string id)
        {

            ViewBag.AtiviEstab = new SelectList(AtividadesDoEstabelecimentoBusiness.Consulta, "IDAtividadesDoEstabelecimento", "DescricaoDestaAtividade");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(AtividadesDoEstabelecimento oAtividadesDoEstabelecimentoBusiness, string IDAtividadesDoEstabelecimento)
        {

            oAtividadesDoEstabelecimentoBusiness.ID = Guid.Parse(IDAtividadesDoEstabelecimento);
            //oMedidasDeControleExistentes.IDAtividadeRiscos = AtivRiscoID;

            if (ModelState.IsValid)
            {
                try
                {

                    AtividadesDoEstabelecimentoBusiness.Inserir(oAtividadesDoEstabelecimentoBusiness);

                    Extensions.GravaCookie("MensagemSucesso", "A imagem '" + oAtividadesDoEstabelecimentoBusiness.NomeDaImagem + "'foi cadastrada com sucesso.", 10);

                    

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Novo", "MedidasDeControle", new { id = oAtividadesDoEstabelecimentoBusiness.ID }) } });
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
        public ActionResult SalvarAtividade(bool Acao, string idAtividade, string idAlocacao, string IDFuncao)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (Acao)
                {
                    //Incluir vinculo entre Atividade e Alocação
                    if (idAlocacao.Contains("|"))
                    {
                        foreach (string item in idAlocacao.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                AtividadeFuncaoLiberadaBusiness.Inserir(new AtividadeFuncaoLiberada()
                                {
                                    IDAlocacao = Guid.Parse(item),
                                    IDAtividade = Guid.Parse(idAtividade),
                                    IDFuncao = Guid.Parse(IDFuncao),
                                    //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                                });
                            }
                        }
                    }
                    else
                    {
                        AtividadeFuncaoLiberadaBusiness.Inserir(new AtividadeFuncaoLiberada()
                        {
                            IDAlocacao = Guid.Parse(idAlocacao),
                            IDAtividade = Guid.Parse(idAtividade),
                            IDFuncao = Guid.Parse(IDFuncao),
                            //UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                        });
                    }
                }
                else
                {
                    //Remover vinculo entre Menu e Perfil
                    if (idAlocacao.Contains("|"))
                    {
                        foreach (string item in idAlocacao.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                AtividadeFuncaoLiberadaBusiness.Alterar(new AtividadeFuncaoLiberada()
                                {

                                    IDAlocacao = Guid.Parse(item),
                                    IDAtividade = Guid.Parse(idAtividade),
                                    IDFuncao = Guid.Parse(IDFuncao),
                                    DataExclusao = DateTime.Now,
                                    //UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                                });
                            }
                        }
                    }
                    else
                    {

                        AtividadeFuncaoLiberadaBusiness.Alterar(new AtividadeFuncaoLiberada()
                        {

                            IDAlocacao = Guid.Parse(idAlocacao),
                            IDAtividade = Guid.Parse(idAtividade),
                            IDFuncao = Guid.Parse(IDFuncao),
                            DataExclusao = DateTime.Now,
                            UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            //UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                        });
                    }
                }

                return Json(new { resultado = new RetornoJSON() { } });
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
