using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEstabelecimento")]
    public class Estabelecimento : EntidadeBase
    {

        [Display(Name ="Tipo de Estabelecimento")]
        public TipoEstabelecimento TipoDeEstabelecimento { get; set; }

        [Display(Name = "Código")]
        public string  Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }


        public List<ReconhecimentoDoRisco> ReconhecimentoDoRisco { get; set; }


    }
}
