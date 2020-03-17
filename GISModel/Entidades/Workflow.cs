using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    public class Workflow: EntidadeBase
    {

        public Guid UKAlocacao { get; set; }

        public Guid UKREL_DocAloc { get; set; }

        public int Status { get; set; }

        [Display(Name ="Comentários")]
        public string Comentarios { get; set; }





    }
}
