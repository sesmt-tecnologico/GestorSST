using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbCargo")]
    public class Cargo: EntidadeBase
    {

        [Display(Name ="Nome do Cargo")]
        public string NomeDoCargo { get; set; }

        [Display(Name = "Diretoria")]
        [Required(ErrorMessage = "Selecione uma Diretoria")]
        public Guid IDDiretoria { get; set; }
        
        public virtual  Diretoria Diretoria { get; set; }

    }
}
