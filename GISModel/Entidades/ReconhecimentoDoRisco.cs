using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbReconhecimentoDoRisco")]
    public class ReconhecimentoDoRisco: EntidadeBase
    {
               

        [Display(Name = "Workarea")]
        public Guid UKWorkarea { get; set; }

        [Display(Name = "Fonte Geradora")]
        public Guid UKFonteGeradora { get; set; }

        [Display(Name = "Perigo")]
        public Guid UKPerigo { get; set; }

        [Display(Name ="Risco")]
        public Guid UKRisco { get; set; }








        [Display(Name = "Tragetória")]
        public ETrajetoria Tragetoria { get; set; }

        [Display(Name = "Classifique o Risco")]
        public EClasseDoRisco EClasseDoRisco { get; set; }

    }
}
