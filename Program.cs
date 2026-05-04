using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ========== CONFIGURAÇÃO DE SERVIÇOS (DI) ==========
// Add services to the container.
builder.Services.AddControllers(); // Registra os controllers da API (ex.: ProdutosController, CategoriasController)
builder.Services.AddOpenApi();      // Habilita documentação OpenAPI (Swagger) em ambiente de desenvolvimento
builder.Services.AddScoped<ICategoriaService, CategoriaService>(); // Scoped: uma instância por requisição (compatível com DbContext)

// JWT: opções e serviço de emissão de token
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
var jwtSecret = jwtSection["SecretKey"] ?? throw new InvalidOperationException("Configure Jwt:SecretKey em appsettings ou variáveis de ambiente.");
if (jwtSecret.Length < 32)
    throw new InvalidOperationException("Jwt:SecretKey deve ter pelo menos 32 caracteres (HMAC-SHA256).");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });
builder.Services.AddAuthorization();

// Conexão com o banco: lida da configuração (appsettings.json) ou da variável de ambiente no Docker
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Port=3306;Database=MinhaPrimeiraApi;User=root;Password=root;";

// Registro do Entity Framework Core com provedor MySQL (Pomelo)
// AddDbContext usa Scoped por padrão: uma instância por requisição HTTP
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(3));
});

var app = builder.Build();

// ========== APLICAR MIGRAÇÕES E SEED ==========
// Database.Migrate() aplica migrações pendentes (tabelas Produtos, Categorias, Usuarios, etc.).
// Se a base foi criada antes só com EnsureCreated (sem histórico de migrações), pode ser necessário
// recriar o banco (ex.: docker compose down -v e subir de novo) para evitar conflito de esquema.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
        // Seed: insere dados iniciais apenas se as tabelas estiverem vazias
        if (!db.Produtos.Any())
        {
            db.Produtos.AddRange(
                new Produto { Nome = "Notebook", Preco = 3500 },
                new Produto { Nome = "Mouse", Preco = 80 });
            db.SaveChanges();
        }
        if (!db.Categorias.Any())
        {
            db.Categorias.AddRange(
                new Categoria { Nome = "Eletrônicos" },
                new Categoria { Nome = "Móveis" });
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Erro ao migrar/seed do banco. Verifique se o MySQL está acessível (ex.: Docker).");
    }
}

// ========== PIPELINE HTTP ==========
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Expõe o documento OpenAPI (ex.: em /openapi/v1.json)
}

// app.UseHttpsRedirection(); // Descomente para forçar redirecionamento HTTPS em produção

app.UseAuthentication(); // Valida JWT no cabeçalho Authorization: Bearer <token>
app.UseAuthorization();

// Mapeia os endpoints definidos nos controllers (rotas como /api/Produtos e /api/Categorias)
app.MapControllers();

// ========== ENDPOINT MÍNIMO (Minimal API) - Exemplo didático ==========
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
