using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoRegistrador
    {
        [Display(Name = "Empregador")]
        Empregador = 0,

        [Display(Name = "Cooperativa")]
        Cooperativa = 1,

        [Display(Name = "Sindicato não Portuário")]
        Sindicato_não_Portuário = 2,

        [Display(Name = "Órgão Gestor de mão de Obra")]
        Org_Gest_MaoObra = 3,

        [Display(Name = "Empregado")]
        Empregado = 4,

        [Display(Name = "Dependente do Empregado")]
        Dependente_Empregado = 5,

        [Display(Name = "Sindicato Competente")]
        Sindicato_Competente = 6,

        [Display(Name = "Médico Assistente")]
        Médico_Assistente = 7,

        [Display(Name = "Autoridade Pública")]
        Autoridade_Publica = 7,

    }
}
