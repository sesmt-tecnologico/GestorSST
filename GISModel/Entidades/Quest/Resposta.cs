using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{

    [Table("tbResposta")]
    public class Resposta : EntidadeBase
    {

        public Guid UKQuestionario { get; set; }

        public Guid UKEmpresa { get; set; }

        public Guid UKEmpregado { get; set; }

    }
}
