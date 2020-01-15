using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class CargoBusiness : BaseBusiness<Cargo>, ICargoBusiness
    {

        public override void Inserir(Cargo pCargo)
        {
            if (Consulta.Any(u => u.ID.Equals(pCargo.ID)))
                throw new InvalidOperationException("Não é possível inserir o Cargo, pois já existe um Cargo com este ID.");

            base.Inserir(pCargo);
        }

        public override void Alterar(Cargo pCargo)
        {
            Cargo tempCargo = Consulta.FirstOrDefault(p => p.ID.Equals(pCargo.ID));

            if (tempCargo == null)
            {
                throw new Exception("não foi possível encontrar este Cargo");
            }

            tempCargo.NomeDoCargo = pCargo.NomeDoCargo;
            
            base.Alterar(tempCargo);
        }

    }
}
