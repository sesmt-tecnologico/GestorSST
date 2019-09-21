using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbDocsPorAtividade")]
    public class DocsPorAtividade: EntidadeBase
    {

        [Display(Name ="Atividade")]
        public string idAtividade { get; set; }

        [Display(Name = "Documento")]
        public string idDocumentosEmpregado { get; set; }
        
        public virtual DocumentosPessoal DocumentosEmpregado { get; set; }

        public virtual Atividade Atividade { get; set; }

    }
}
