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
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_WorkAreaAtividade> REL_WorkAreaAtividadeBusiness { get; set; }

        #endregion

        // GET: WorkArea
        public ActionResult Index()
        {
            ViewBag.Estab = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();  

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarWorkArea(PesquisaWorkAreaViewModel entidade)
        {

            try
            {


                var dep = from r in REL_WorkAreaAtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          join d in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on r.IDEstabelecimento equals entidade.IDEstabelecimento                          
                          join w in WorkAreaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on r.IDWorkArea equals w.UniqueKey
                          join a in AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on r.IDAtividade equals a.ID
                          where d.ID.Equals(entidade.IDEstabelecimento)
                          select new PesquisaWorkAreaViewModel()
                          {
                             IDWorkarea = w.ID,
                             IDEstabelecimento = d.ID,
                             Nome = w.Nome,
                             Descricao = w.Descricao,
                             UniqueKey = w.UniqueKey,
                             Atividade = a.Descricao
                             
                              
                          };


               




                List<PesquisaWorkAreaViewModel> lista = dep.ToList();    
                   

                return PartialView("_Pesquisa", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        public ActionResult Novo()
        {

            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

           

            ViewBag.Atividade = AtividadeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();


            var Est = from e in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      select new WorkAreaViewModel()
                      {
                          IDEstabelecimento = e.ID,
                          NomeCompleto = e.NomeCompleto

                      };



            ViewBag.Estab = Est.ToList();



            return View();
        }

        public ActionResult Cadastrar(WorkAreaViewModel entidade)
        {
           

            if (ModelState.IsValid)
            {
                try
                {

                    if (entidade?.Atividade?.Count > 0)
                    {
                        WorkArea obj = new WorkArea()
                        {
                            UniqueKey = Guid.NewGuid(),
                            Nome = entidade.Nome,
                            Descricao = entidade.Descricao,                            
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                        };
                        WorkAreaBusiness.Inserir(obj);

                        if (entidade?.Atividade?.Count > 0)
                        {

                            var Rel_Work = from a in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.UniqueKey.Equals(entidade.IDEstabelecimento))).ToList()
                                           select new Estabelecimento()
                                           {
                                               ID = a.ID
                                           };



                            Guid filtro = new Guid();                          

                            if(Rel_Work != null)
                            {
                                foreach(var item in Rel_Work)
                                {
                                    filtro = item.ID;
                                }
                                
                            }
                           
                            foreach(string wk in entidade.Atividade)
                            {
                                REL_WorkAreaAtividade rel = new REL_WorkAreaAtividade()
                                {
                                    IDAtividade = Guid.Parse(wk),
                                    IDWorkArea = obj.UniqueKey,
                                    IDEstabelecimento = filtro                                  
                                   
                                };
                                REL_WorkAreaAtividadeBusiness.Inserir(rel);


                            }

                        }

                    }else
                    {
                        throw new Exception("É necessário informar pelo menos uma Atividade para prosseguir com o cadastro.");
                    }

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