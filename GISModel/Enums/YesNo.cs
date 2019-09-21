using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum YesNo
    {
        [Display(Name = "Não")]
        Nao = 0, 

        [Display(Name = "Sim")]
        Sim = 1,

    }
}
