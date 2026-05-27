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

        [HttpGet]
        public IActionResult ListarTarefas()
        {
            var tarefas = _context.Tarefas.ToList();

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult CriarTarefa([FromBody] Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);

            _context.SaveChanges();

            return Ok(tarefa);
        }
    }
}