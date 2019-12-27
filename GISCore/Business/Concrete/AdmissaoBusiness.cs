using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
   
    public class AdmissaoBusiness : BaseBusiness<Admissao>, IAdmissaoBusiness
    {
        

        public override void Inserir(Admissao pAdmissao)
        {
                        
            pAdmissao.Admitido = "Admitido";
            base.Inserir(pAdmissao);
        }

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
                tempAdmissao.Imagem = pAdmissao.Imagem;
                
                base.Alterar(tempAdmissao);
            }

        }

    }
}
