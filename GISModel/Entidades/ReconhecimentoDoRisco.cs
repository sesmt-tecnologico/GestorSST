using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbReconhecimentoDoRisco")]
    public class ReconhecimentoDoRisco: EntidadeBase
    {
               

        [Display(Name = "Workarea")]
        public Guid UKWorkarea { get; set; }

        [Display(Name = "Perigo")]
        public Guid Perigo { get; set; }

        [Display(Name ="Risco")]
        public Guid UKRisco { get; set; }

        [Display(Name = "Classifique o Risco")]
        public string EClasseDoRisco { get; set; }

        [Display(Name = "Fonte Geradora")]
        public string FonteGeradora { get; set; }

        [Display(Name = "Tragetória")]
        public string Tragetoria { get; set; }


    }
}
