using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EExposicaoCalor
    {
        [Display(Name = "Leve")]
        Leve = 1,

        [Display(Name = "Moderado")]
        Moderado = 2,

        [Display(Name = "Pesado")]
        Pesado = 3

        

        
    }
}

