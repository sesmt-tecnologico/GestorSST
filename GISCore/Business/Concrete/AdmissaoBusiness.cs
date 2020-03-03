using GISCore.Business.Abstract;
using GISModel.DTO.Admissao;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace GISCore.Business.Concrete
{
   
    public class AdmissaoBusiness : BaseBusiness<Admissao>, IAdmissaoBusiness
    {
        [Inject]
        public IArquivoBusiness ArquivoBusiness { get; set; }

        [Inject]
        public IREL_ArquivoEmpregadoBusiness REL_ArquivoEmpregadoBusiness { get; set; }

        public override void Alterar(Admissao pAdmissao)
        {
            Admissao tempAdmissao = Consulta.FirstOrDefault(p => p.ID.Equals(pAdmissao.ID));
            if (tempAdmissao == null)
            {
                throw new Exception("Não foi possível encontrar o empregado através do ID.");
            }
            else
            {
                tempAdmissao.Empregado.Nome = pAdmissao.Empregado.Nome;
                tempAdmissao.Empregado.CPF = pAdmissao.Empregado.CPF;
                tempAdmissao.Empregado.DataNascimento = pAdmissao.Empregado.DataNascimento;
                
                base.Alterar(tempAdmissao);
            }

        }
       
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        Admissao IAdmissaoBusiness.GetAdmissao(Guid ukEmpregado)
        {
            var admissao = Consulta.FirstOrDefault(x => x.UKEmpregado == ukEmpregado);
            if (admissao == null)
                throw new Exception("Não foi possível encontrar um registro de admissao para esse empregado.");

            return admissao;
        }

        List<Alocacao> IAdmissaoBusiness.BuscarAlocacoes(string UKAdmissao)
        {
            List<Alocacao> lista = new List<Alocacao>();

            string query = @"select al.UniqueKey, c.Numero as Contrato, cargo.NomeDoCargo, func.NomeDaFuncao, est.Descricao as Estabelecimento, eq.NomeDaEquipe, dep.Sigla, 
                                    atv.Descricao as Atividade, al.DataInclusao, al.UsuarioInclusao, est.UniqueKey as UKEstab
                             from tbAlocacao al 
		                             inner join tbContrato c on al.UKContrato = c.UniqueKey and c.UsuarioExclusao is null
		                             inner join tbCargo cargo on al.UKCargo = cargo.UniqueKey and cargo.UsuarioExclusao is null
		                             inner join tbFuncao func on al.UKFuncao = func.UniqueKey and func.UsuarioExclusao is null
		                             inner join tbEstabelecimento est on al.UKEstabelecimento = est.UniqueKey and est.UsuarioExclusao is null
		                             inner join tbEquipe eq on al.UKEquipe = eq.UniqueKey and eq.UsuarioExclusao is null
		                             inner join tbDepartamento dep on al.UKDepartamento = dep.UniqueKey and dep.UsuarioExclusao is null
		                             left outer join REL_FuncaoAtividade fa on func.UniqueKey = fa.UKFuncao and fa.UsuarioExclusao is null
		                             left outer join tbAtividade atv on fa.UKAtividade = atv.UniqueKey and atv.UsuarioExclusao is null
                             where al.UsuarioExclusao is null and al.UKAdmissao = '" + UKAdmissao + "' ";

            DataTable result = GetDataTable(query);
            //var ctx = new GISCore.Repository.Configuration.SESTECContext();
            //var result_list = ctx.Database.SqlQuery<AlocacaoAdmissaoViewModel>(query).ToList();

            //DataTable result = ToDataTable<AlocacaoAdmissaoViewModel>(result_list.ToList());

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

                        };

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
                            }
                        };

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

        public List<Admissao> BuscarAdmissoesAtuais(string UKEmpregado)
        {
            try
            {
                List<Admissao> lista = new List<Admissao>();

                string query = @"select a.UniqueKey, a.DataAdmissao, a.DataDemissao, a.Justificativa, e.NomeFantasia as Empresa, u.Nome as NomeUsuario
                             from tbAdmissao a, tbEmpresa e, tbUsuario u
                             where a.UKEmpregado = '" + UKEmpregado + @"' and a.Status = 1 and a.UsuarioExclusao is null and
	                               a.UKEmpresa = e.UniqueKey and e.UsuarioExclusao is null and
	                               a.UsuarioInclusao = u.Login and u.UsuarioExclusao is null";

                DataTable result = GetDataTable(query);
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

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: "+ex.Message);
            }
        }

        public List<Alocacao> BuscarAlocacoes(string UKAdmissao, Guid UKEmpregado)
        {

            List<Alocacao> lista = new List<Alocacao>();

            string query = @"select al.UniqueKey, c.Numero as Contrato, cargo.NomeDoCargo, func.NomeDaFuncao, est.Descricao as Estabelecimento, eq.NomeDaEquipe, dep.Sigla, 
                                    atv.Descricao as Atividade, al.DataInclusao, al.UsuarioInclusao, est.UniqueKey as UKEstab, func.UniqueKey as UKFuncao, fa.UKAtividade as UKAtividade
                             from tbAlocacao al 
		                             inner join tbContrato c on al.UKContrato = c.UniqueKey and c.UsuarioExclusao is null
		                             inner join tbCargo cargo on al.UKCargo = cargo.UniqueKey and cargo.UsuarioExclusao is null
		                             inner join tbFuncao func on al.UKFuncao = func.UniqueKey and func.UsuarioExclusao is null
		                             inner join tbEstabelecimento est on al.UKEstabelecimento = est.UniqueKey and est.UsuarioExclusao is null
		                             inner join tbEquipe eq on al.UKEquipe = eq.UniqueKey and eq.UsuarioExclusao is null
		                             inner join tbDepartamento dep on al.UKDepartamento = dep.UniqueKey and dep.UsuarioExclusao is null
		                             left outer join REL_FuncaoAtividade fa on func.UniqueKey = fa.UKFuncao and fa.UsuarioExclusao is null
		                             left outer join tbAtividade atv on fa.UKAtividade = atv.UniqueKey and atv.UsuarioExclusao is null
                             where al.UsuarioExclusao is null and al.UKAdmissao = '" + UKAdmissao + "' ";

            DataTable result = GetDataTable(query);
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

                        al.ArquivoEmpregado = RetonarListaArquivoEmpregado(al.UniqueKey, UKEmpregado, al.Funcao.UniqueKey);

                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            {
                                UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                Descricao = row["Atividade"].ToString()
                            });
                        }

                        foreach (var itm in al.Funcao.Atividades)
                        {
                            itm.DocumentosPessoal = ListaDocumentosPessoal(itm.UniqueKey.ToString());
                        }

                    }
                    else if (al.UniqueKey.ToString().Equals(row["UniqueKey"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            { 
                                UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                Descricao = row["Atividade"].ToString()
                            });
                        }

                        foreach (var itm in al.Funcao.Atividades)
                        {
                            itm.DocumentosPessoal = ListaDocumentosPessoal(itm.UniqueKey.ToString());
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

                        al.ArquivoEmpregado = RetonarListaArquivoEmpregado(al.UniqueKey, UKEmpregado, al.Funcao.UniqueKey);

                        if (!string.IsNullOrEmpty(row["Atividade"].ToString()))
                        {
                            al.Funcao.Atividades.Add(new Atividade()
                            {
                                UniqueKey = Guid.Parse(row["UKAtividade"].ToString()),
                                Descricao = row["Atividade"].ToString()
                            });
                        }

                        foreach (var itm in al.Funcao.Atividades)
                        {
                            itm.DocumentosPessoal = ListaDocumentosPessoal(itm.UniqueKey.ToString());
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

        public List<ArquivoEmpregadoViewModel> RetonarListaArquivoEmpregado(Guid ukLocado, Guid ukEmpregado, Guid ukFuncao)
        {
            var lista = new List<ArquivoEmpregadoViewModel>();

            var listaArquivoEmpregado = this.REL_ArquivoEmpregadoBusiness.Consulta
                           .Where(x => x.UKLocacao == ukLocado
                           && x.UKEmpregado == ukEmpregado
                           && x.UKFuncao == ukFuncao && x.UsuarioExclusao == null).ToList();
            if (listaArquivoEmpregado != null)
            {
                foreach (var arqemp in listaArquivoEmpregado)
                {
                    var arquivo = this.ArquivoBusiness.Consulta.SingleOrDefault(s => s.UniqueKey == arqemp.UKObjetoArquivo && arqemp.UsuarioExclusao == null);
                    if (arquivo != null)
                    {
                        lista.Add(new ArquivoEmpregadoViewModel()
                        {
                            NomeLocal = arquivo.NomeLocal,
                            NomeRemoto = arquivo.NomeRemoto,
                            UniqueKey = arqemp.UniqueKey,
                            UKLocacao = arqemp.UKLocacao,
                            UKEmpregado = arqemp.UKEmpregado,
                            UKFuncao = arqemp.UKFuncao,
                            UKObjectArquivo = arqemp.UKObjetoArquivo
                        });
                    }
                }
            }

            return lista;
        }

        public List<DocumentosPessoal> ListaDocumentosPessoal(string ukAtividade)
        {

            var lista = new List<DocumentosPessoal>();

            string query = $@"select d.NomeDocumento from rel_DocumentoPessoalAtividade r 
	        join tbAtividade a on a.UniqueKey = r.UKAtividade and a.UsuarioExclusao is null
	        join tbDocumentosPessoal d on d.UniqueKey = r.UKDocumentoPessoal and d.UsuarioExclusao is null
	        where r.UKAtividade = '{ukAtividade}' and r.UsuarioExclusao is null";

            DataTable result = GetDataTable(query);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    lista.Add(new DocumentosPessoal() { NomeDocumento = item["NomeDocumento"].ToString() });
                }
            }

            return lista;
        }
    }
}
