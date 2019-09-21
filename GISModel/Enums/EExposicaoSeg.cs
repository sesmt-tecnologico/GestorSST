using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EExposicaoSeg
    {

        // Semestral
        [Display(Name = "Muito Baixa")]
        Muito_Baixa = 1,

        // Mensal
        [Display(Name = "Baixa")]
        Baixa = 2,

        //Semanal
        [Display(Name = "Média")]
        Media = 3,

        //Diária
        [Display(Name = "Alta")]
        Alta = 4,

        //Diária, com necessidade frequente de horas extras
        [Display(Name = "Muito Alta")]
        Muito_Alta = 5

        

    }
}

