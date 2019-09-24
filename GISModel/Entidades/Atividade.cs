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


        [Display(Name = "Função")]
        public Guid idFuncao { get; set; }

        [Display(Name = "Diretoria")]
        public Guid idDiretoria { get; set; }
       

        public virtual Funcao Funcao { get; set; }

        public virtual Diretoria Diretoria { get; set; }

    }
}
