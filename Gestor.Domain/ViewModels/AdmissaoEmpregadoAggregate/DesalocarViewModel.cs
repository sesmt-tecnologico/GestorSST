using System;
using System.ComponentModel.DataAnnotations;

namespace Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate
{
    public class DesalocarViewModel
    {
        [Display(Name = "Data de Desalocação")]
        [Required]
        public DateTime? DataDesalocacao { get; set; }
    }
}