using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EPeriodicidade
    {
        [Display(Name = "Semestral")]
        Semestral = 1,

        [Display(Name = "Anual")]
        Anual = 2,

        [Display(Name = "Bianual")]
        Bianual = 3,

        [Display(Name = "Se Necessário")]
        Se_Necessario = 4,

        
    }
}
