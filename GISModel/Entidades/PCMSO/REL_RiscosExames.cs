using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.PCMSO
{
    [Table("REL_RiscosExames")]
    public class REL_RiscosExames:EntidadeBase
    {
        [Display(Name ="Tipo de Exame")]
        public ETipoExame TipoExame { get; set; }

        [Display(Name ="Perigo")]
        public Guid ukPerigo { get; set; }

        [Display(Name ="Exame")]
        public Guid ukExame { get; set; }

        [Display(Name = "Obrigatoriedade")]
        public EObrigatoriedade Obrigariedade { get; set; }

      


    }
}
