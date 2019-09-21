using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Conta
{
    public class NovaSenhaViewModel
    {

        public string IDUsuario { get; set; }

        public string Email { get; set; }

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
