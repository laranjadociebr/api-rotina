// Usuário da API: credenciais persistidas no MySQL; a senha é armazenada apenas como hash (nunca em texto plano).
public class Usuario
{
    public int Id { get; set; }

    public required string Username { get; set; }

    /// <summary>Hash gerado por <see cref="Microsoft.AspNetCore.Identity.PasswordHasher{TUser}"/>.</summary>
    public required string PasswordHash { get; set; }
}
