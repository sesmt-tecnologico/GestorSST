using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_FontePerigo")]
    public class REL_FontePerigo: EntidadeBase
    {
        

        [Display(Name = "Fonte Geradora de Risco")]
        public Guid UKFonteGeradora { get; set; }

        [Display(Name = "Perigo")]
        public Guid UKPerigo { get; set; }        

        public virtual FonteGeradoraDeRisco FonteGeradoraDeRisco { get; set; }

        public virtual Perigo Perigo { get; set; }


    }
}
