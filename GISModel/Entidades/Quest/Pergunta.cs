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


        [Display(Name = "Descrição")]
        public string Descricao { get; set; }


        [Display(Name = "Tipo de Resposta")]
        public ETipoResposta TipoResposta { get; set; }

        [Display(Name = "Listagem de Respostas")]
        public Guid? UKTipoResposta { get; set; }

        public int Ordem { get; set; }

        [NotMapped]
        public TipoResposta _TipoResposta { get; set; }

    }
}
