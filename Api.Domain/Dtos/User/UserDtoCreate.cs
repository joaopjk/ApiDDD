using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.User
{
    public class UserDtoCreate
    {
        [Required]
        public Guid Id {get; set;}
        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        [StringLength(60,ErrorMessage = "Tamanho do campo máximo de {1} caracteres ")]
        public string Nome {get; set;}
        [Required(ErrorMessage = "Email é um campo obrigatório ")]
        [EmailAddress(ErrorMessage="Email enviado é inválido !")]
        [StringLength(100,ErrorMessage="Email deve ter no máximo 100 caracteres !")]
        public string Email {get; set;}
    }
}