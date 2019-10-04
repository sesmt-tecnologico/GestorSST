using System;
using System.ComponentModel.DataAnnotations;

namespace Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate
{
    public class AtribuirViewModel
    {
        [Display(Name = "Atividade")]
        [Required]
        public Guid? AtividadeId { get; set; }
    }
}