using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EControle
    {
        [Display(Name = "Neutralizado/Mitigado")]
        Neutralizado_Mitigado = 1,

        [Display(Name = "Reduzido")]
        Reduzido = 2,

        [Display(Name = "Não se Aplica")]
        Não_se_Aplica = 3,

        

        
    }
}

