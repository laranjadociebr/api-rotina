using System.ComponentModel.DataAnnotations;

namespace ApiTarefas.Models
{
    public class Rotina
    {
        [Key]
        public int Id { get; set; }

        public string NomeRotina { get; set; }

        public List<Tarefa> Tarefas { get; set; }
            = new();
    }
}