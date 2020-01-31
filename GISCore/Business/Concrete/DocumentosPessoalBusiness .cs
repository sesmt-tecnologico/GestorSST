using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class DocumentosPessoalBusiness : BaseBusiness<DocumentosPessoal>, IDocumentosPessoalBusiness
    {

        public override void Inserir(DocumentosPessoal pDocumento)
        {

           

            base.Inserir(pDocumento);
        }

        public override void Alterar(DocumentosPessoal pDocumentosPessoal)
        {
            DocumentosPessoal tempDocumentosPessoal = Consulta.FirstOrDefault(p => p.ID.Equals(pDocumentosPessoal.ID));
            if (tempDocumentosPessoal == null)
            {
                throw new Exception("Não foi possível encontrar este Documento");
            }

            tempDocumentosPessoal.FimDE = DateTime.Now.ToString("dd/MM/yyyy");
            tempDocumentosPessoal.UsuarioExclusao = pDocumentosPessoal.UsuarioExclusao;
            tempDocumentosPessoal.DataExclusao = DateTime.Now;
            base.Alterar(tempDocumentosPessoal);

            
           
           
            pDocumentosPessoal.ApartirDe = DateTime.Now.ToString("dd/MM/yyyy");
            pDocumentosPessoal.FimDE = string.Empty;
            pDocumentosPessoal.UsuarioInclusao = tempDocumentosPessoal.UsuarioExclusao;
            pDocumentosPessoal.DataExclusao = DateTime.MaxValue;
            pDocumentosPessoal.UniqueKey = tempDocumentosPessoal.UniqueKey;
            pDocumentosPessoal.UsuarioExclusao = null;
            pDocumentosPessoal.AtualizadoPor = pDocumentosPessoal.UsuarioExclusao;
            pDocumentosPessoal.ID = Guid.Empty;

            base.Inserir(pDocumentosPessoal);
        }

    }
}
