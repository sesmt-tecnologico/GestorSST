using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum TipoEstabelecimento
    {
        [Display(Name = "Operacional")]
        Operacional = 1,

        [Display(Name = "Administrativo")]
        Administrativo = 2,

        [Display(Name = "Operacional e Administrativo")]
        Operacional_Administrativo = 3,

        [Display(Name = "Alojamento")]
        Alojamento = 4,

        

    }
}

