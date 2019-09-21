using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoEntrada
    {
        [Display(Name = "Acidente de Trabalho")]
        Acidente_de_Trabalho = 1,

        [Display(Name = "Acidente de Trajeto")]
        Acidente_de_Trajeto = 2,

        [Display(Name = "Doença Ocupacional")]
        Doenca_Ocupacional = 3
    }
}
