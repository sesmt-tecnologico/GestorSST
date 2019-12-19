using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO;
using GISWeb.Infraestrutura.Provider.Concrete;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.DTO.Estabelecimento;
using System.Collections.Generic;
using GISModel.DTO.WorkArea;
using System.Data;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class WorkAreaController : BaseController
    {

        #region inject

        [Inject]
        public IWorkAreaBusiness WorkAreaBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        
        public ActionResult Index()
        {
            ViewBag.Estab = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();  

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarWorkArea(WorkAreaViewModel entidade)
        {
            try
            {
                List<WorkArea> lista = new List<WorkArea>();

                string sql = @"select wa.UniqueKey, wa.Nome, wa.Descricao, 
	                                  r1.Uniquekey as relwap, p.UniqueKey as ukperigo, p.Descricao as perigo, 
	                                  r2.UniqueKey as relpr, r.UniqueKey as ukrisco, r.Nome as risco 
                               from tbWorkArea wa 
	                                left join REL_WorkAreaPerigo r1 on r1.UKWorkArea = wa.UniqueKey 
	                                left join tbPerigo p on r1.UKPerigo = p.UniqueKey 
	                                left join REL_PerigoRisco r2 on r2.UKPerigo = p.UniqueKey 
	                                left join tbRisco r on r2.UKRisco = r.UniqueKey 
                               where wa.DataExclusao = '9999-12-31 23:59:59.997' and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + @"' 
                               order by wa.UniqueKey ";

                DataTable result = WorkAreaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    WorkArea obj = null;

                    foreach (DataRow row in result.Rows)
                    {
                        if (obj == null)
                        {
                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                Perigos = new List<Perigo>()
                            };


                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                Perigo oPerigo = new Perigo()
                                {
                                    ID = Guid.Parse(row["relwap"].ToString()),
                                    UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                    Descricao = row["perigo"].ToString(),
                                    Riscos = new List<Risco>()
                                };

                                if (!string.IsNullOrEmpty(row["relpr"].ToString())) {
                                    oPerigo.Riscos.Add(new Risco()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                        Nome = row["risco"].ToString()
                                    });
                                }

                                obj.Perigos.Add(oPerigo);
                            }

                        }
                        else if (obj.UniqueKey.Equals(Guid.Parse(row["UniqueKey"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                Perigo oPerigo = new Perigo()
                                {
                                    ID = Guid.Parse(row["relwap"].ToString()),
                                    UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                    Descricao = row["perigo"].ToString(),
                                    Riscos = new List<Risco>()
                                };

                                if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                {
                                    oPerigo.Riscos.Add(new Risco()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                        Nome = row["risco"].ToString()
                                    });
                                }

                                obj.Perigos.Add(oPerigo);
                            }
                        }
                        else
                        {
                            lista.Add(obj);

                            obj = new WorkArea()
                            {
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                Nome = row["Nome"].ToString(),
                                Descricao = row["Descricao"].ToString(),
                                Perigos = new List<Perigo>()
                            };


                            if (!string.IsNullOrEmpty(row["relwap"].ToString()))
                            {
                                Perigo oPerigo = new Perigo()
                                {
                                    ID = Guid.Parse(row["relwap"].ToString()),
                                    UniqueKey = Guid.Parse(row["ukperigo"].ToString()),
                                    Descricao = row["perigo"].ToString(),
                                    Riscos = new List<Risco>()
                                };

                                if (!string.IsNullOrEmpty(row["relpr"].ToString()))
                                {
                                    oPerigo.Riscos.Add(new Risco()
                                    {
                                        ID = Guid.Parse(row["relpr"].ToString()),
                                        UniqueKey = Guid.Parse(row["ukrisco"].ToString()),
                                        Nome = row["risco"].ToString()
                                    });
                                }

                                obj.Perigos.Add(oPerigo);
                            }
                        }
                    }

                    if (obj != null)
                        lista.Add(obj);

                }

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        public ActionResult Novo()
        {

            ViewBag.Estabelecimentos = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        public ActionResult Cadastrar(WorkArea entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    WorkAreaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "WorkArea '" + entidade.Nome + "' foi cadastrada com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "WorkArea") } });
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
            Guid ID = Guid.Parse(id);
            ViewBag.Workarea = new SelectList(WorkAreaBusiness.Consulta.ToList(), "ID", "Nome");

            var lista = WorkAreaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.ID.Equals(ID)));                       


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(WorkArea entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    WorkAreaBusiness.Alterar(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "A WorkArea '" + entidade.Nome + "' foi atualizado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "WorkArea") } });
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

    }
}