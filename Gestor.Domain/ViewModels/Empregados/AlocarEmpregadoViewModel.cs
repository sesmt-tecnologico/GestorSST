using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestor.Domain.ViewModels.Empregados
{
    public class AlocarEmpregadoViewModel
    {
        [Display(Name = "Estabelecimento")]
        [Required]
        public Guid? EstabelecimentoId { get; set; }

        [Display(Name = "Cargo")]
        [Required]
        public Guid? CargoId { get; set; }

        [Display(Name = "Função")]
        [Required]
        public Guid? FuncaoId { get; set; }

        [Display(Name = "Atividades")]
        public IEnumerable<Guid> AtividadesIds { get; set; }

        [Display(Name = "Departamento")]
        public Guid? DepartamentoId { get; set; }

        [Display(Name = "Equipe")]
        public Guid? EquipeId { get; set; }

        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }
    }
}