using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbCargo")]
    public class Cargo: EntidadeBase
    {
        
        [Display(Name = "Nome do Cargo")]
        public string NomeDoCargo { get; set; }

        public List<Funcao> Funcoes { get; set; }

    }
}
