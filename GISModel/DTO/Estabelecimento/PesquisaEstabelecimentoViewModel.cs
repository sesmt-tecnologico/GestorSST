using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Estabelecimento
{
    public class PesquisaEstabelecimentoViewModel
    {
        
        public Guid  UniqueKey { get; set; }
        
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Display(Name = "Estabelecimento")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Tipo")]
        public TipoEstabelecimento? TipoDeEstabelecimento { get; set; }

        [Display(Name = "Departamento")]
        public Guid UKDepartamento { get; set; }

        public string Departamento { get; set; }
        
    }
}
