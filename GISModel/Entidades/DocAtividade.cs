using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbDocAtividade")]
    public class DocAtividade: EntidadeBase
    {

        public Guid IDUniqueKey { get; set; }

        public Guid IDDocumentosEmpregado { get; set; }

        public Atividade Atividade { get; set; }

        public virtual DocumentosPessoal DocumentosEmpregado { get; set; }

    }
}
