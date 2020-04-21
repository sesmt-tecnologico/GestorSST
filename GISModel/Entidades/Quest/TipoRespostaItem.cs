using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("tbTipoRespostaItem")]
    public class TipoRespostaItem : EntidadeBase
    {

        public Guid UKTipoResposta { get; set; }

        public string Nome { get; set; }

        public int Ordem { get; set; }

        [NotMapped]
        public List<Pergunta> Perguntas { get; set; }

    }
}
