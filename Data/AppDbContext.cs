using Microsoft.EntityFrameworkCore;
using ApiTarefas.Models;

namespace ApiTarefas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }

        public DbSet<Rotina> Rotinas { get; set; }
    }
}