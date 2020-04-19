using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("tbTipoResposta")]
    public class TipoResposta : EntidadeBase
    {

        public string Nome { get; set; }

        [NotMapped]
        public List<TipoRespostaItem> TiposResposta { get; set; }

    }
}
