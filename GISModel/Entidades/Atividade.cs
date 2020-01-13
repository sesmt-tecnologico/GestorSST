using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividade")]
    public class Atividade: EntidadeBase
    {
        
        [Display(Name ="Descrição da Atividade")]
        public string Descricao { get; set; }

        [Display(Name = "Nome da Imagem ")]
        public string NomeDaImagem { get; set; }

        [Display(Name = "Imagem")]
        public string Imagem { get; set; }     




        //public Guid FuncCargoID { get; set; }

        //public virtual FuncCargo FuncCargo { get; set; }

    }
}
