using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{
    [Table("tbPergunta")]
    public class Pergunta : EntidadeBase
    {

        public Guid UKQuestionario { get; set; }

        public string Descricao { get; set; }

        [Display(Name = "Tipo de Resposta")]
        public ETipoResposta TipoResposta { get; set; }

        [Display(Name = "Listagem de Respostas")]
        public Guid UKTipoResposta { get; set; }

        [Display(Name = "Pergunta vinculada")]
        public Guid UKPerguntaVinculada { get; set; }

        [NotMapped]
        public TipoResposta _TipoResposta { get; set; }

    }
}
