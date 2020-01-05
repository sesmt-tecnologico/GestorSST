using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbFuncao")]
    public class Funcao: EntidadeBase
    {

        [Display(Name ="Nome da Função")]
        public string NomeDaFuncao { get; set; }

        [Display(Name ="Uk_Cargo")]
        public Guid Uk_Cargo { get; set; }

    }
}
