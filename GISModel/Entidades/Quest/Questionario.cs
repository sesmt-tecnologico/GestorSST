using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{
    [Table("tbQuestionario")]
    public class Questionario : EntidadeBase
    {

        public Guid UKEmpresa { get; set; }

        public string Nome { get; set; }

        public ETipoQuestionario? TipoQuestionario { get; set; }


        [NotMapped]
        public List<Pergunta> Perguntas { get; set; }

    }
}
