using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.DTO.EPI;
using GISModel.Entidades;
using GISModel.Entidades.Estoques;
using GISModel.Entidades.PPRA;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using GISWeb.Infraestrutura.Provider.Abstract;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;
using Rotativa;

namespace GISWeb.Controllers.PPRA
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FichaDeEpiController : BaseController
    {
        #region

        [Inject]
        public IBaseBusiness<FichaDeEPI> FichaDeEpiBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Admissao> AdmissaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Alocacao> AlocacaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Cargo> CargoBusiness { get; set; }


        [Inject]
        public IBaseBusiness<Produto> ProdutoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Categoria> CategoriaBusiness { get; set; }


        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        // GET: Epi
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FichaEpi(string idEmp)
        {
            List<Produto> prod = ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.epi =new SelectList( prod, "UniqueKey","Nome");
           

            Guid ukemp = Guid.Parse(idEmp);

            Empregado empregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ukemp));

            ViewBag.NomeEmpregado = empregado.Nome;

            ViewBag.Emp = ukemp;

            var enumData02 = from EMotivoDevolucao e in Enum.GetValues(typeof(EMotivoDevolucao))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.GetDisplayName()
                             };
            ViewBag.Motivo = new SelectList(enumData02, "ID", "Name");


            return View();
        }


        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarEPI(EpiViewModel oEpi, string idEmp)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    //FichaDeEPI fepi = FichaDeEpiBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                    //&& a.UniqueKey.Equals(oEpi.Uniquekey));

                    Produto produto = ProdutoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                     && a.UniqueKey.Equals(oEpi.UKProduto));

                    var Total = produto.Qunatidade - oEpi.Quantidade;

                    produto.Qunatidade = Total;
                    ProdutoBusiness.Alterar(produto);
                    

                    FichaDeEPI obj = new FichaDeEPI()
                    {
                        
                        UKEmpregado =Guid.Parse(idEmp),
                        UKProduto = oEpi.UKProduto,
                        CA = oEpi.CA,
                        Quantidade = oEpi.Quantidade,                        
                        MotivoDevolucao = oEpi.MotivoDevolucao                       

                        
                    };

                    FichaDeEpiBusiness.Inserir(obj);

                    Extensions.GravaCookie("MensagemSucesso", "O EPI  foi cadastrado com sucesso.", 10);



                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("FichaEpi", "FichaDeEpi", new { idEmp = idEmp.ToString() }) } });
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

        
        public ActionResult FichaDeEpiPorEmpregado(string idEmp)
        {
            Guid ukemp = Guid.Parse(idEmp);

            List<string> oProd = new List<string>();

            var admissao = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(ukemp));

            var empresa = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(admissao.UKEmpresa));


            var aloc =( from a in AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                       join c in CargoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                       on a.UKCargo equals c.UniqueKey
                       where a.UKAdmissao.Equals(admissao.UniqueKey)
                       select new Cargo()
                       {
                           NomeDoCargo = c.NomeDoCargo

                       }).Take(1);


            List<string> carg = new List<string>();

            foreach( var item in aloc)
            {
                if (item != null)
                {

                    carg.Add(item.NomeDoCargo);

                                                                                                
                }

            }

            ViewBag.cargo = carg.ToList();

            ViewBag.empresa = empresa.NomeFantasia;

            //var empresa = CustomAuthorizationProvider.UsuarioAutenticado.Empresa;

            //var categoria = CategoriaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            var Ficha = from f in FichaDeEpiBusiness.Consulta.Where(a=>string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on f.UKEmpregado equals e.UniqueKey
                        join p in ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on f.UKProduto equals p.UniqueKey
                        join c in CategoriaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on p.UKCategoria equals c.UniqueKey
                        where e.UniqueKey.Equals(ukemp) 
                        select new FichaDeEPIViewModel()
                        {
                            UKFicahaDeEPI = f.UniqueKey,
                            UKEmpregado = e.UniqueKey,
                            Nome = e.Nome,
                            CA = f.CA,
                            Produto = p.Nome, 
                            UKProduto = p.UniqueKey,
                            DataEntrega = p.DataInclusao,
                            DataDevolucao = p.DataExclusao,
                            Quantidade = f.Quantidade,
                            MotivoDevolucao = f.MotivoDevolucao
                                                        
                           
                         };

        


            ViewBag.ficha = Ficha.ToList();

            if(Ficha != null)
            {
                foreach (var item in Ficha)
                {
                    if(item != null)
                    {
                        oProd.Add(item.Produto);
                    }

                }
            }

            ViewBag.epi = oProd.ToList();


           List<Produto> produto = ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

           

            return View(Ficha.ToList());
        }


        public ActionResult FichaDeEpiPorEmpregadoPDF()
        {
            Guid ukemp = Guid.Parse("82F74A42-69A1-44EC-9B68-6E9B7789F3CE");

            var Ficha = from f in FichaDeEpiBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on f.UKEmpregado equals e.UniqueKey
                        join p in ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on f.UKProduto equals p.UniqueKey
                        where e.UniqueKey.Equals(ukemp)
                        select new FichaDeEPIViewModel()
                        {
                            Nome = e.Nome,
                            Produto = p.Nome,
                           Quantidade = f.Quantidade,
                            MotivoDevolucao = f.MotivoDevolucao
                            
                        };

            var relatorioPDF = new ViewAsPdf
            {
                ViewName = "FichaDeEpiPorEmpregado",
                IsGrayScale = true,
                //Model = ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                Model = Ficha.ToList()


            };
            return relatorioPDF;
        }

    }
}