using System.ComponentModel.DataAnnotations;

namespace Getask.Repository.Models.ViewModel
{
    public class RedefinirSenhaViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "O campo Nova Senha é obrigatório.")]
        [Display(Name = "Nova Senha")]
        [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a nova senha")]
        [Compare("NovaSenha", ErrorMessage = "A nova senha e a senha de confirmação não são iguais.")]
        public string ConfirmaSenha { get; set; }
    }
}