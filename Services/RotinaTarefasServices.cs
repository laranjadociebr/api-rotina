public class RotinaTarefasServices
{
    private readonly AppDbContext _context;

    public RotinaTarefasServices(AppDbContext context)
    {
        _context = context;
    }

    public List<RotinaTarefas> GetAll()
    {
        return _context.RotinasTarefas.ToList();
    }

    public RotinaTarefas GetById(int id)
    {
        return _context.RotinasTarefas.Find(id);
    }

    public void Create(RotinaTarefas rotina)
    {
        _context.RotinasTarefas.Add(rotina);
        _context.SaveChanges();
    }

    public void Update(RotinaTarefas rotina)
    {
        _context.RotinasTarefas.Update(rotina);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var rotina = _context.RotinasTarefas.Find(id);
        if (rotina != null)
        {
            _context.RotinasTarefas.Remove(rotina);
            _context.SaveChanges();
        }
    }
}