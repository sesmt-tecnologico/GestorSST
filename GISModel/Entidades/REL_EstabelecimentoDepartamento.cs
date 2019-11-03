using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_EstabelecimentoDepartamento")]
    public class REL_EstabelecimentoDepartamento : EntidadeBase
    {

        [Display(Name = "Estabelcimento")]
        public Guid UKEstabelecimento { get; set; }

        [Display(Name = "Departamento")]
        public Guid UKDepartamento { get; set; }


        public virtual Departamento Departamento { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

    }
}
