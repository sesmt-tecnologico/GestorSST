using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoAcidente
    {
        [Display(Name = "001 - Acidente com Contratado")]
        Acidente_com_Contratado = 1,

        [Display(Name = "002 - Acidente com Empregado")]
        Acidente_com_Empregado = 2,

        [Display(Name = "008 - Acidente em Novos Negócios")]
        Acidente_em_Novos_Negocios = 3,

        [Display(Name = "009 - Acidente em Obra PART")]
        Acidente_em_Obra_PART = 4,

        [Display(Name = "012 - Doença Ocupacional")]
        Doença_Ocupacional = 5

    }
}

