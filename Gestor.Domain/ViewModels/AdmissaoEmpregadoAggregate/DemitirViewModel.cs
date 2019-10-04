using System;
using System.ComponentModel.DataAnnotations;

namespace Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate
{
    public class DemitirViewModel
    {
        [Display(Name = "Data de Demissão")]
        [Required]
        public DateTime? DataDemissao { get; set; }
    }
}