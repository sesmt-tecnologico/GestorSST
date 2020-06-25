using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoMedicoes
    {

        [Display(Name = "Ruido")]
        Ruido = 1,

        [Display(Name = "Ruido Impacto")]
        ruido_Impacto = 2,

        [Display(Name = "Calor")]
        Calor = 3,

        [Display(Name = "Radiação Ionizantes")]
        Radiacao_Ionizantes = 4,

        [Display(Name = "Condições Hiperbáricas")]
        Condicoes_Hiperbaricas = 5,

        [Display(Name = "Radiação não Ionizantes")]
        Radiacao_nao_Ionizantes = 6,

        [Display(Name = "Vibrações")]
        Vibracoes = 7,

        [Display(Name = "Frio")]
        Frio = 8,

        [Display(Name = "Umidade")]
        Umidade = 9,

        [Display(Name = "Poeiras Minerais")]
        Poeiras_Minerais = 10,

        [Display(Name = "Agentes Quimicos")]
        Agentes_Quimicos = 11,

        [Display(Name = "Benzeno")]
        Benzeno = 12,

        [Display(Name = "Agentes Biológicos")]
        Biologicos = 13,
    }
}
