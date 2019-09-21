using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EConsequencia
    {
        [Display(Name = "MORTE")]
        MORTE = 1,

        [Display(Name = "INCAPACIDADE PERMANENTE TOTAL")]
        INCAPACIDADE_PERMANENTE_TOTAL = 2,

        [Display(Name = "INCAPACIDADE PERMANENTE PARCIAL")]
        INCAPACIDADE_PERMANENTE_PARCIAL = 3,

        [Display(Name = "INCAPACIDADE TEMPORARIA TOTAL")]
        INCAPACIDADE_TEMPORARIA_TOTAL = 4,

        [Display(Name = "SEM INCAPACITACAO")]
        SEM_INCAPACITACAO = 5

        
    }
}
