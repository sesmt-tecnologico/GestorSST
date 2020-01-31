﻿using GISCore.Business.Abstract;
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
using System.Collections.Generic;
using System.Data;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PerigoController : BaseController
    {

        #region Inject

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Perigo = PerigoBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).ToList().OrderBy(p => p.Descricao);

            return View();
        }

        public ActionResult Novo()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Perigo oPerigo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    PerigoBusiness.Inserir(oPerigo);

                    Extensions.GravaCookie("MensagemSucesso", "O evento '" + oPerigo.Descricao + "' foi cadastrado com sucesso!", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perigo") } });

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
            var ID = Guid.Parse(id);

            return View(PerigoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Perigo oPerigo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PerigoBusiness.Alterar(oPerigo);

                    Extensions.GravaCookie("MensagemSucesso", "O Evento Perigoso '" + oPerigo.Descricao + "' foi atualizado com sucesso.", 10);


                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Perigo") } });
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


        public ActionResult ListaPerigo()
        {

            string sql = @"select p.UniqueKey as UK_P, p.Descricao, r.UniqueKey as UK_R, r.Nome, pr.UKPerigo, pr.UKRisco as relR
                            from tbPerigo p
                            left join REL_PerigoRisco pr on pr.UKPerigo = p.UniqueKey
                            left join tbRisco r on r.UniqueKey = pr.UKRisco
                            order by p.Descricao";


            DataTable result = PerigoBusiness.GetDataTable(sql);

            List<Perigo> lista = new List<Perigo>();


            if (result.Rows.Count > 0)
            {
                Perigo obj = null;
                Risco oRisco = null;

                foreach (DataRow row in result.Rows)
                {
                    if (obj == null)
                    {
                        obj = new Perigo()
                        {
                            UniqueKey = Guid.Parse(row["UK_P"].ToString()),
                            Descricao = row["Descricao"].ToString(),
                            Riscos = new List<Risco>()
                        };


                        if (!string.IsNullOrEmpty(row["relR"].ToString()))
                        {
                            oRisco = new Risco()
                            {
                                UniqueKey = Guid.Parse(row["relR"].ToString()),
                                Nome = row["Nome"].ToString(),

                            };
                            obj.Riscos.Add(oRisco);
                        }


                    }
                    //if UniqueKey for igual a UKPerigo
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UK_P"].ToString())))
                    {
                        //if UKRisco nao for nulo
                        if (!string.IsNullOrEmpty(row["relR"].ToString()))
                        {
                            if (oRisco == null)
                            {
                                oRisco = new Risco()
                                {
                                    UniqueKey = Guid.Parse(row["relR"].ToString()),
                                    Nome = row["Nome"].ToString(),

                                };

                                obj.Riscos.Add(oRisco);

                            }


                        }


                    }
                

                    else 
                    {
                        lista.Add(obj);

                        obj = new Perigo()
                        {
                            UniqueKey = Guid.Parse(row["UK_P"].ToString()),                            
                            Descricao = row["Descricao"].ToString(),
                            Riscos = new List<Risco>()
                        };


                        if (!string.IsNullOrEmpty(row["relR"].ToString()))
                        {
                            oRisco = new Risco()
                            {
                                ID = Guid.Parse(row["relR"].ToString()),
                                UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                                Nome = row["Nome"].ToString(),
                            
                            };                        

                            obj.Riscos.Add(oRisco);
                        }
                    }
                }

                    if (obj != null)
                        lista.Add(obj);

             }

                    return View("_ListaPerigo", lista);

        }






        public ActionResult Excluir(string id)
        {
            ViewBag.Perigo = new SelectList(PerigoBusiness.Consulta.ToList(), "ID", "Descricao");
            return View(PerigoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Terminar(string id)
        {


            var ID = Guid.Parse(id);

            try
            {
                Perigo oPerigo = PerigoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(ID));
                if (oPerigo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Evento Perigoso, pois o mesmo não foi localizado." } });
                }
                else
                {

                    oPerigo.DataExclusao = DateTime.Now;
                    oPerigo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PerigoBusiness.Alterar(oPerigo);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O  Perigo '" + oPerigo.Descricao + "' foi excluído com sucesso." } });
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


        [RestritoAAjax]
        public ActionResult BuscarPerigoForAutoComplete(string key)
        {
            try
            {
                List<string> perigoAsString = new List<string>();
                List<Perigo> lista = PerigoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Perigo com in lista)
                    perigoAsString.Add(com.Descricao);

                return Json(new { Result = perigoAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarPerigoForAutoComplete(string key)
        {
            try
            {
                Perigo item = PerigoBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }


    }
}