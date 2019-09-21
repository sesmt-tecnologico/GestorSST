using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEventoPerigoso")]
    public class EventoPerigoso: EntidadeBase
    {

        [Display(Name ="Evento Perigoso Potencial")]
        public string Descricao { get; set; }

    }
}
