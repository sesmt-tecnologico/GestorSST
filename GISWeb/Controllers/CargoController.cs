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
    public class CargoController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoesBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }


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
        public ActionResult Cadastrar(Cargo oCargo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    CargoesBusiness.Inserir(oCargo);

                    Extensions.GravaCookie("MensagemSucesso", "O Cargo '" + oCargo.NomeDoCargo + "' foi cadastrado com sucesso!", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Cargo", new { id = oCargo.ID })}});

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
        public ActionResult Atualizar(Cargo oCargo)
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

            string sql = @"select c.UniqueKey, c.NomeDoCargo as Cargo, 
	                              f.UniqueKey as UKFuncao, f.NomeDaFuncao as Funcao, 
	                              cfa.UniqueKey as UKRel,
	                              a.UniqueKey UKAtividade, a.Descricao as Atividade
						  from tbCargo c
                                  left join tbFuncao f on f.UKCargo = c.UniqueKey 
						          left join REL_FuncaoAtividade cfa on f.UniqueKey = cfa.UKFuncao  
						          left join tbAtividade a on a.UniqueKey = cfa.UkAtividade                          
						  order by c.NomeDoCargo, f.NomeDaFuncao ";

            DataTable result = CargoesBusiness.GetDataTable(sql);

            List<Cargo> lista = new List<Cargo>();
            if (result.Rows.Count > 0)
            {
                Cargo obj = null;
                Funcao oFuncao = null;

                foreach (DataRow row in result.Rows)
                {

                    if (obj == null)
                    {
                        obj = new Cargo()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            NomeDoCargo = row["Cargo"].ToString(),
                            Funcoes = new List<Funcao>()
                        };

                        if (!string.IsNullOrEmpty(row["UKFuncao"].ToString()))
                        {
                            oFuncao = new Funcao()
                            {
                                UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                NomeDaFuncao = row["Funcao"].ToString(),
                                Atividades = new List<Atividade>()
                            };

                            if (!string.IsNullOrEmpty(row["UKRel"].ToString()))
                            {
                                oFuncao.Atividades.Add(new Atividade()
                                {
                                    UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                    Descricao = row["Atividade"].ToString()
                                });
                            }

                            obj.Funcoes.Add(oFuncao);
                        }

                    }
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqueKey"].ToString())))
                    {
                        if (!string.IsNullOrEmpty(row["UKFuncao"].ToString()))
                        {
                            if (oFuncao == null)
                            {
                                oFuncao = new Funcao()
                                {
                                    UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                    NomeDaFuncao = row["Funcao"].ToString(),
                                    Atividades = new List<Atividade>()
                                };

                                if (!string.IsNullOrEmpty(row["UKRel"].ToString()))
                                {
                                    oFuncao.Atividades.Add(new Atividade()
                                    {
                                        UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                        Descricao = row["Atividade"].ToString()
                                    });
                                }

                                obj.Funcoes.Add(oFuncao);
                            }
                            else if (oFuncao.NomeDaFuncao.Equals(row["Funcao"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(row["UKRel"].ToString()))
                                {
                                    oFuncao.Atividades.Add(new Atividade()
                                    {
                                        UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                        Descricao = row["Atividade"].ToString()
                                    });
                                }
                            }
                            else
                            {
                                oFuncao = new Funcao()
                                {
                                    UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                    NomeDaFuncao = row["Funcao"].ToString(),
                                    Atividades = new List<Atividade>()
                                };

                                if (!string.IsNullOrEmpty(row["UKRel"].ToString()))
                                {
                                    oFuncao.Atividades.Add(new Atividade()
                                    {
                                        UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                        Descricao = row["Atividade"].ToString()
                                    });
                                }

                                obj.Funcoes.Add(oFuncao);
                            }

                        }
                    }
                    else
                    {
                        lista.Add(obj);

                        obj = new Cargo()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            NomeDoCargo = row["Cargo"].ToString(),
                            Funcoes = new List<Funcao>()
                        };

                        if (!string.IsNullOrEmpty(row["UKFuncao"].ToString()))
                        {
                            oFuncao = new Funcao()
                            {
                                UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                NomeDaFuncao = row["Funcao"].ToString(),
                                Atividades = new List<Atividade>()
                            };

                            if (!string.IsNullOrEmpty(row["UKRel"].ToString()))
                            {
                                oFuncao.Atividades.Add(new Atividade()
                                {
                                    UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                    Descricao = row["Atividade"].ToString()
                                });
                            }

                            obj.Funcoes.Add(oFuncao);
                        }
                    }

                } // Fim foreach 

                if (obj != null)
                    lista.Add(obj);

            }

            return PartialView("_ListaCargo", lista);
        }


        


        [HttpPost]
        public ActionResult TerminarComRedirect(string IDCargo)
        {

            try
            {
                Cargo oCargo = CargoesBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(IDCargo));
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