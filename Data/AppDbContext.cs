using Microsoft.EntityFrameworkCore;

// Contexto do Entity Framework Core para a aplicação.
// Representa uma "sessão" com o banco de dados e expõe os DbSets (tabelas).
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Tabela Produtos no banco. Cada Produto vira uma linha na tabela.
    public DbSet<Produto> Produtos => Set<Produto>();

    // Tabela Categorias no banco.
    public DbSet<Categoria> Categorias => Set<Categoria>();

    // Usuários cadastrados (login/registro).
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<RotinaTarefas> RotinasTarefas => Set<RotinaTarefas>();

    // Configuração do modelo: tamanhos máximos e precisão para o MySQL.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Produto: garantir que Nome tenha tamanho máximo (evita problemas com varchar no MySQL)
        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Nome).HasMaxLength(200);
            e.Property(p => p.Preco).HasPrecision(18, 2);
        });

        // Categoria: mesmo raciocínio
        modelBuilder.Entity<Categoria>(e =>
        {
            e.Property(c => c.Nome).HasMaxLength(200);
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.Property(u => u.Username).HasMaxLength(50);
            e.Property(u => u.PasswordHash).HasMaxLength(500);
            e.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<RotinaTarefas>(e =>
        {
            e.Property(r => r.NomeTarefa).HasMaxLength(200);
            e.Property(r => r.TipoTarefa).HasMaxLength(100);
            e.Property(r => r.DiasSemana).HasMaxLength(100);
            e.Property(r => r.Duracao).HasMaxLength(50);
            e.Property(r => r.Localizacao).HasMaxLength(200);
            e.Property(r => r.Alarme).HasMaxLength(100);
            e.Property(r => r.WidgetAtivado).HasDefaultValue(false);
        });
    }
}
