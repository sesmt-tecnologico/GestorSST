using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoPlanoAcao
    {
        [Display(Name = "Controle de Risco de Ambiente")]
        Controle_Risco_Ambiente = 1,

        [Display(Name = "Atividade Interrompida")]
        Atividade_Interrompida = 2,

        [Display(Name = "Outros")]
        Outros = 3

        
        
    }
}

