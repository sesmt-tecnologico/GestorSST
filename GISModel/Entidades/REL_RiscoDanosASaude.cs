using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_RiscoDanosASaude")]
    public class REL_RiscoDanosASaude:EntidadeBase
    {
        [Display(Name ="Riscos")]
        public Guid UKRiscos { get; set; }

        [Display(Name ="Danos a Saúde")]
        public Guid UKDanosSaude { get; set; }


    }
}
