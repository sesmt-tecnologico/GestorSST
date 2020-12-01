using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ECategoriaFrases
    {

        [Display(Name = "SEGURANÇA")]
        SEGURANCA = 1,

        [Display(Name = "SAÚDE")]
        SAUDE = 2,

        [Display(Name = "MEIO AMBIENTE")]
        MEIO_AMBIENTE = 3,

        [Display(Name = "CIPA")]
        CIPA = 4,

        [Display(Name = "CIPAT")]
        CIPAT = 5,

        [Display(Name = "TRÂNSITO")]
        TRANSITO = 6,

    }
}
