using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EClassificacaoDaMedia
    {

       
            [Display(Name = "Administrativa")]
            Administrativa = 1,

            [Display(Name = "Engenharia")]
            Engenharia = 2,

            [Display(Name = "EPC")]
            EPC = 3,

            [Display(Name = "EPI")]
            EPI = 4, 


    }
}
