using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EventoPerigosoBusiness : BaseBusiness<EventoPerigoso>, IEventoPerigosoBusiness
    {

        public override void Alterar(EventoPerigoso pEvento)
        {

            EventoPerigoso tempEvento = Consulta.FirstOrDefault(p => p.ID.Equals(pEvento.ID));
            if (tempEvento == null)
            {
                throw new Exception("Não foi possível encontrar o Evento Perigoso através do ID.");
            }
            else
            {
                tempEvento.Descricao = pEvento.Descricao;
                base.Alterar(tempEvento);
            }

        }

    }
}
