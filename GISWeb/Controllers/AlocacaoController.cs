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
using System.Data;
using GISModel.DTO.DocumentosAlocacao;
using System.Collections.Generic;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AlocacaoController : BaseController
    {

        #region
        
        [Inject]
        public IEquipeBusiness EquipeBusiness { get; set; }

        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IContratoBusiness ContratoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public IBaseBusiness<DocumentosPessoal> DocumentosPessoalBusiness { get; set; }

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }


        [Inject]
        public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_ContratoFornecedor> ContratoFornecedorBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_DocumentosAlocados> REL_DocumentosAlocadoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_EstabelecimentoDepartamento> REL_EstabelecimentoDepartamentoBusiness { get; set; }




        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Novo(string id)
        {
            try
            {
                Guid UKAdmissao = Guid.Parse(id);
                Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKAdmissao));
                Empresa oEmpresa = EmpresaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(oAdmissao.UKEmpresa));
                Departamento oDepartamento = DepartamentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(oEmpresa.UniqueKey));

                Alocacao obj = new Alocacao();
                obj.UKAdmissao = oAdmissao.UniqueKey;

                ViewBag.Contratos = (from contForn in ContratoFornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKFornecedor.Equals(oAdmissao.UKEmpresa)).ToList()
                                     join cont in ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on contForn.UKContrato equals cont.UniqueKey
                                     select cont).ToList();

                ViewBag.Cargos = CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                ViewBag.Estabelecimentos = (from e in EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                            join re in REL_EstabelecimentoDepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                            on e.UniqueKey equals re.UKEstabelecimento
                                            where re.UKDepartamento.Equals(oDepartamento.UniqueKey)
                                            select e).ToList();


                    
                    
                    ViewBag.Equipes = EquipeBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(oAdmissao.UKEmpresa)).ToList();

                return PartialView("_Novo", obj);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Alocacao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {



                        Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(entidade.UKAdmissao));
                    if (oAdmissao == null)
                        throw new Exception("Não foi possível encontrar a admissão na base de dados.");


                    Empregado emp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(oAdmissao.UKEmpregado));
                    if (emp == null)
                        throw new Exception("Não foi possível encontrar o empregado na base de dados.");




                    if (AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                                             a.UKAdmissao.Equals(entidade.UKAdmissao) &&
                                                             a.UKCargo.Equals(entidade.UKCargo) &&
                                                             a.UKFuncao.Equals(entidade.UKFuncao)).Count() > 0) {
                        throw new Exception("Já existe uma alocação deste empregado neste cargo e função selecionado.");
                    }
                    else
                    {
                        entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        AlocacaoBusiness.Inserir(entidade);


                        Usuario usr = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Login.Equals(emp.CPF.Replace(".", "").Replace("-", "")));
                        if (usr == null)
                        {
                            // string Senha = GISHelpers.Utils.Severino.GeneratePassword();

                            string Senha = "escola10";

                            usr = new Usuario()
                            {
                                UniqueKey = Guid.NewGuid(),
                                CPF = emp.CPF,
                                Nome = emp.Nome,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                Email = emp.Email,
                                Login = emp.CPF.Replace(".", "").Replace("-", ""),
                                Senha = Senha,
                                TipoDeAcesso = GISModel.Enums.TipoDeAcesso.Sistema,
                                UKEmpresa = oAdmissao.UKEmpresa,
                                UKDepartamento = entidade.UKDepartamento
                            };

                            UsuarioBusiness.Inserir(usr);

                            Perfil per = PerfilBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals("Empregado"));
                            if (per != null)
                            {
                                UsuarioPerfilBusiness.Inserir(new UsuarioPerfil()
                                {
                                    UKPerfil = per.UniqueKey,
                                    UKUsuario = usr.UniqueKey,
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                                    UKConfig = entidade.UKDepartamento
                                });
                            }
                        }

                        //Cadastrando os documentos relacionados com a atividade alocada.


                        var aloc = @"select top 1  UniqueKey, DataInclusao  from tbAlocacao LAST_INSET_ID ORDER BY DataInclusao DESC  ";

                        List<Alocacao> lista1 = new List<Alocacao>();

                        DataTable result1 = DocumentosPessoalBusiness.GetDataTable(aloc);

                        if (result1.Rows.Count > 0)
                        {

                            Alocacao UkAloc = null;
                            Guid ukal = Guid.Empty;

                            foreach (DataRow row in result1.Rows)
                            {

                                if (UkAloc == null)
                                {
                                    UkAloc = new Alocacao()
                                    {
                                        UniqueKey = Guid.Parse(row["UniqueKey"].ToString())
                                    };

                                }

                            }

                            if (UkAloc != null)
                                lista1.Add(UkAloc);

                            foreach (var item in lista1)
                            {
                                if (item != null)
                                {
                                    ukal = item.UniqueKey;
                                }
                            }



                        List<Alocacao> lista = new List<Alocacao>();


                        var sql = @"select al.UniqueKey as UKal, fa.UniqueKey as UKfa, d.UniqueKey as UKd, d.NomeDocumento as Nome,
                        da.UniqueKey as UKda
                        from tbAlocacao al
                        join REL_FuncaoAtividade fa
                        on al.UKFuncao = fa.UKFuncao
                        join REL_DocumentoPessoalAtividade da
                        on fa.UKAtividade = da.UKAtividade
                        join tbDocumentosPessoal d
                        on da.UKDocumentoPessoal = d.UniqueKey and d.DataExclusao = '9999-12-31 23:59:59.997'
                        where al.UniqueKey = '" + ukal + @"'
                        order by al.UniqueKey ";

                        DataTable result = DocumentosPessoalBusiness.GetDataTable(sql);

                        if (result.Rows.Count > 0)
                        {

                            Alocacao obj = null;
                            DocumentosPessoal oDoc = null;

                            foreach (DataRow row in result.Rows)
                            {

                                if (obj == null)
                                {
                                    obj = new Alocacao()
                                    {
                                        UniqueKey = Guid.Parse(row["UKal"].ToString()),
                                        DocumentosPessoal = new List<DocumentosPessoal>()
                                    };

                                    if (!string.IsNullOrEmpty(row["UKd"].ToString()))
                                    {
                                        oDoc = new DocumentosPessoal()
                                        {
                                            UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                            NomeDocumento = row["nome"].ToString(),

                                        };


                                        obj.DocumentosPessoal.Add(oDoc);
                                    }
                                }
                                else if (obj.UniqueKey.Equals(Guid.Parse(row["UKal"].ToString())))
                                {
                                    if (!string.IsNullOrEmpty(row["UKda"].ToString()))
                                    {
                                        if (oDoc == null)
                                        {
                                            oDoc = new DocumentosPessoal()
                                            {
                                                UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                                NomeDocumento = row["nome"].ToString(),

                                            };

                                            obj.DocumentosPessoal.Add(oDoc);
                                        }


                                        else
                                        {
                                            oDoc = new DocumentosPessoal()
                                            {
                                                UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                                NomeDocumento = row["nome"].ToString(),

                                            };

                                            obj.DocumentosPessoal.Add(oDoc);
                                        }


                                    }
                                }
                                else
                                {
                                    lista.Add(obj);

                                    obj = new Alocacao()
                                    {
                                        UniqueKey = Guid.Parse(row["UKal"].ToString()),
                                        DocumentosPessoal = new List<DocumentosPessoal>()
                                    };

                                    if (!string.IsNullOrEmpty(row["UKd"].ToString()))
                                    {
                                        oDoc = new DocumentosPessoal()
                                        {
                                            UniqueKey = Guid.Parse(row["UKd"].ToString()),
                                            NomeDocumento = row["nome"].ToString(),

                                        };


                                        obj.DocumentosPessoal.Add(oDoc);
                                    }
                                }
                            }

                            if (obj != null)
                                lista.Add(obj);


                            if (lista == null)
                                throw new Exception("Nenhum Documento para vincular.");

                            string documento = string.Empty;

                            foreach (var item in obj.DocumentosPessoal)
                            {
                                if (item != null)
                                {

                                    documento += item.NomeDocumento + ",";

                                }
                            }


                            if (documento.Contains(","))
                            {
                                documento = documento.Remove(documento.Length - 1);

                                foreach (string ativ in documento.Split(','))
                                {
                                    if (!string.IsNullOrEmpty(ativ.Trim()))
                                    {
                                        DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(ativ.Trim()));
                                        if (pTemp != null)
                                        {
                                            if (REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumento.Equals(pTemp.UniqueKey) && a.UKAlocacao.Equals(ukal)).Count() == 0)
                                            {
                                                REL_DocumentosAlocadoBusiness.Inserir(new REL_DocumentosAlocados()
                                                {
                                                    Posicao = 0,
                                                    UKAlocacao = ukal,
                                                    UKDocumento = pTemp.UniqueKey,
                                                    DataDocumento = DateTime.MaxValue,
                                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                                });

                                            }
                                            //else
                                            //{
                                            //    return Json(new { resultado = new RetornoJSON() { Erro = "Este documento já está cadastrado para esta alocação!." } });
                                            //}
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DocumentosPessoal pTemp = DocumentosPessoalBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeDocumento.Equals(documento.Trim()));
                                if (pTemp != null)
                                {
                                   
                                    if (REL_DocumentosAlocadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDocumento.Equals(pTemp.UniqueKey) && a.UKAlocacao.Equals(ukal)).Count() == 0)
                                    {
                                        REL_DocumentosAlocadoBusiness.Inserir(new REL_DocumentosAlocados()
                                        {
                                            UKAlocacao = ukal,
                                            UKDocumento = pTemp.UniqueKey,
                                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                        });
                                    }
                                }
                            }
                        } }


                            Extensions.GravaCookie("MensagemSucesso", "O empregado foi alocado com sucesso.", 10);

                            return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = oAdmissao.UKEmpregado.ToString() }) } });
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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult Desalocar(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Não foi possível localizar a identificação da alocação para prosseguir com a operação.");

                Guid UKAlocacao = Guid.Parse(id);
                Alocacao al = AlocacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKAlocacao));
                if (al == null)
                    throw new Exception("Não foi possível encontrar a alocação na base de dados.");

                Admissao ad = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(al.UKAdmissao));
                if (ad == null)
                    throw new Exception("Não foi possível encontrar a admissão onde o empregado está alocado na base de dados.");

                al.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                AlocacaoBusiness.Terminar(al);

                //var da = REL_DocumentosAlocadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKAlocacao.Equals(UKAlocacao)).ToList(); 
                //foreach(var item in da)
                //{
                //    if(da != null)
                //    {
                //        item.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                //        REL_DocumentosAlocadoBusiness.Terminar(item);
                //    }
                //}

                Extensions.GravaCookie("MensagemSucesso", "O empregado foi desalocado com sucesso.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = ad.UKEmpregado.ToString() }) } });
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
