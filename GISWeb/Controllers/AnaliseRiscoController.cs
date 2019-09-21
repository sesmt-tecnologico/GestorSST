using GISCore.Business.Abstract;
using GISModel.DTO.AnaliseRisco;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AnaliseRiscoController : BaseController
    {
        #region-Inject
        [Inject]
        public IAtividadesDoEstabelecimentoBusiness AtividadesDoEstabelecimentoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IAtividadeAlocadaBusiness AtividadeAlocadaBusiness { get; set; }


        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IAnaliseRiscoBusiness AnaliseRiscoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }

        [Inject]
        public IPerigoPotencialBusiness PerigoPotencialBusiness { get; set; }

        [Inject]
        public IMedidasDeControleBusiness MedidasDeControleBusiness { get; set; }

        #endregion

        // GET: AtividadeAlocada
        public ActionResult Novo(string id)
        {

            ViewBag.Analise = new SelectList(AtividadesDoEstabelecimentoBusiness.Consulta, "IDAtividadesDoEstabelecimento", "DescricaoDestaAtividade");

            return View();
        }

        //Lista atividade para executar análise de risco.
        //Ao escolher a atividade abrirá outra caixa listando os riscos e o empregado informa se 
        //está apto a executar a atividade.
        public ActionResult PesquisarAtividadesRiscos(string idEstabelecimento, string idAlocacao)
        {
            ViewBag.Imagens = AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDEstabelecimento.Equals(idEstabelecimento))).ToList();

            try
            {


                var listaAmbientes = from AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idAlocacao.Equals(idAlocacao)).ToList()
                                     join AR in AnaliseRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on AL.ID equals AR.IDAtividadeAlocada
                                     into ARGroup
                                     from item in ARGroup.DefaultIfEmpty()

                                     join AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on AL.idAtividadesDoEstabelecimento equals AE.ID
                                     into AEGroup
                                     from item0 in AEGroup.DefaultIfEmpty()

                                     join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                     on item0.ID equals TR.idAtividadesDoEstabelecimento
                                     into TRGroup
                                     from item1 in TRGroup.DefaultIfEmpty()

                                     select new AnaliseRiscosViewModel
                                     {
                                         DescricaoAtividade = AL.AtividadesDoEstabelecimento.DescricaoDestaAtividade,
                                         //Riscos = item1.PerigoPotencial.DescricaoEvento,
                                         //FonteGeradora = item1.FonteGeradora,
                                         AlocaAtividade = (item == null ? false : true),
                                         //Conhecimento = item.Conhecimento,
                                         //BemEstar = item.BemEstar,



                                     };


                List<AnaliseRiscosViewModel> lAtividadesRiscos = listaAmbientes.ToList();



                AtividadesDoEstabelecimento oIDRiscosDoEstabelecimento = AtividadesDoEstabelecimentoBusiness.Consulta.FirstOrDefault(p => p.IDEstabelecimento.Equals(idEstabelecimento));
                if (oIDRiscosDoEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Atividades de Riscos não encontrada." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("SalvarAnaliseRisco", lAtividadesRiscos), Contar = lAtividadesRiscos.Count() });
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


        public ActionResult SalvarAnaliseRisco(string idEstabelecimento, string idAlocacao)
        {

            ViewBag.EventoPerigoso = new SelectList(EventoPerigosoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEventoPerigoso", "Descricao");

            ViewBag.PerigoPotencial = new SelectList(PerigoPotencialBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDPerigoPotencial", "DescricaoEvento");



            var ListaAmbientes = from AL in AtividadeAlocadaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idAlocacao.Equals(idAlocacao)).ToList()
                                 join AR in AnaliseRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                 on AL.ID equals AR.IDAtividadeAlocada
                                 into ARGroup
                                 from item in ARGroup.DefaultIfEmpty()

                                 join AE in AtividadesDoEstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                 on AL.idAtividadesDoEstabelecimento equals AE.ID
                                 into AEGroup
                                 from item2 in AEGroup.DefaultIfEmpty()

                                 join TR in TipoDeRiscoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                 on item2.ID equals TR.idAtividadesDoEstabelecimento
                                 into TRGroup
                                 from item3 in TRGroup.DefaultIfEmpty()

                                

                                 select new AnaliseRiscosViewModel
                                 {
                                     IDTipoDeRisco= item3.ID,
                                     FonteGeradora = item3.FonteGeradora,
                                     IDAmissao = AL.Alocacao.Admissao.ID,
                                     Imagem = AL.Alocacao.Admissao.Imagem,
                                     Riscos = item3.EventoPerigoso.Descricao,
                                     DescricaoAtividade = item2.DescricaoDestaAtividade,
                                     IDAtividadeAlocada = AL.ID,
                                     Conhecimento = item? .Conhecimento??false,                                     
                                     BemEstar = item?.BemEstar??false,
                                     PossiveisDanos = item3.PossiveisDanos.DescricaoDanos,
                                     IDAtividadeEstabelecimento = AL.AtividadesDoEstabelecimento.ID,
                                     imagemEstab = AL.AtividadesDoEstabelecimento.Imagem,
                                     AlocaAtividade = item == null ? false : true
                                 };


            List<AnaliseRiscosViewModel> lAtividadesRiscos = ListaAmbientes.ToList();

            ViewBag.Risco = ListaAmbientes.ToList();

            var Emp = from Adm in AdmissaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      join Aloc in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      on Adm.ID equals Aloc.IdAdmissao
                      join Empre in EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      on Adm.IDEmpregado equals Empre.ID
                      join Firm in EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                      on Adm.IDEmpresa equals Firm.ID
                      where Aloc.ID.Equals(idAlocacao)
                      select new Admissao()
                      {
                          DataAdmissao = Adm.DataAdmissao,
                          Empresa = new Empresa()
                          {
                              NomeFantasia = Firm.NomeFantasia

                          },

                          Empregado = new Empregado()
                          {
                              Nome = Empre.Nome,
                              DataNascimento = Empre.DataNascimento,

                          },
                          
                          

                      };

            ViewBag.Emp = Emp.ToList();



            var ListaEmpregado = from AL in AlocacaoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(idAlocacao)).ToList()
                                 select new Alocacao()
                                 {
                                     Admissao = new Admissao()
                                     {
                                       Empregado = new Empregado()
                                       {
                                           Nome = AL.Admissao.Empregado.Nome,
                                       
                                       },
                                         Empresa = new Empresa()
                                         {
                                             NomeFantasia = AL.Admissao.Empresa.NomeFantasia
                                         }
                                     },

                                   Equipe = new Equipe()
                                   {
                                       NomeDaEquipe = AL.Equipe.NomeDaEquipe
                                   },
                                                             


                                 };

            ViewBag.Listaempregado = ListaEmpregado;




            return View();
        }

        [HttpPost]
        public ActionResult SalvarAnaliseRisco(AnaliseRisco oAnaliseRisco, string AtivEstabID, string idATivAlocada, bool ConhecID, bool BemEstarID)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    oAnaliseRisco.IDAtividadesDoEstabelecimento = AtivEstabID;
                    oAnaliseRisco.Conhecimento = ConhecID;
                    oAnaliseRisco.BemEstar = BemEstarID;
                    oAnaliseRisco.IDAtividadeAlocada = idATivAlocada;

                    //oAnaliseRisco.BemEstar = oAnaliseRiscosViewModel.BemEstar;
                    //oAnaliseRisco.Conhecimento = oAnaliseRiscosViewModel.Conhecimento;

                    AnaliseRiscoBusiness.Inserir(oAnaliseRisco);

                    TempData["MensagemSucesso"] = "O empregado foi admitido com sucesso.";

                    //var iAdmin = oAdmissao.IDAdmissao;

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("SalvarAnaliseRisco", "AnaliseRisco") } });
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

        public ActionResult BuscarDetalhesDeMedidasDeControleTipoRisco(string IDTipoDeRisco)
        {
            ViewBag.Imagens = MedidasDeControleBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && (p.IDTipoDeRisco.Equals(IDTipoDeRisco))).ToList();
            try
            {
                MedidasDeControleExistentes oMedidasDeControleExistentes = MedidasDeControleBusiness.Consulta.FirstOrDefault(p => p.IDTipoDeRisco.Equals(IDTipoDeRisco));
                if (oMedidasDeControleExistentes == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Favor cadastrar uma medida de controle ou criar um Plano de Ação!!! ." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oMedidasDeControleExistentes) });
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






        private string RenderRazorViewToString(string viewName, object model = null)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public RetornoJSON TratarRetornoValidacaoToJSON()
        {

            string msgAlerta = string.Empty;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (System.Web.Mvc.ModelError i in item.Errors)
                    {
                        msgAlerta += i.ErrorMessage;
                    }
                }
            }

            return new RetornoJSON()
            {
                Alerta = msgAlerta,
                Erro = string.Empty,
                Sucesso = string.Empty
            };

        }

    }



}
