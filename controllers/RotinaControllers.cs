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
    }
}