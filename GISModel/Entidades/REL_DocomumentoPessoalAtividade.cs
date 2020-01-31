using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_DocumentoPessoalAtividade")]
    public class REL_DocomumentoPessoalAtividade: EntidadeBase
    {

        [Display(Name ="Atividade")]
        public Guid UKAtividade { get; set; }

        [Display(Name = "Documento")]
        public Guid UKDocumentoPessoal { get; set; }
        
        

    }
}
