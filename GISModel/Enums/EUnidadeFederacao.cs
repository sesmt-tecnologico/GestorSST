using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EUnidadeFederacao
    {

        [Display(Name = "Minas Gerais")]
        MG = 1,

        [Display(Name = "são Paulo")]
        SP = 2,

        [Display(Name = "Rio de Janeiro")]
        RJ = 3,
    }
}
