using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbFonteGeradoraDeRisco")]
    public class FonteGeradoraDeRisco: EntidadeBase
    {
       

        [Display(Name ="Fonte Geradora")]
        public string FonteGeradora { get; set; }



    }
}
