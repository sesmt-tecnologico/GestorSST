using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_DepartamentoContrato")]
    public class REL_DepartamentoContrato : EntidadeBase
    {

        [Display(Name ="Contrato")]
        public Guid IDContrato { get; set; }


        [Display(Name = "Departamento")]
        public Guid IDDepartamento { get; set; }


        public virtual Departamento Departamento { get; set; }

        public virtual Contrato Contrato { get; set; }

    }
}
