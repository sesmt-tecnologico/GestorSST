using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Conta
{
    public class NovaSenhaViewModel
    {
        public Guid UKUsuario { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        [Required(ErrorMessage = "Informe a senha atual")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Informe a nova senha")]
        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Informe a nova senha novamente")]
        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        public string ConfirmarNovaSenha { get; set; }

    }
}
