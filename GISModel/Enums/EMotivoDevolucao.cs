using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EMotivoDevolucao
    {

        [Display(Name = "Danificado em Trabalho")]
        Danificado_em_Trabalho = 1,

        [Display(Name = "Troca por tempo uso")]
        Tempo_de_Uso = 2,

        [Display(Name = "Perda")]
        Perda = 3,

        [Display(Name = "Não fará mais uso")]
        Nao_fara_uso = 4,

        [Display(Name = "Demissão")]
        Demissao = 5,

    }
}
