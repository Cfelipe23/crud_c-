using Microsoft.AspNetCore.Mvc;
using crud_c_.Modules.Lotes.Application;
using crud_c_.Modules.Lotes.Domain;

namespace crud_c_.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LotesController : ControllerBase
{
    private readonly ILoteRepository _repository;

    public LotesController(ILoteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lote>>> GetLotes()
    {
        var lotes = await _repository.GetAllAsync();
        return Ok(lotes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lote>> GetLote(int id)
    {
        var lote = await _repository.GetByIdAsync(id);
        if (lote == null)
        {
            return NotFound();
        }
        return Ok(lote);
    }

    [HttpPost]
    public async Task<ActionResult<Lote>> CreateLote(Lote lote)
    {
        await _repository.AddAsync(lote);
        return CreatedAtAction(nameof(GetLote), new { id = lote.Id }, lote);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLote(int id, Lote lote)
    {
        if (id != lote.Id)
        {
            return BadRequest();
        }

        var existingLote = await _repository.GetByIdAsync(id);
        if (existingLote == null)
        {
            return NotFound();
        }

        await _repository.UpdateAsync(lote);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLote(int id)
    {
        var lote = await _repository.GetByIdAsync(id);
        if (lote == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}