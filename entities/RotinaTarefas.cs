public class RotinaTarefas
{
    public int Id { get; set; }
    public string NomeTarefa { get; set; }
    public string TipoTarefa { get; set; }
    public DateTime DataHorarioInicial { get; set; }
    public DateTime DataHorarioFinal { get; set; }
    public string DiasSemana { get; set; }
    public string Duracao { get; set; }
    public string Localizacao { get; set; }
    public string Alarme { get; set; }
    public bool WidgetAtivado { get; set; }
}