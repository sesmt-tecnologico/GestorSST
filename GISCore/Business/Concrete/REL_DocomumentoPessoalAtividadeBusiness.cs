using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class REL_DocomumentoPessoalAtividadeBusiness : BaseBusiness<REL_DocomumentoPessoalAtividade>, IREL_DocomumentoPessoalAtividadeBusiness
    {

        public override void Alterar(REL_DocomumentoPessoalAtividade pDocsPorAtividade)
        {
            REL_DocomumentoPessoalAtividade tempDocsPorAtividade = Consulta.FirstOrDefault(p => p.ID.Equals(pDocsPorAtividade.ID));
            if (tempDocsPorAtividade == null)
            {
                throw new Exception("Não foi possível encontrar este Documento");
            }

            tempDocsPorAtividade.UKAtividade = pDocsPorAtividade.UKAtividade;
            tempDocsPorAtividade.UKDocumentoPessoal = pDocsPorAtividade.UKDocumentoPessoal;

            base.Alterar(tempDocsPorAtividade);
        }

    }
}
