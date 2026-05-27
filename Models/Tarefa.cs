namespace ApiTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        public string NomeTarefa { get; set; } = "";

        public string TarefaSelecionada { get; set; } = "";

        public TimeSpan HorarioInicio { get; set; }

        public TimeSpan HorarioFim { get; set; }

        public string DiaSemana { get; set; } = "";

        public string Duracao { get; set; } = "";
    }
}