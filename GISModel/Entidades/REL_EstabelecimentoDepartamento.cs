using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_EstabelecimentoDepartamento")]
    public class REL_EstabelecimentoDepartamento: EntidadeBase
    {

        [Display(Name = "idEstabelcimento")]
        public Guid IDEstabelecimento { get; set; }

        [Display(Name = "idDepartamento")]
        public Guid IDDepartamento { get; set; }


        public virtual Departamento Departamento { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

    }
}
