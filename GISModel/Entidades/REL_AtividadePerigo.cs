using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("REL_AtividadePerigo")]
    public class REL_AtividadePerigo: EntidadeBase
    {        

        [Display(Name = "Atividade")]
        public Guid UKAtividade { get; set; }

        [Display(Name = "Perigo")]
        public Guid UKPerigo { get; set; }

       

    }
}
