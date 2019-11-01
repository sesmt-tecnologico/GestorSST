using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("REL_WorkAreaRisco")]
    public class REL_WorkAreaRisco : EntidadeBase
    {

        [Display(Name = "WorkArea")]
        public Guid UKWorkArea { get; set; }

        [Display(Name = "Risco")]
        public Guid UKRisco { get; set; }

        public virtual WorkArea WorkArea { get; set; }

        public virtual Risco Risco { get; set; }

    }
}
