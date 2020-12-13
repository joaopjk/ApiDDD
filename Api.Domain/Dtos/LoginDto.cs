using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email é um campo obrigatório para Login")]
        [EmailAddress(ErrorMessage="Email enviado é inválido !")]
        [StringLength(100,ErrorMessage="Email deve ter no máximo 100 caracteres !")]
        public string Email { get; set; }
    }
}