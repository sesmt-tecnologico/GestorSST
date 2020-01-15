using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_FuncaoAtividade")]
    public class REL_FuncaoAtividade : EntidadeBase
    {

        [Display(Name = "Função")]
        public Guid UKFuncao { get; set; }

        [Display(Name = "Atividade")]
        public Guid UKAtividade { get; set; }



        public virtual Funcao Funcao { get; set; }

        public virtual Atividade Atividade { get; set; }

    }
}
