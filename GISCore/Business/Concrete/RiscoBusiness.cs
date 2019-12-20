using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class RiscoBusiness : BaseBusiness<Risco>, IRiscoBusiness
    {

        public override void Alterar(Risco pEvento)
        {

            Risco tempRisco = Consulta.FirstOrDefault(p => p.ID.Equals(pEvento.ID));
            if (tempRisco == null)
            {
                throw new Exception("Não foi possível encontrar o Evento Perigoso através do ID.");
            }
            else
            {
                tempRisco.Nome = pEvento.Nome;
                base.Alterar(tempRisco);
            }

        }

    }
}
