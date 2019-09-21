using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class DocsPorAtividadeBusiness : BaseBusiness<DocsPorAtividade>, IDocsPorAtividadeBusiness
    {

        public override void Alterar(DocsPorAtividade pDocsPorAtividade)
        {
            DocsPorAtividade tempDocsPorAtividade = Consulta.FirstOrDefault(p => p.ID.Equals(pDocsPorAtividade.ID));
            if (tempDocsPorAtividade == null)
            {
                throw new Exception("Não foi possível encontrar este Documento");
            }

            tempDocsPorAtividade.idAtividade = pDocsPorAtividade.idAtividade;
            tempDocsPorAtividade.idDocumentosEmpregado = pDocsPorAtividade.idDocumentosEmpregado;

            base.Alterar(tempDocsPorAtividade);
        }

    }
}
