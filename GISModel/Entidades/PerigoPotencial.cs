using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbPerigoPotencial")]
    public class PerigoPotencial : EntidadeBase
    {

        [Display(Name = "Evento Perigoso Potencial")]
        public string DescricaoEvento { get; set; }

    }
}
