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

        [Display(Name ="WorkArea")]
        public Guid UKWorkArea { get; set; }

        [Display(Name ="Fonte Geradora")]
        public string FonteGeradora { get; set; }

        [Display(Name = "Descrição")]
        public string  Descricao { get; set; }

        public List<Perigo> Perigos { get; set; }



    }
}
