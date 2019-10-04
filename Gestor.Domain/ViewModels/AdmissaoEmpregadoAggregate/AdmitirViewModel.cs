using System;
using System.ComponentModel.DataAnnotations;

namespace Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate
{
    public class AdmitirViewModel
    {
        [Display(Name = "Empresa")]
        [Required]
        public Guid? EmpresaId { get; set; }

        [Display(Name = "Tomadora")]
        public Guid? TomadoraId { get; set; }

        [Display(Name = "Data de Admissão")]
        [Required]
        public DateTime? DataAdmissao { get; set; }
    }
}