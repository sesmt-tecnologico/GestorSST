using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    public class ClassificacaoMedida: EntidadeBase
    {
        [Display(Name ="Classificação da Medida")]
        public string Nome { get; set; }


    }
}
