using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [MinLength(3, ErrorMessage = "O nome de usuário deve ter pelo menos 3 caracteres.")]
    [MaxLength(50, ErrorMessage = "O nome de usuário pode ter no máximo 50 caracteres.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    [MaxLength(100, ErrorMessage = "A senha pode ter no máximo 100 caracteres.")]
    public string Password { get; set; } = string.Empty;
}
