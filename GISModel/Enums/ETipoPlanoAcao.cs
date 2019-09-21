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

        [Display(Name = "Outros")]
        Outros = 2

        //[Display(Name = "EM TREINAMENTO EXTERNO")]
        //EM_TREINAMENTO_EXTERNO = 3,

        //[Display(Name = "EM TREINAMENTO NA UNIVERCEMIG")]
        //EM_TREINAMENTO_NA_UNIVERCEMIG = 4,

        //[Display(Name = "A SERVIÇO DE OUTRO ÓRGÃO")]
        //A_SERVIÇO_DE_OUTRO_ÓRGÃO = 5,

        //[Display(Name = "TRAJETO")]
        //TRAJETO = 6,

        
    }
}

