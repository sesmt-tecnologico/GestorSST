using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbDocumentosPessoal")]
    public class DocumentosPessoal: EntidadeBase
    {

        [Display(Name ="Nome do Documento")]
        public string NomeDocumento { get; set; }

        [Display(Name ="Descrição")]
        public string DescriçãoDocumento { get; set; }

    }
}
