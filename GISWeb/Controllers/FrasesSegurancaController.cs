using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.Entidades;
using GISModel.Entidades.Quest;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{


    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FrasesSegurancaController : BaseController
    {
        #region
        [Inject]
        public IBaseBusiness<FrasesSeguranca> FrasesSegurancaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        #endregion

        // GET: FrasesSeguranca
        public ActionResult Index()
        {

            ViewBag.Frases = FrasesSegurancaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).OrderByDescending(a=>a.Status).ToList();

            return View();
        }

        public ActionResult Novo()
        {
            var enumData = from ECategoriaFrases e in Enum.GetValues(typeof(ECategoriaFrases))
                           select new
                           {
                               ID = (int)e,
                               Name = e.GetDisplayName()
                           };
            ViewBag.ECategoriaFrases = new SelectList(enumData, "ID", "Name");



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(FrasesSeguranca entidade) {


            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    entidade.Status = Situacao.Ativo;
                    FrasesSegurancaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "A Frase '" + entidade.Descricao + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FrasesSeguranca") } });

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
        public ActionResult Desativar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);

                FrasesSeguranca temp = FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível desativar a frase, pois a mesma não foi localizada na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                FrasesSegurancaBusiness.Terminar(temp);

                FrasesSegurancaBusiness.Inserir(new FrasesSeguranca()
                {
                    UniqueKey = temp.UniqueKey,
                    Descricao = temp.Descricao,
                    Categoria = temp.Categoria,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                    Status = Situacao.Inativo
                });

                return Json(new { resultado = new RetornoJSON() { Sucesso = " A Frase '" + temp.Descricao + "'foi desativada com sucesso." } });
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

        public ActionResult Edicao(string id)
        {

            
                Guid ID = Guid.Parse(id);

               if(FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID) && p.Status == Situacao.Ativo) == null)
                {

                    Extensions.GravaCookie("MensagemSucesso", "A Frase deve estar Ativada!", 10);

                return View();
                }
                else
                {
                    return View(FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID) && p.Status == GISModel.Enums.Situacao.Ativo));

                }                        
                
            
            




            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(FrasesSeguranca oFrases)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    

                    FrasesSeguranca tempFrases = FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(oFrases.ID));

                    tempFrases.Categoria = oFrases.Categoria;
                    tempFrases.Descricao = oFrases.Descricao;
                    tempFrases.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FrasesSegurancaBusiness.Alterar(tempFrases);

                    Extensions.GravaCookie("MensagemSucesso", "A frase '" + oFrases.Descricao + "' foi atualizado com sucesso.", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "FrasesSeguranca") } });
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
        public ActionResult Ativar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                FrasesSeguranca temp = FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível ativar a frase, pois a mesma não foi localizada na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                FrasesSegurancaBusiness.Terminar(temp);

                FrasesSegurancaBusiness.Inserir(new FrasesSeguranca()
                {
                    UniqueKey = temp.UniqueKey,
                    Descricao = temp.Descricao,
                    Categoria = temp.Categoria,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                    Status = Situacao.Ativo
                });

                return Json(new { resultado = new RetornoJSON() { Sucesso = " A Frase '" + temp.Descricao + "'foi Ativada com sucesso." } });
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
        public ActionResult Terminar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                FrasesSeguranca temp = FrasesSegurancaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o questionário, pois o mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                FrasesSegurancaBusiness.Terminar(temp);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O questionário '" + temp.Descricao + "' foi excluído com sucesso." } });

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