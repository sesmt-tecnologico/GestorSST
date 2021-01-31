using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("tbResposta")]
    public class Resposta : EntidadeBase
    {

        [Display(Name = "Questionário")]
        public Guid? UKQuestionario { get; set; }


        [Display(Name = "Empresa")]
        public Guid? UKEmpresa { get; set; }


        [Display(Name = "Empregado")]
        public Guid? UKEmpregado { get; set; }

        [Display(Name = "Objeto")]
        public Guid? UKObjeto { get; set; }

        [Display(Name = "Registro")]
        public string Registro { get; set; }

        [Display(Name = "latitude")]
        public string latitude { get; set; }

        [Display(Name = "longitude")]
        public string longitude { get; set; }


        [Display(Name = "Status")]
        public string Status { get; set; }





        [NotMapped]
        public Questionario Questionario { get; set; }

        [NotMapped]
        public Empresa Empresa { get; set; }

        [NotMapped]
        public Empregado Empregado { get; set; }

    }
}
