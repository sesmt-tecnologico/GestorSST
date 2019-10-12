using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_DepartamentoContrato")]
    public class REL_DepartamentoContrato : EntidadeBase
    {
        [Display(Name ="Contrato")]
        public Guid IDContrato { get; set; }

        [Display(Name = "Departamento")]
        public Guid IDDepartamento { get; set; }

        [Display(Name = "Fornecedor")]
        public Guid IDFornecedor { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

    }
}
