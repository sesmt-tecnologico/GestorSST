using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoExame
    {

        [Display(Name = "Admissional")]
        Admissional = 1,

        [Display(Name = "Periódico")]
        Periodico= 2,

        [Display(Name = "Retorno ao Trabalho")]
        Retornop = 3,
    }
}
