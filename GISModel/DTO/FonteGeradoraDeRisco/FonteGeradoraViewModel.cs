using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.FonteGeradoraDeRisco
{
    public class FonteGeradoraViewModel
    {

        public Guid UniqueKey { get; set; }


        [Display(Name = "Estabelecimento")]
        public string UKEstabelecimento { get; set; }


        [Display(Name ="Nome da WorkArea")]
        public string Nome { get; set; }


        [Display(Name ="Descrição")]
        public string Descricao { get; set; }


        [Display(Name ="Nome")]
        public string NomeCompleto { get; set; }

    }
}
