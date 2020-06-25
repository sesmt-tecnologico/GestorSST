using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EObrigatoriedade
    {
        [Display(Name = "Obrigatório")]
        Obrigatorio = 1,

        [Display(Name = "Se Necessário")]
        Se_Necessário = 2,

        [Display(Name = "Maior de 35 anos")]
        Maior_35_Anos = 3,

        [Display(Name = "Homem Maior de 40 anos")]
        Homem_Maior_40_Anos = 4,

        [Display(Name = "Homem Maior de 45 anos")]
        Homem_Maior_45_Anos = 5,

        [Display(Name = "Mulher")]
        Mulher = 6,
    }
}
