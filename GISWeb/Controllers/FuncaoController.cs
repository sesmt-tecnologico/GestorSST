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
    public class FuncaoController : BaseController
    {

        #region Inject
       
        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Funcao = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d=>d.Cargo.NomeDoCargo).ToList();

            return View();
        }

        public ActionResult ListaFuncao(string id)
        {
            ViewBag.Funcao = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)&&(d.IdCargo.Equals(id))).OrderBy(d => d.NomeDaFuncao).ToList();

            return View();
        }

        public ActionResult Novo(string id, string nome)
        {
            ViewBag.Cargo = id;
            ViewBag.NomeDoCargo = nome;

            ViewBag.FuncCarg = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.IdCargo.Equals(id))).Count();


            //List<Atividade> Ativ = (from a in FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
            //                       join b in AtividadeBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList()
            //                       on a.IDFuncao equals b.idFuncao
            //                       where a.IdCargo.Equals(id)
            //                       select new Atividade()
            //                       {
            //                           Descricao = b.Descricao,

            
            //                           Funcao = new Funcao()
            //                           {
            //                               NomeDaFuncao = a.NomeDaFuncao,
            //                               IdCargo = a.IdCargo

            //                           },                                      
                                       
            //                       }

            //                       ).ToList();
            //ViewBag.FuncaoCargo = Ativ;

            ViewBag.funcoesPorCargo = FuncaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IdCargo.Equals(id))).ToList();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Funcao oFuncao, string cargID)
        {
            oFuncao.IdCargo = Guid.Parse(cargID);
            if (ModelState.IsValid)
            {
                try
                {
                    FuncaoBusiness.Inserir(oFuncao);

                    Extensions.GravaCookie("MensagemSucesso", "A Função '" + oFuncao.NomeDaFuncao + "' foi cadastrada com sucesso!", 10);


                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo") } });

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
            //ViewBag.Riscos = TipoDeRiscoBusiness.Consulta.Where(p => p.IDTipoDeRisco.Equals(id));

            return View(FuncaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Funcao oFuncao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FuncaoBusiness.Alterar(oFuncao);

                    Extensions.GravaCookie("MensagemSucesso", "A Função '" + oFuncao.NomeDaFuncao + "' foi atualizada com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Funcao") } });
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
        public ActionResult TerminarComRedirect(string IDFuncao)
        {

            try
            {
                Funcao oFuncao = FuncaoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDFuncao));
                if (oFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta Função." } });
                }
                else
                {
                    oFuncao.DataExclusao = DateTime.Now;
                    oFuncao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FuncaoBusiness.Alterar(oFuncao);

                    Extensions.GravaCookie("MensagemSucesso", "A Função'" + oFuncao.NomeDaFuncao + "' foi excluida com sucesso.", 10);

                                        
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Funcao") } });
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