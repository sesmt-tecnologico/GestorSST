using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum ETipoIniciativa
    {
        [Display(Name = "Empregador")]
        Empregador = 0,

        [Display(Name = "Ordem Judicial")]
        Ordem_Judicial = 1,

        [Display(Name = "Determinação de Órgão Fiscalizador")]
        Orgao_Fiscalizador = 2
    }
}
