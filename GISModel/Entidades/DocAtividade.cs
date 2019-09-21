using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbDocAtividade")]
    public class DocAtividade: EntidadeBase
    {

        public string IDUniqueKey { get; set; }

        public string IDDocumentosEmpregado { get; set; }

        public Atividade Atividade { get; set; }

        public virtual DocumentosPessoal DocumentosEmpregado { get; set; }

    }
}
