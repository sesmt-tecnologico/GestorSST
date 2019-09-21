using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoLocalAcid
    {
        [Display(Name = "Estab.empregador no Brasil")]
        Estab_empregador_no_Brasil=1,

        [Display(Name = "Estab.empregador no exterior")]
        Estab_empregador_no_exterior = 2,

        [Display(Name = "Estab. terc. onde presta serviço")]
        Estab_terc_onde_presta_serv = 3,

        [Display(Name = "Via pública")]
        Via_pública = 4,

        [Display(Name = "Área rural")]
        Área_rural = 5,

        [Display(Name = "Embarcação")]
        Embarcação = 6,

        [Display(Name = "Outros")]
        Outros = 7

    }
}

