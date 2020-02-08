using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("tbRisco")]
    public class Risco : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o nome do risco")]
        public string Nome { get; set; }

        public List<PossiveisDanos> Danos { get; set; }

    }
}
