using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbWorkArea")]
    public class WorkArea: EntidadeBase
    {

        [Display(Name ="Nome da Workarea")]
        public string  Nome { get; set; }

        [Display(Name ="Descrição")]
        public string Descricao { get; set; }     
       


    }
}
