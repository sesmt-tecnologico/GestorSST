using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EClasseDoRisco
    {

        [Display(Name = "Acidente")]
        Acidente = 1,

        [Display(Name = "Doença Ocupacional")]
        Doenca_Ocupacional = 2,

        [Display(Name = "Acidente com Terceiros")]
        Acidente_Terceiros = 3,

        [Display(Name = "Acidente com População")]
        Acidente_Populacao = 4,

        [Display(Name = "Danos Materiais")]
        Dano_Matrial = 5,

        [Display(Name = "Calor")]
        Calor = 6,

    }
}
