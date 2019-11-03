using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("REL_WorkAreaPerigo")]
    public class REL_WorkAreaPerigo : EntidadeBase
    {

        [Display(Name = "WorkArea")]
        public Guid UKWorkArea { get; set; }

        [Display(Name = "Risco")]
        public Guid UKPerigo { get; set; }

        public virtual WorkArea WorkArea { get; set; }

        public virtual Perigo Perigo { get; set; }

    }
}
