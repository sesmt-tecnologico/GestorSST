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
                        
            pAdmissao.Status = "Admitido";
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
                
                base.Alterar(tempAdmissao);
            }

        }

    }
}
