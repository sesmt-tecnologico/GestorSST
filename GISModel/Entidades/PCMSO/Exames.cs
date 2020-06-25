using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.PCMSO
{
    [Table("tbExames")]
    public class Exames: EntidadeBase
    {

        [Display(Name ="Exame")]
        public string Nome { get; set; }


    }
}
