using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbPossiveisDanos")]
    public class PossiveisDanos: EntidadeBase
    {

        [Display(Name ="Possível Dano")]
        public string DescricaoDanos { get; set; }

       
        public virtual Risco Risco { get; set; }

    }
}
