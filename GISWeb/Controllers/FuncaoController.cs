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
using GISModel.DTO.Funcao;
using System.Collections.Generic;
using System.Data;

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
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_FuncaoAtividade> REL_FuncaoAtividadeBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            //ViewBag.Funcao = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(d=>d.Cargo.NomeDoCargo).ToList();

            return View();
        }



        
        public ActionResult ListaFuncao(string Uk)
        {
            var Uk_Cargo = Guid.Parse(Uk);

            ViewBag.Funcao = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)&&(d.UKCargo.Equals(Uk_Cargo))).OrderBy(d => d.NomeDaFuncao).ToList();

            var ListFuncao = from c in CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                             join f in FuncaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                             on c.UniqueKey equals f.UKCargo
                             into g
                             from func in g.DefaultIfEmpty()
                             where c.UniqueKey.Equals(Uk_Cargo)
                             select new ListaFuncaoViewModel()
                             {

                                 ID_Funcao = func.ID,
                                 Uk_Cargo = c.UniqueKey,
                                 ID_Cargo = c.ID,
                                 nomeCargo = c.NomeDoCargo,
                                 Uk_Funcao = func.UniqueKey,
                                 NomeFuncao = func.NomeDaFuncao


                             };


            List<ListaFuncaoViewModel> lista = ListFuncao.ToList();

            ViewBag.Lista = lista;

            

            return View();
        }

        public ActionResult Novo(string Uk, string nome)
        {
            Funcao func = new Funcao();
            func.UKCargo = Guid.Parse(Uk);
            
            ViewBag.NomeDoCargo = nome;

            ViewBag.FuncCarg = FuncaoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.UKCargo.Equals(func.UKCargo))).Count();


            string sql = @"select f.UniqueKey, f.NomeDaFuncao, f.UkCargo 
                             from tbFuncao f
                             where f.UkCargo = '" + func.UKCargo.ToString() + @"' order by f.NomeDaFuncao ";

            List<Funcao> lista = new List<Funcao>();
            DataTable result = FuncaoBusiness.GetDataTable(sql);
            if (result.Rows.Count > 0)
            {
                Funcao obj = null;

                foreach (DataRow row in result.Rows)
                {
                    if (result.Rows.Count > 0)
                    {
                        obj = new Funcao()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            NomeDaFuncao = row["NomeDaFuncao"].ToString()

                        };
                    }
                    
                }

                if (obj != null)
                    lista.Add(obj);
            }

            ViewBag.lista = lista;


            return View(func);

        }




        //Função deve ser vinculada a um cargo no ato da criação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Funcao oFuncao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    oFuncao.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
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


        
        
        public ActionResult VincularFuncaoAtividade(string UK_Funcao)
        {

            ViewBag.Uk_Funcao = UK_Funcao;

            return PartialView("_VincularFuncaoAtividade");
        }

        [HttpPost]
       
        public ActionResult VincularCargoFuncaoAtividade(string UKAtividade, string UkFuncao)
        {
           

            try
            {
                if (string.IsNullOrEmpty(UkFuncao))
                    throw new Exception("Não foi possível localizar a função.");

                if (string.IsNullOrEmpty(UKAtividade))
                    throw new Exception("Nenhuma  atividade recebida como parâmetro para vincular a função.");

                var UKFuncCargo = Guid.Parse(UkFuncao);

                if (UKAtividade.Contains(","))
                {
                    foreach (string ativ in UKAtividade.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            Atividade pTemp = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_FuncaoAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKFuncao.Equals(UKFuncCargo) && a.UKAtividade.Equals(pTemp.UniqueKey)).Count() == 0)
                                {
                                    REL_FuncaoAtividadeBusiness.Inserir(new REL_FuncaoAtividade()
                                    {
                                        UKFuncao = UKFuncCargo,
                                        UKAtividade = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Atividade pTemp = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.Equals(UKAtividade.Trim()));
                    
                    if (pTemp != null)
                    {
                        if (REL_FuncaoAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKFuncao.Equals(UKFuncCargo) && a.UKAtividade.Equals(pTemp.UniqueKey)).Count() == 0)
                        {
                            REL_FuncaoAtividadeBusiness.Inserir(new REL_FuncaoAtividade()
                            {
                                UKFuncao = UKFuncCargo,
                                UKAtividade = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Atividade relacionado a função com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
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