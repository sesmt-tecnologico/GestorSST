using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class ContratoBusiness : BaseBusiness<Contrato>, IContratoBusiness
    {

        public override void Alterar(Contrato pContrato)
        {

            Contrato tempContrato = Consulta.FirstOrDefault(p => p.ID.Equals(pContrato.ID));
            if (tempContrato == null)
            {
                throw new Exception("Não foi possível encontrar o Contrato através do ID.");
            }
            else
            {
                tempContrato.DescricaoContrato = pContrato.DescricaoContrato;
                base.Alterar(tempContrato);
            }
        }

    }

}
        

