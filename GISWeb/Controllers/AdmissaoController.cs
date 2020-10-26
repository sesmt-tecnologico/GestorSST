using GISCore.Business.Abstract;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Admissao;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdmissaoController : BaseController
    {

        #region

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IAlocacaoBusiness AlocacaoBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IArquivoBusiness ArquivoBusiness { get; set; }
        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public IPerigoBusiness PerigoBusiness { get; set; }
        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IAtividadeBusiness AtividaeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_FuncaoAtividade> FuncaoAtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AtividadePerigo> AtividadePerigoBusiness { get; set; }
        [Inject]
        public IBaseBusiness<REL_PerigoRisco> PerigoRiscoBusiness { get; set; }

        [Inject]
        public IREL_ArquivoEmpregadoBusiness REL_ArquivoEmpregadoBusiness { get; set; }


        #endregion



        public ActionResult Novo(string id)
        {
            try
            {
                var UKEmpregado = Guid.Parse(id);

                List<Empresa> empresas = EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Fornecedor).ToList();
                List<Admissao> admissoesAtivas = AdmissaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(UKEmpregado) && string.IsNullOrEmpty(a.DataDemissao)).ToList();

                //foreach (Admissao adm in admissoesAtivas)
                //{
                //    empresas.RemoveAll(a => a.UniqueKey.Equals(adm.UKEmpresa));
                //}

                if (empresas.Count == 0)
                    throw new Exception("Nenhuma empresa disponível para admissão.");

                ViewBag.Empresas = empresas;
                ViewBag.PossuiAdmissaoAtiva = admissoesAtivas.Count() > 0;

                Admissao obj = new Admissao()
                {
                    UKEmpregado = UKEmpregado
                };

                return PartialView("_Novo", obj);
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Admissao entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UKEmpregado));
                    if (oEmp == null)
                        throw new Exception("Não foi possível encontrar o empregado relacionado a admissão.");

                    entidade.Status = GISModel.Enums.Situacao.Ativo;
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    AdmissaoBusiness.Inserir(entidade);

                    oEmp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    EmpregadoBusiness.Terminar(oEmp);

                    EmpregadoBusiness.Inserir(new Empregado()
                    {
                        CPF = oEmp.CPF,
                        Nome = oEmp.Nome,
                        DataNascimento = oEmp.DataNascimento,
                        Email = oEmp.Email,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                        UniqueKey = oEmp.UniqueKey,
                        Status = "Atualmente admitido"
                    });


                    Extensions.GravaCookie("MensagemSucesso", "Admissão realizada com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = entidade.UKEmpregado.ToString() }) } });
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
        public ActionResult BuscarAdmissoesAtuais(string UKEmpregado)
        {
            try
            {
               

                
                

                List <Admissao> lista = new List<Admissao>();

                string query = @"select a.UniqueKey, a.DataAdmissao, a.DataDemissao, a.Justificativa, e.NomeFantasia as Empresa, u.Nome as NomeUsuario
                             from tbAdmissao a, tbEmpresa e, tbUsuario u

                             where a.UKEmpregado = '" + UKEmpregado + @"' and a.Status = 1 and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                               a.UKEmpresa = e.UniqueKey and e.DataExclusao = '9999-12-31 23:59:59.997' and
	                               a.UsuarioInclusao = u.Login and u.DataExclusao = '9999-12-31 23:59:59.997'";


                DataTable result = AdmissaoBusiness.GetDataTable(query);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        Admissao adm = new Admissao()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            UKEmpregado = Guid.Parse(UKEmpregado.ToString()),
                            DataAdmissao = row["DataAdmissao"].ToString(),
                            DataDemissao = row["DataDemissao"].ToString(),
                            Justificativa = row["Justificativa"].ToString(),
                            Empresa = new Empresa()
                            {
                                NomeFantasia = row["Empresa"].ToString()
                            },
                            UsuarioInclusao = row["NomeUsuario"].ToString(),
                            Alocacoes = BuscarAlocacoes(row["UniqueKey"].ToString(), Guid.Parse(UKEmpregado.ToString()))
                        };

                        lista.Add(adm);
                    }
                }

                return PartialView("_BuscarAdmissoesAtuais", lista);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }
        }

        private List<Alocacao> BuscarAlocacoes(string UKAdmissao, Guid UKEmpregado)
        {
            List<Alocacao> lista = new List<Alocacao>();

            string query = @"select al.UniqueKey, c.Numero as Contrato, cargo.NomeDoCargo, func.NomeDaFuncao, est.Descricao as Estabelecimento, eq.NomeDaEquipe, dep.Sigla, 
                                    atv.Descricao as Atividade, al.DataInclusao, al.UsuarioInclusao, est.UniqueKey as UKEstab, func.UniqueKey as UKFuncao
                             from tbAlocacao al 
		                             inner join tbContrato c on al.UKContrato = c.UniqueKey and c.DataExclusao = '9999-12-31 23:59:59.997'
		                             inner join tbCargo cargo on al.UKCargo = cargo.UniqueKey and cargo.DataExclusao = '9999-12-31 23:59:59.997'
		                             inner join tbFuncao func on al.UKFuncao = func.UniqueKey and func.DataExclusao = '9999-12-31 23:59:59.997'
		                             inner join tbEstabelecimento est on al.UKEstabelecimento = est.UniqueKey and est.DataExclusao = '9999-12-31 23:59:59.997'
		                             inner join tbEquipe eq on al.UKEquipe = eq.UniqueKey and eq.DataExclusao = '9999-12-31 23:59:59.997'
		                             inner join tbDepartamento dep on al.UKDepartamento = dep.UniqueKey and dep.DataExclusao = '9999-12-31 23:59:59.997'
		                             left outer join REL_FuncaoAtividade fa on func.UniqueKey = fa.UKFuncao and fa.DataExclusao = '9999-12-31 23:59:59.997'
		                             left outer join tbAtividade atv on fa.UKAtividade = atv.UniqueKey and atv.DataExclusao = '9999-12-31 23:59:59.997'
                             where al.DataExclusao = '9999-12-31 23:59:59.997' and al.UKAdmissao = '" + UKAdmissao + "' ";

            DataTable result = AdmissaoBusiness.GetDataTable(query);
            if (result.Rows.Count > 0)
            {
                Alocacao al = null;

                foreach (DataRow row in result.Rows)
                {

                    if (al == null)
                    {
                        al = new Alocacao()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            DataInclusao = DateTime.Parse(row["DataInclusao"].ToString()),
                            UsuarioInclusao = row["UsuarioInclusao"].ToString(),
                            Contrato = new Contrato()
                            {
                                Numero = row["Contrato"].ToString()
                            },
                            Cargo = new Cargo()
                            {
                                NomeDoCargo = row["NomeDoCargo"].ToString()
                            },
                            Funcao = new Funcao()
                            {
                                UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                NomeDaFuncao = row["NomeDaFuncao"].ToString(),
                                Atividades = new List<Atividade>()
                            },
                            Estabelecimento = new Estabelecimento()
                            {
                                UniqueKey = Guid.Parse(row["UKEstab"].ToString()),
                                Descricao = row["Estabelecimento"].ToString()
                            },
                            Equipe = new Equipe()
                            {
                                NomeDaEquipe = row["NomeDaEquipe"].ToString()
                            },
                            Departamento = new Departamento()
                            {
                                Sigla = row["Sigla"].ToString()
                            },
                            ArquivoEmpregado = new List<ArquivoEmpregadoViewModel>()
                        };

                        al.ArquivoEmpregado = this.AdmissaoBusiness.RetonarListaArquivoEmpregado(al.UniqueKey, UKEmpregado, al.Funcao.UniqueKey);

                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            {
                                Descricao = row["Atividade"].ToString()
                            });
                        }

                    }
                    else if (al.UniqueKey.ToString().Equals(row["UniqueKey"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            {
                                Descricao = row["Atividade"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lista.Add(al);

                        al = new Alocacao()
                        {
                            UniqueKey = Guid.Parse(row["UniqueKey"].ToString()),
                            DataInclusao = DateTime.Parse(row["DataInclusao"].ToString()),
                            UsuarioInclusao = row["UsuarioInclusao"].ToString(),
                            Contrato = new Contrato()
                            {
                                Numero = row["Contrato"].ToString()
                            },
                            Cargo = new Cargo()
                            {
                                NomeDoCargo = row["NomeDoCargo"].ToString()
                            },
                            Funcao = new Funcao()
                            {
                                UniqueKey = Guid.Parse(row["UKFuncao"].ToString()),
                                NomeDaFuncao = row["NomeDaFuncao"].ToString(),
                                Atividades = new List<Atividade>()
                            },
                            Estabelecimento = new Estabelecimento()
                            {
                                UniqueKey = Guid.Parse(row["UKEstab"].ToString()),
                                Descricao = row["Estabelecimento"].ToString()
                            },
                            Equipe = new Equipe()
                            {
                                NomeDaEquipe = row["NomeDaEquipe"].ToString()
                            },
                            Departamento = new Departamento()
                            {
                                Sigla = row["Sigla"].ToString()
                            },
                            ArquivoEmpregado = new List<ArquivoEmpregadoViewModel>()
                        };

                        al.ArquivoEmpregado = this.AdmissaoBusiness.RetonarListaArquivoEmpregado(al.UniqueKey, UKEmpregado, al.Funcao.UniqueKey);

                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            {
                                Descricao = row["Atividade"].ToString()
                            });
                        }

                    }
                } // Fim foreach

                if (al != null)
                {
                    lista.Add(al);
                }
            }

            return lista;
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult Demitir(string id)
        {
            try
            {

                //######################################################################################################

                if (string.IsNullOrEmpty(id))
                    throw new Exception("Não foi possível localizar a identificação da admissão para prosseguir com a operação.");

                Guid UKAdmissao = Guid.Parse(id);
                Admissao adm = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKAdmissao));
                if (adm == null)
                    throw new Exception("Não foi possível encontrar a admissão na base de dados.");

                Empregado oEmp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(adm.UKEmpregado));
                if (oEmp == null)
                    throw new Exception("Não foi possível encontrar o empregado na base de dados.");

                //######################################################################################################


                List<Alocacao> als = AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKAdmissao.Equals(UKAdmissao)).ToList();
                if (als.Count > 0)
                {
                    foreach (Alocacao al in als)
                    {
                        al.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                        AlocacaoBusiness.Terminar(al);
                    }
                }

                adm.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                AdmissaoBusiness.Terminar(adm);



                oEmp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                EmpregadoBusiness.Terminar(oEmp);

                EmpregadoBusiness.Inserir(new Empregado()
                {
                    CPF = oEmp.CPF,
                    Nome = oEmp.Nome,
                    DataNascimento = oEmp.DataNascimento,
                    Email = oEmp.Email,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login,
                    UniqueKey = oEmp.UniqueKey,
                    Status = "Já admitido alguma vez"
                });



                Extensions.GravaCookie("MensagemSucesso", "O empregado foi demitido com sucesso.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Perfil", "Empregado", new { id = adm.UKEmpregado.ToString() }) } });
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
