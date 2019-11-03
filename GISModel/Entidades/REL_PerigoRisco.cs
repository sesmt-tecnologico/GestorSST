using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.Entidades
{
    public class REL_PerigoRisco : EntidadeBase
    {

        [Display(Name = "Perigo")]
        public Guid UKPerigo { get; set; }

        [Display(Name = "Risco")]
        public Guid UKRisco { get; set; }

        public virtual Perigo Perigo { get; set; }

        public virtual Risco Risco { get; set; }

    }
}
