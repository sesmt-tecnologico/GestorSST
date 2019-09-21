using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EExposicaoInsalubre
    {
        [Display(Name = "Contínuo")]
        Continuo = 1,

        [Display(Name = "Eventual")]
        Eventual = 2,

        [Display(Name = "Intermitente")]
        Intermitente = 3

        

        
    }
}

