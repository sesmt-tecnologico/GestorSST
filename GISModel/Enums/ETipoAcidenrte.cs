using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoAcidenrte
    {
        [Display(Name = "ACIDENTE COM AFASTAMENTO")]
        ACIDENTE_COM_AFASTAMENTO = 1,

        [Display(Name = "ACIDENTE SEM AFASTAMENTO")]
        ACIDENTE_SEM_AFASTAMENTO = 2,

        [Display(Name = "ACIDENTE FATAL")]
        ACIDENTE_FATAL = 3,
    }
}

