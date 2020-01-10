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