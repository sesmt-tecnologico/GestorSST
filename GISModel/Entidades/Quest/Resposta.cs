using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("tbResposta")]
    public class Resposta : EntidadeBase
    {

        [Display(Name = "Questionário")]
        public Guid UKQuestionario { get; set; }


        [Display(Name = "Empresa")]
        public Guid UKEmpresa { get; set; }


        [Display(Name = "Empregado")]
        public Guid UKEmpregado { get; set; }



        [NotMapped]
        public Questionario Questionario { get; set; }

        [NotMapped]
        public Empresa Empresa { get; set; }

        [NotMapped]
        public Empregado Empregado { get; set; }

    }
}
