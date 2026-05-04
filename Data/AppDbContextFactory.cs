using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// Permite `dotnet ef migrations add` sem MySQL em execução (versão do servidor fixa para o modelo).
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = "Server=localhost;Port=3306;Database=MinhaPrimeiraApi;User=root;Password=root;";
        var serverVersion = ServerVersion.Parse("8.0.36-mysql");
        optionsBuilder.UseMySql(connectionString, serverVersion);
        return new AppDbContext(optionsBuilder.Options);
    }
}
