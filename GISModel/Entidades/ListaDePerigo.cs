using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbListaDePerigo")]
    public class ListaDePerigo : EntidadeBase
    {

        [Display(Name = "Descricao do Perigo")]
        public string DescricaoPerigo { get; set; }

    }
}
