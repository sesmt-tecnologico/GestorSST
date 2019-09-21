using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoCAT
    {
        [Display(Name = "Inicial")]
        Inicial = 0,

        [Display(Name = "Reabertura")]
        Reabertura = 1,

        [Display(Name = "Comunicação de Óbito")]
        Comunicação_Obito = 2

       
    }
}
