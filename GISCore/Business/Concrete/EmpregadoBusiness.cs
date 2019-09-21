using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EmpregadoBusiness : BaseBusiness<Empregado>, IEmpregadoBusiness
    {

        public override void Alterar(Empregado pEmpregado)
        {
            Empregado tempEmpregado = Consulta.FirstOrDefault(p => p.ID.Equals(pEmpregado.ID));
            if (tempEmpregado == null)
            {
                throw new Exception("Não foi possível encontrar o Empregado.");
            }

            tempEmpregado.Nome = pEmpregado.Nome;
            tempEmpregado.CPF = pEmpregado.CPF;
            tempEmpregado.DataNascimento = pEmpregado.DataNascimento;
            tempEmpregado.Email = pEmpregado.Email;

            base.Alterar(tempEmpregado);
        }

    }


}
