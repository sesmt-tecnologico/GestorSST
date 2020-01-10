using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class CargoesBusiness : BaseBusiness<Cargoes>, ICargoesBusiness
    {

        public override void Inserir(Cargoes pCargo)
        {
            if (Consulta.Any(u => u.ID.Equals(pCargo.ID)))
                throw new InvalidOperationException("Não é possível inserir o Cargo, pois já existe um Cargo com este ID.");

            base.Inserir(pCargo);
        }

        public override void Alterar(Cargoes pCargo)
        {
            Cargoes tempCargo = Consulta.FirstOrDefault(p => p.ID.Equals(pCargo.ID));

            if (tempCargo == null)
            {
                throw new Exception("não foi possível encontrar este Cargo");
            }

            tempCargo.NomeDoCargo = pCargo.NomeDoCargo;
            
            base.Alterar(tempCargo);
        }

    }
}
