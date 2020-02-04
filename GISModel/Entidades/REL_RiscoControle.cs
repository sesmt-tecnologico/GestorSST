using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_RiscoControle")]
   public class REL_RiscoControle: EntidadeBase
    {

        [Display(Name ="Risco")]
        public Guid UKRisco { get; set; }

        [Display(Name ="Controle")]
        public Guid  UKReconhecimentoRisco { get; set; }

        public virtual Risco Risco { get; set; }

        public virtual ControleDeRiscos ControleDeRisco { get; set; }


    }
}
