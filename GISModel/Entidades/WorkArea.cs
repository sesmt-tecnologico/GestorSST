using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbWorkArea")]
    public class WorkArea : EntidadeBase
    {

        [Display(Name = "Estabelecimento")]
        [Required(ErrorMessage = "Selecione um estabelecimento")]
        public Guid UKEstabelecimento { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }



        [Display(Name ="Nome da Workarea")]
        [Required(ErrorMessage = "Informe o nome da workarea")]
        public string  Nome { get; set; }



        [Display(Name ="Descrição")]
        public string Descricao { get; set; }

        public List<Perigo> Perigos { get; set; }

    }
}
