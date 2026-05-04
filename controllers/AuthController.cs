using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;
    private readonly IAuthService _authService;
    private readonly JwtSettings _jwt;

    public AuthController(
        IJwtTokenService tokenService,
        IAuthService authService,
        IOptions<JwtSettings> jwtOptions)
    {
        _tokenService = tokenService;
        _authService = authService;
        _jwt = jwtOptions.Value;
    }

    /// <summary>Cadastra um novo usuário no banco de dados (senha armazenada com hash).</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var (success, error, data) = await _authService.RegisterAsync(request, cancellationToken);
        if (!success)
            return Conflict(new { message = error });

        return CreatedAtAction(nameof(Login), null, data);
    }

    /// <summary>Autentica com usuário e senha cadastrados no banco e retorna um JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var usuario = await _authService.ValidateLoginAsync(request.Username, request.Password, cancellationToken);
        if (usuario == null)
            return Unauthorized(new { message = "Usuário ou senha inválidos." });

        var token = _tokenService.CreateToken(usuario.Username);
        return Ok(new LoginResponse
        {
            Token = token,
            TokenType = "Bearer",
            ExpiresInMinutes = _jwt.ExpirationMinutes,
        });
    }
}
