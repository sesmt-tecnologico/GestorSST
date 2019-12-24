using GISCore.Business.Abstract;
using GISCore.Repository.Configuration;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Admissao;
using System.Collections.Generic;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EmpregadoController : BaseController
    {

        #region Inject

        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoAmbienteBusiness EstabelecimentoAmbienteBusiness { get; set; }
        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        #endregion


        public ActionResult ListaEmpregadoNaoAdmitido()
        {
           // var ID = Guid.Parse(id);

            var listEmp = from e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          join a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                          on e.ID equals a.IDEmpregado
                          into g
                          from NaoAdm in g.DefaultIfEmpty()
                          select new AdmissaoViewModel()
                          {
                              ID = e.ID,
                              NomeEmpregado = e.Nome,
                              CPF = e.CPF,
                              Admitido = NaoAdm?.Admitido ?? string.Empty

                          };

            
            List<AdmissaoViewModel> lista = new List<AdmissaoViewModel>();

            lista = listEmp.ToList();

            ViewBag.Empregado = lista;

            //ViewBag.Empregado = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }


        public ActionResult ListaEmpregadoAdmitidoPorEmpresa()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListaEmpregadoAdmitidoPorEmpresa(PesquisaEmpregadoViewModel entidade)
        {
            try
            {

                string sWhere = string.Empty;

                if (string.IsNullOrEmpty(entidade.NomeEmpregado) &&
                   string.IsNullOrEmpty(entidade.CPF) &&
                   string.IsNullOrEmpty(entidade.NomeEmpresa)
                   )
                    throw new Exception("Informe pelo menos um filtro para prosseguir na pesquisa.");

                
                var listEmp = from e in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                              join a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                              on e.ID equals a.IDEmpregado
                              into g
                              from y in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                              join a in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                              on y.ID equals a.IDEmpresa
                              into h

                              from NaoAdm in g.DefaultIfEmpty()

                              where e.Nome.Contains(entidade.NomeEmpregado) ||
                              e.CPF.Equals(entidade.CPF) ||
                              y.NomeFantasia.Equals(entidade.NomeEmpresa)

                              select new PesquisaEmpregadoViewModel()
                              {
                                  idEmpregado = e.ID,
                                  NomeEmpregado = e.Nome,
                                  CPF = e.CPF,
                                  NomeEmpresa = y.NomeFantasia,
                                  Admitido = NaoAdm?.Admitido ?? string.Empty

                              };


                List<PesquisaEmpregadoViewModel> lista = listEmp.ToList();

               

                return PartialView("_ListaEmpregadoPorEmpresa", lista);
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







        public ActionResult Novo()
        {
            return View();
        }

        public ActionResult Edicao(string id)
        {
            return View(EmpregadoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    EmpregadoBusiness.Inserir(empregado);

                    Extensions.GravaCookie("MensagemSucesso", "O empregado '" + empregado.Nome + "' foi cadastrado com sucesso.", 10);

                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("ListaEmpregado", "Empregado",new {id=empregado.ID }) } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoBusiness.Alterar(empregado);

                    Extensions.GravaCookie("MensagemSucesso", "O empregado '" + empregado.Nome + "' foi atualizado com sucesso.", 10);


                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empregado") } });
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
        public ActionResult Terminar(string IDEmpregado)
        {

            try
            {
                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDEmpregado));
                if (oEmpregado == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o empregado, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oEmpregado.DataExclusao = DateTime.Now;
                    //oEmpregado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EmpregadoBusiness.Excluir(oEmpregado);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O empregado '" + oEmpregado.Nome + "' foi excluído com sucesso." } });
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