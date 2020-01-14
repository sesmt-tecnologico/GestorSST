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
    public class FuncCargoController : BaseController
    {

        #region Inject
       
        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICargoesBusiness CargoBusiness { get; set; }

        [Inject]
        public IFuncCargoBusiness FuncCargoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Rel_CargoFuncAtividade> Rel_CargoFuncAtividadeBusiness { get; set; }

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

            ViewBag.Funcao = FuncCargoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)&&(d.Uk_Cargo.Equals(Uk_Cargo))).OrderBy(d => d.NomeDaFuncao).ToList();

            var ListFuncao = from c in CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                             join f in FuncCargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                             on c.UniqueKey equals f.Uk_Cargo
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

        public ActionResult Novo(string Uk, string nome, string id)
        {
            var uk_Cargo = Guid.Parse(Uk);
            var idCargo = Guid.Parse(id);
            ViewBag.Cargo = Uk;
            ViewBag.NomeDoCargo = nome;
            ViewBag.idCargo = idCargo;

            ViewBag.FuncCarg = FuncCargoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao) && (d.Uk_Cargo.Equals(uk_Cargo))).Count();

            string sql = @"select f.UniqueKey, f.NomeDaFuncao, f.Uk_Cargo 
                             from tbFuncCargo f
                             where f.Uk_Cargo = '" + uk_Cargo + @"' order by f.NomeDaFuncao ";

            List<FuncCargo> lista = new List<FuncCargo>();


            DataTable result = FuncCargoBusiness.GetDataTable(sql);

            if (result.Rows.Count > 0)
            {
                FuncCargo obj = null;

                foreach (DataRow row in result.Rows)
                {
                    if (result.Rows.Count > 0)
                    {
                        obj = new FuncCargo()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            NomeDaFuncao = row["NomeDaFuncao"].ToString()

                        };


                    }

                    if (obj != null)
                        lista.Add(obj);
                }


                
            }


            ViewBag.lista = lista;


                        return View();

        }




        //Função deve ser vinculada a um cargo no ato da criação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(FuncCargo oFuncao, string Uk_Cargo, string ID_Cargo)
        {
            oFuncao.Uk_Cargo = Guid.Parse(Uk_Cargo);
            oFuncao.CargoesID = Guid.Parse(ID_Cargo);
            if (ModelState.IsValid)
            {
                try
                {
                    FuncCargoBusiness.Inserir(oFuncao);

                    Extensions.GravaCookie("MensagemSucesso", "A Função '" + oFuncao.NomeDaFuncao + "' foi cadastrada com sucesso!", 10);


                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargoes") } });

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
                                if (Rel_CargoFuncAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UkFuncCargo.Equals(UKFuncCargo) && a.UkAtividade.Equals(pTemp.UniqueKey)).Count() == 0)
                                {
                                    Rel_CargoFuncAtividadeBusiness.Inserir(new Rel_CargoFuncAtividade()
                                    {
                                        UkFuncCargo = UKFuncCargo,
                                        UkAtividade = pTemp.UniqueKey,
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
                        if (Rel_CargoFuncAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UkFuncCargo.Equals(UKFuncCargo) && a.UkAtividade.Equals(pTemp.UniqueKey)).Count() == 0)
                        {
                            Rel_CargoFuncAtividadeBusiness.Inserir(new Rel_CargoFuncAtividade()
                            {
                                UkFuncCargo = UKFuncCargo,
                                UkAtividade = pTemp.UniqueKey,
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

            return View(FuncCargoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(FuncCargo oFuncao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FuncCargoBusiness.Alterar(oFuncao);

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
                FuncCargo oFuncao = FuncCargoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDFuncao));
                if (oFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir esta Função." } });
                }
                else
                {
                    oFuncao.DataExclusao = DateTime.Now;
                    oFuncao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    FuncCargoBusiness.Alterar(oFuncao);

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