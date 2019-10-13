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

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class EstabelecimentoController : BaseController
    {

        #region Inject

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        public ActionResult Lista()
        {
            ViewBag.Estabelecimento = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        public ActionResult Novo(string IDEmpresa, string nome)
        {
            ViewBag.Empresas = IDEmpresa;
            ViewBag.NomeEmpresa = nome;
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.Where(p=>string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDepartamento", "Sigla");
            

            try
            {
                // Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idFuncao.Equals(id));

                if (ViewBag.Empresas == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Parametro id não passado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Novo") });
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

        public ActionResult ListarEstabelecimentoPorDepartamento(string idDepartamento)
        {

            return Json(new { resultado = EstabelecimentoBusiness.Consulta.Where(p => p.IDDepartamento.Equals(idDepartamento)).ToList().OrderBy(p => p.IDDepartamento) });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Estabelecimento oEstabelecimento, string EmpresaID)
        {
            
            if (ModelState.IsValid)
            {

                try
                {

                    EstabelecimentoBusiness.Inserir(oEstabelecimento);

                    Extensions.GravaCookie("MensagemSucesso", "O Estbelecimento '" + oEstabelecimento.NomeCompleto + "' foi cadastrado com sucesso.", 10);

                                                           
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("EmpresaCriacoes", "Empresa", new { id = EmpresaID }) } });
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
            ViewBag.Departamento = new SelectList(DepartamentoBusiness.Consulta.ToList(), "IDDepartamento", "Sigla");
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Estabelecimento oEstabelecimento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EstabelecimentoBusiness.Alterar(oEstabelecimento);

                    Extensions.GravaCookie("MensagemSucesso", "O Estbelecimento '" + oEstabelecimento.NomeCompleto + "' foi atualizado com sucesso.", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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

        public ActionResult Excluir(string id)
        {
            ViewBag.Empresa = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");
            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.ID.Equals(id)));

        }

        [HttpPost]
        public ActionResult Excluir(Estabelecimento oEstabelecimento)
        {

            try
            {

                if (oEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Estabelecimento, pois o mesmo não foi localizado." } });
                }
                else
                {

                    //oEstabelecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    //oEstabelecimento.UKUsuarioDemissao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    oEstabelecimento.UsuarioExclusao = "Antonio Henriques";
                    oEstabelecimento.DataExclusao = DateTime.Now;
                    EstabelecimentoBusiness.Excluir(oEstabelecimento);

                    Extensions.GravaCookie("MensagemSucesso", "O Estabelecimento '" + oEstabelecimento.NomeCompleto + "' foi excluido com sucesso.", 10);

                   
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
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