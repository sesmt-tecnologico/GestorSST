using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbNivelHierarquico")]
    public class NivelHierarquico : EntidadeBase
    {

        [Display(Name = "Nível Hierárquico")]
        [Required(ErrorMessage = "Informe o nome do nível")]
        public string Nome { get; set; }

    }
}
