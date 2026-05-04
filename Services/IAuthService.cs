public interface IAuthService
{
    /// <summary>Registra um novo usuário. Retorna erro se o nome já existir.</summary>
    Task<(bool Success, string? ErrorMessage, RegisterResponse? Data)> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>Valida usuário e senha no banco.</summary>
    Task<Usuario?> ValidateLoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
