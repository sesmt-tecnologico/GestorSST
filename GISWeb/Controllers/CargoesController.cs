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
    public class CargoesController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICargoesBusiness CargoesBusiness { get; set; }

        [Inject]
        public IFuncCargoBusiness FuncaoBusiness { get; set; }


        [Inject]
        public IDiretoriaBusiness DiretoriaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Cargo = CargoesBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).Distinct().ToList();

            return View();
        }

        public ActionResult Novo()
        {
            //var Dir = from a in DiretoriaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
            //          join b in EmpresaBusiness.Consulta.ToList()
            //          on a.IDEmpresa equals b.ID                      
            //          select new Diretoria()
            //          {
            //              ID = a.ID,
            //              Sigla = a.Sigla
            //          };

            //ViewBag.Diretoria = new SelectList(Dir.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDiretoria", "Sigla");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Cargoes oCargo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    CargoesBusiness.Inserir(oCargo);

                    Extensions.GravaCookie("MensagemSucesso", "O Cargo '" + oCargo.NomeDoCargo + "' foi cadastrado com sucesso!", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargoes", new { id = oCargo.ID })}});

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

            return View(CargoesBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Cargoes oCargo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CargoesBusiness.Alterar(oCargo);

                    Extensions.GravaCookie("MensagemSucesso", "O Cargo '" + oCargo.NomeDoCargo + "' foi atualizado com sucesso.", 10);

                                        
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

        public ActionResult ListaCargo()
        {

            string sql = @"select c.ID as id, c.UniqueKey, c.NomeDoCargo, f.ID as rel02, f.Uk_Cargo as rel01 ,f.NomeDaFuncao from tbCargoes c
                          left join tbFuncCargo f
                          on c.UniqueKey = f.Uk_Cargo
                          order by c.NomeDoCargo";


            DataTable result = CargoesBusiness.GetDataTable(sql);


            List<Cargoes> lista = new List<Cargoes>();

           
            if (result.Rows.Count > 0)
            {
                Cargoes oCargos = null;

                foreach (DataRow Row in result.Rows)
                {
                    if (result.Rows.Count > 0)
                    {
                      
                        oCargos = new Cargoes()
                        {
                            ID = Guid.Parse(Row["ID"].ToString()),
                            UniqueKey = Guid.Parse(Row["UniqueKey"].ToString()),
                            NomeDoCargo = Row["NomeDoCargo"].ToString(),                            
                            funcoes = new List<FuncCargo>()


                        };

                        foreach (DataRow Row2 in result.Rows)
                        {
                            if (!string.IsNullOrEmpty(Row2["rel01"].ToString()) && Row2["rel01"].ToString().Equals(Row["rel01"].ToString()))
                            {
                                FuncCargo oFuncao = new FuncCargo()
                                {
                                    ID = Guid.Parse(Row2["rel02"].ToString()),
                                    Uk_Cargo = Guid.Parse(Row2["rel01"].ToString()),
                                    UniqueKey = Guid.Parse(Row2["UniqueKey"].ToString()),
                                    NomeDaFuncao = Row2["NomeDaFuncao"].ToString(),
                                    

                                };

                                oCargos.funcoes.Add(oFuncao);
                            }

                        }

                        if (oCargos != null)
                            lista.Add(oCargos);
                        
                    }
                   
                    
                }
            }

            return PartialView("_ListaCargo", lista);
        }





        [HttpPost]
        public ActionResult TerminarComRedirect(string IDCargo)
        {

            try
            {
                Cargoes oCargo = CargoesBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDCargo));
                if (oCargo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir este Cargo." } });
                }
                else
                {
                    oCargo.DataExclusao = DateTime.Now;
                    oCargo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    CargoesBusiness.Alterar(oCargo);


                    Extensions.GravaCookie("MensagemSucesso", "O Cargo'" + oCargo.NomeDoCargo + "' foi excluido com sucesso.", 10);
                    
                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo", new { id = IDCargo }) } });
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