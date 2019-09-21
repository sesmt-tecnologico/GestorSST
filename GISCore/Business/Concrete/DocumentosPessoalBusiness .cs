using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class DocumentosPessoalBusiness : BaseBusiness<DocumentosPessoal>, IDocumentosPessoalBusiness
    {
        public override void Alterar(DocumentosPessoal pDocumentosPessoal)
        {
            DocumentosPessoal tempDocumentosPessoal = Consulta.FirstOrDefault(p => p.ID.Equals(pDocumentosPessoal.ID));
            if (tempDocumentosPessoal == null)
            {
                throw new Exception("Não foi possível encontrar este Documento");
            }

            tempDocumentosPessoal.NomeDocumento = pDocumentosPessoal.NomeDocumento;
            base.Alterar(tempDocumentosPessoal);
        }

    }
}
