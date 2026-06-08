using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ApiTarefas.Data;
using ApiTarefas.Models;

namespace ApiTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RotinasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RotinasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarRotina(
            [FromBody] Rotina rotina
        )
        {
            _context.Rotinas.Add(rotina);

            await _context.SaveChangesAsync();

            return Ok(rotina);
        }

        [HttpGet]
        public async Task<ActionResult<List<Rotina>>>
            ListarRotinas()
        {
            var rotinas = await _context.Rotinas
                .Include(r => r.Tarefas)
                .ToListAsync();

            return Ok(rotinas);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarRotina(int id)
        {
            var rotina = await _context.Rotinas.FindAsync(id);

            if (rotina == null)
            {
                return NotFound();
            }

            _context.Rotinas.Remove(rotina);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarRotina(
    int id,
    [FromBody] Rotina rotinaAtualizada
)
        {
            var rotinaExistente = await _context.Rotinas
                .Include(r => r.Tarefas)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rotinaExistente == null)
            {
                return NotFound();
            }

            // Atualiza nome da rotina
            rotinaExistente.NomeRotina =
                rotinaAtualizada.NomeRotina;

            // REMOVE tarefas antigas
            _context.Tarefas.RemoveRange(
                rotinaExistente.Tarefas
            );

            // ADICIONA tarefas novas
            rotinaExistente.Tarefas =
                rotinaAtualizada.Tarefas;

            await _context.SaveChangesAsync();

            return Ok(rotinaExistente);
        }

    }
}