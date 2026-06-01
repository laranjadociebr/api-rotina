using ApiTarefas.Data;
using ApiTarefas.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // READ (Ler/Listar todas tarefas)
        // GET: api/tarefas
        // =========================
        [HttpGet]
        public IActionResult ListarTarefas()
        {
            return Ok(_context.Tarefas.ToList());
        }

        // =========================
        // READ (Buscar tarefa por ID)
        // GET: api/tarefas/1
        // =========================
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        // =========================
        // CREATE (Criar nova tarefa)
        // POST: api/tarefas
        // =========================
        [HttpPost]
        public IActionResult CriarTarefa([FromBody] Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);

            _context.SaveChanges();

            return Ok(tarefa);
        }

        // =========================
        // UPDATE (Atualizar tarefa)
        // PUT: api/tarefas/1
        // =========================
        [HttpPut("{id}")]
        public IActionResult AtualizarTarefa(int id, [FromBody] Tarefa tarefaAtualizada)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            tarefa.NomeTarefa = tarefaAtualizada.NomeTarefa;
            tarefa.TarefaSelecionada = tarefaAtualizada.TarefaSelecionada;
            tarefa.HorarioInicio = tarefaAtualizada.HorarioInicio;
            tarefa.HorarioFim = tarefaAtualizada.HorarioFim;
            tarefa.DiaSemana = tarefaAtualizada.DiaSemana;
            tarefa.Duracao = tarefaAtualizada.Duracao;

            _context.SaveChanges();

            return Ok(tarefa);
        }

        // =========================
        // DELETE (Remover tarefa)
        // DELETE: api/tarefas/1
        // =========================
        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            _context.Tarefas.Remove(tarefa);

            _context.SaveChanges();

            return Ok("Tarefa removida");
        }
    }
}