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
	                                  (select STRING_AGG( CONCAT(r.UniqueKey, '$' , r.Nome), ',') WITHIN GROUP (ORDER BY r.Nome) 
	                                   from REL_WorkAreaRisco r1, tbRisco r 
		                               where r1.DataExclusao = '9999-12-31 23:59:59.997' and r.DataExclusao = '9999-12-31 23:59:59.997' and 
			                                 r1.UKWorkArea = wa.UniqueKey and r1.UKRisco = r.UniqueKey
	                                  ) as Riscos
                               from tbWorkArea wa
                               where wa.DataExclusao = '9999-12-31 23:59:59.997' and wa.UKEstabelecimento = '" + entidade.UKEstabelecimento + "'";

                DataTable result = WorkAreaBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        WorkArea obj = new WorkArea()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            Nome = row["Nome"].ToString(),
                            Descricao = row["Descricao"].ToString(),
                            Riscos = new List<Risco>()
                        };

                        if (!string.IsNullOrEmpty(row["Riscos"].ToString()))
                        {
                            if (row["Riscos"].ToString().Contains(","))
                            {
                                foreach (string item in row["Riscos"].ToString().Split('$'))
                                {
                                    if (!string.IsNullOrEmpty(item) && !item.Equals(","))
                                    {
                                        obj.Riscos.Add(new Risco()
                                        {
                                            UniqueKey = Guid.Parse(row["Riscos"].ToString().Split('$')[0]),
                                            Nome = row["Riscos"].ToString().Split('$')[1]
                                        });
                                    }
                                }
                            }
                            else
                            {
                                obj.Riscos.Add(new Risco()
                                {
                                    UniqueKey = Guid.Parse(row["Riscos"].ToString().Split('$')[0]),
                                    Nome = row["Riscos"].ToString().Split('$')[1]
                                });
                            }
                        }

                        lista.Add(obj);
                    }
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