using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbFuncao")]
    public class Funcao: EntidadeBase
    {

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "Selecione um cargo")]
        public Guid UKCargo { get; set; }


        [Display(Name = "Nome da Função")]
        public string NomeDaFuncao { get; set; }


        public List<Atividade> Atividades { get; set; }

        
    }
}
