namespace ApiTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        public string NomeTarefa { get; set; }

        public string TarefaSelecionada { get; set; }

        public TimeSpan HorarioInicio { get; set; }

        public TimeSpan HorarioFim { get; set; }

        public List<string> DiaSemana { get; set; }
            = new();

        public string Duracao { get; set; }

        public int RotinaId { get; set; }

        public Rotina? Rotina { get; set; }
    }
}