using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AuthService(AppDbContext db, IPasswordHasher<Usuario> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<(bool Success, string? ErrorMessage, RegisterResponse? Data)> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var username = request.Username.Trim();
        if (string.IsNullOrEmpty(username))
            return (false, "Nome de usuário inválido.", null);

        if (await _db.Usuarios.AnyAsync(u => u.Username == username, cancellationToken))
            return (false, "Este nome de usuário já está em uso.", null);

        var usuario = new Usuario
        {
            Username = username,
            PasswordHash = string.Empty,
        };
        usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.Password);

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync(cancellationToken);

        return (true, null, new RegisterResponse
        {
            Id = usuario.Id,
            Username = usuario.Username,
            Message = "Registro concluído. Você já pode fazer login.",
        });
    }

    public async Task<Usuario?> ValidateLoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var u = await _db.Usuarios.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (u == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(u, u.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        return u;
    }
}
