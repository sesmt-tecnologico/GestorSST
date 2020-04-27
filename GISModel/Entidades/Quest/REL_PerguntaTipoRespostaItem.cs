using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("REL_PerguntaTipoRespostaItem")]
    public class REL_PerguntaTipoRespostaItem : EntidadeBase
    {

        public Guid UKPerguntaVinculada { get; set; }

        public Guid UKTipoRespostaItem { get; set; }


        public Guid UKNovaPergunta { get; set; }

    }
}
