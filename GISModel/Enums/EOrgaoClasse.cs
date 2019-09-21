using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum EOrgaoClasse
    {
        [Display(Name = "Conselho Regional de Medicina")]
        CRM = 0,

        [Display(Name = "Conselho Regional de Odontologia")]
        CRO = 1,

        [Display(Name = "Reg. Minist. do Trabalho")]
        Minist_Trabalho = 2

        
    }
}
