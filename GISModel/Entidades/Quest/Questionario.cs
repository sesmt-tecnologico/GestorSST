using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{
    [Table("tbQuestionario")]
    public class Questionario : EntidadeBase
    {

        public Guid UKEmpresa { get; set; }

        public string Nome { get; set; }

        public ETipoQuestionario? TipoQuestionario { get; set; }

        public int Tempo { get; set; }


        [Display(Name = "Período")]
        public EPeriodo Periodo { get; set; }


        public Situacao Status { get; set; }


        [NotMapped]
        public List<Pergunta> Perguntas { get; set; }

    }
}
