using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbTipoDeControle")]
    public class TipoDeControle: EntidadeBase
    {
       
        [Display(Name ="Tipo de Controle")]
        public string Descricao { get; set; }

    }
}
